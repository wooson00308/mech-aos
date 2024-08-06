using JetBrains.Annotations;
using Photon.Client.StructWrapping;
using Quantum;
using Quantum.Mech;
using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Scripting;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;

enum UIState
{
    Waiting,
    Countdown,
    InBattle,
    GameOver,
    Result
}

[System.Serializable]
struct StateObjectPair
{
    public UIState State;
    public GameObject Object;
}

[Preserve]
public unsafe class GameUI : QuantumViewComponent<CustomViewContext>
{
    [SerializeField] private List<StateObjectPair> _stateObjectPairs = new();
    private Dictionary<UIState, GameObject> _stateObjectDictionary = new();

    [Header("Header")]
    public List<PlayerInfoUI> playerInfoUIs;
    public List<CharacterHUD> playerHUDs;
    public TimerUI timer;
    public Button settingButton;
    public Button settingPopupCloseButton;
    public GameObject settingPanel;
    public KillLogStorage killLog;

    [Header("Controller")]
    public List<SkillButton> weaponSkillButtons;

    [Header("Test")]
    public float testTime;
    private float timeRemaining; 
    private bool isTimerRunning = false;

    private Frame f;

    private List<EntityRef> entityRefs = new();

    private Camera _camera;

    public Vector3 hudOffset;

    private PlayerRef _localPlayerRef;
    private EntityRef _localEntityRef = default;

    private void Awake()
    {
        _camera = FindObjectOfType<Camera>();
        QuantumEvent.Subscribe(this, (EventOnMechanicCreated e) => OnMechanicCreated(e));
        QuantumEvent.Subscribe(this, (EventOnNexusTakeDamage e) => OnNexusTakeDamage(e));
        QuantumEvent.Subscribe(this, (EventOnMechanicDeath e) => OnMechanicDeath(e));
        QuantumEvent.Subscribe(this, (EventOnMechanicRespawn e) => OnMechanicRespawn(e));

        foreach (var pair in _stateObjectPairs)
        {
            _stateObjectDictionary.Add(pair.State, pair.Object);
        }
        f = QuantumRunner.DefaultGame.Frames.Verified;
    }

    private void OnMechanicRespawn(EventOnMechanicRespawn e)
    {
        entityRefs.Add(e.Mechanic);
    }

    private void OnMechanicDeath(EventOnMechanicDeath e)
    {
        var killedPlayerLink = f.Get<PlayerLink>(e.Mechanic);
        var runtimeKilledPlayer = f.GetPlayerData(killedPlayerLink.PlayerRef);

        entityRefs.Remove(e.Mechanic);

        var killerPlayerLink = f.Get<PlayerLink>(e.Killer);
        var runtimeKillerPlayer = f.GetPlayerData(killerPlayerLink.PlayerRef);

        killLog.GetKillLogUI().Show(runtimeKilledPlayer.PlayerNickname, runtimeKillerPlayer.PlayerNickname);
    }

    private void OnMechanicCreated(EventOnMechanicCreated e)
    {
        var playerLink = f.Get<PlayerLink>(e.Mechanic);

        if (_localEntityRef.IsValid && QuantumRunner.DefaultGame.PlayerIsLocal(playerLink.PlayerRef))
        {
            _localPlayerRef = playerLink.PlayerRef;
            _localEntityRef = e.Mechanic;
        }

        var players = QuantumRunner.DefaultGame.GetLocalPlayers();

        int index = 0;
        foreach(var playerRef in players)
        {
            var runtimePlayer = f.GetPlayerData(playerRef);
            entityRefs.Add(e.Mechanic);

            // Player Setting
            Status* playerStatus = f.Unsafe.GetPointer<Status>(e.Mechanic);

            float currentHealthPlayer = playerStatus->CurrentHealth.AsFloat;
            float maxHealthPlayer = f.FindAsset<StatusData>(playerStatus->StatusData.Id).MaxHealth.AsFloat;

            playerHUDs[index].SetPlayer(runtimePlayer.PlayerNickname, e.Mechanic);
            playerHUDs[index].UpdateHealth(currentHealthPlayer, maxHealthPlayer);

            // Nexus Setting
            var playableMechanic = f.Get<PlayableMechanic>(e.Mechanic);

            Nexus* Nexus = null;

            foreach (var nexus in f.Unsafe.GetComponentBlockIterator<Nexus>())
            {
                if (nexus.Component->Team != playableMechanic.Team) continue;
                Nexus = nexus.Component;
            }

            if (Nexus == null) return;

            float currentHealthNexus = Nexus->CurrentHealth.AsFloat;

            playerInfoUIs[index].SetPlayer(runtimePlayer.PlayerNickname);
            playerInfoUIs[index].UpdateHealth(currentHealthNexus, currentHealthNexus);
            index++;
        }
    }

    private void OnNexusTakeDamage(EventOnNexusTakeDamage e)
    {
        var playerLink = f.Get<PlayerLink>(e.Nexus);
        Status* nexusStatus = f.Unsafe.GetPointer<Status>(e.Nexus);

        float currentHealth = nexusStatus->CurrentHealth.AsFloat;
        float maxHealth = f.FindAsset<StatusData>(nexusStatus->StatusData.Id).MaxHealth.AsFloat;
        playerInfoUIs[playerLink.PlayerRef._index - 1].UpdateHealth(currentHealth, maxHealth);
    }

    private void Start()
    {
        settingButton.onClick.AddListener(OnSettingButtonClicked);
        settingPopupCloseButton.onClick.AddListener(OnSettingButtonClicked);

        SetTimer(testTime);
    }

    private void FixedUpdate()
    {
        foreach (var entity in entityRefs)
        {
            var transform3D = f.Get<Transform3D>(entity);
            var mechPosition = transform3D.Position.ToUnityVector3();

            var hud = playerHUDs.Find(x => entity == x.entityRef);

            if (hud == null) return;

            var hudRect = hud.GetComponent<RectTransform>();

            var screenPosition = _camera.WorldToScreenPoint(mechPosition);

            if (screenPosition.z > 0 && screenPosition.x > 0 && screenPosition.x < Screen.width && screenPosition.y > 0 && screenPosition.y < Screen.height)
            {
                hudRect.gameObject.SetActive(true);
                hudRect.position = screenPosition + hudOffset;
            }
            else
            {
                hudRect.gameObject.SetActive(false);
            }
        }
    }

    private void Update()
    {
        if(_localEntityRef.IsValid)
        {
            SkillInventory* skillInventory = f.Unsafe.GetPointer<SkillInventory>(_localEntityRef);
            var skills = f.ResolveList(skillInventory->Skills);
            for (int i = 0; i < skills.Count; i++)
            {
                var skill = skills.GetPointer(i);
                var skillData = f.FindAsset(skill->SkillData);
                switch (skill->Status)
                {
                    case SkillStatus.Casting:
                        weaponSkillButtons[i].OnActivate(false);
                        break;
                    case SkillStatus.CoolTime:
                        weaponSkillButtons[i].UpdateCooltime(skill->RemainingCoolTime.AsFloat, skillData.CoolTime.AsFloat);
                        break;
                    case SkillStatus.Ready:
                        weaponSkillButtons[i].OnActivate(true);
                        break;
                }
            }
        }

        // Test Code
        if (isTimerRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                timer.UpdateTimerDisplay(timeRemaining);
            }
            else
            {
                timeRemaining = 0; 
                isTimerRunning = false;
                TimerEnded(); 
            }
        }
    }

    private void UpdateHUD(Vector3 position)
    {

    } 

    public void SetTimer(float time)
    {
        timeRemaining = time;
        timer.UpdateTimerDisplay(time);

        isTimerRunning = true;
    }

    private void TimerEnded()
    {
        Debug.Log("Time's up!");
    }

    public void OnSettingButtonClicked()
    {
        settingPanel.SetActive(!settingPanel.activeSelf);
    }
}
