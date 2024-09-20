using JetBrains.Annotations;
using Photon.Client.StructWrapping;
using Quantum;
using Quantum.Mech;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Scripting;
using UnityEngine.UI;
using UnityEngine.Video;
using Button = UnityEngine.UI.Button;

enum UIState
{
    Waiting,
    Countdown,
    InBattle,
    GameOver,
    Result
}

[Serializable]
struct StateObjectPair
{
    public UIState State;
    public GameObject Object;
}

[Serializable]
public class SkillAudioData
{
    public SkillStatus status;
    public AudioClip castingClip;
    public AudioClip readyClip;
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
    public Chat ChatUI;
    
    [Header("Controller")]
    public List<SkillButton> weaponSkillButtons;
    public List<SkillAudioData> weaponSkillAudioDatas;

    [Header("Popup")]
    public GameObject resultPopup;

    [Header("Test")]
    public float testTime;
    private float timeRemaining; 

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
        QuantumEvent.Subscribe(this, (EventGameStateChanged e) => OnGameStateChanged(e));
        QuantumEvent.Subscribe(this, (EventOnMechanicTakeDamage e) => OnMechanicTakeDamage(e));

        foreach (var pair in _stateObjectPairs)
        {
            _stateObjectDictionary.Add(pair.State, pair.Object);
        }
        f = QuantumRunner.DefaultGame.Frames.Predicted;
    }

    private void OnMechanicTakeDamage(EventOnMechanicTakeDamage e)
    {
        UpdateHUD(e.Mechanic);
    }

    private void UpdateHUD(EntityRef Mechanic, bool isDeath = false)
    {
        var hud = playerHUDs.Find(x => Mechanic == x.entityRef);
        if (hud == null) return;

        if(isDeath)
        {
            hud.gameObject.SetActive(false);
            return;
        }

        hud.gameObject.SetActive(true);

        Status* playerStatus = f.Unsafe.GetPointer<Status>(Mechanic);
        float currentHealthPlayer = playerStatus->CurrentHealth.AsFloat;
        float maxHealthPlayer = f.FindAsset<StatusData>(playerStatus->StatusData.Id).MaxHealth.AsFloat;

        hud.UpdateHealth(currentHealthPlayer, maxHealthPlayer);
        
    }

    private void OnGameStateChanged(EventGameStateChanged e)
    {

        if (e.NewState == GameState.Countdown)
        {
            timer.titleText.gameObject.SetActive(true);    

            timer.titleText.text = "Wait for seconds..";
        }

        if (e.NewState == GameState.Game)
        {
            timer.titleText.gameObject.SetActive(false);    
        }
        

        if(e.NewState == GameState.Outro)
        {
            resultPopup.SetActive(true);
        }
    } 

    private void OnMechanicRespawn(EventOnMechanicRespawn e)
    {
        entityRefs.Add(e.Mechanic);
        
        var playerLink = f.Unsafe.GetPointer<PlayerLink>(e.Mechanic);
        Status* playerStatus = f.Unsafe.GetPointer<Status>(e.Mechanic);
        var currentHealthPlayer = playerStatus->CurrentHealth.AsFloat;
        var maxHealthPlayer = f.FindAsset<StatusData>(playerStatus->StatusData.Id).MaxHealth.AsFloat;

        var playerIndex = playerLink->PlayerRef._index - 1;
        playerHUDs[playerIndex].UpdateHealth(currentHealthPlayer, maxHealthPlayer);
    }

    private void OnMechanicDeath(EventOnMechanicDeath e)
    {
        UpdateHUD(e.Mechanic, true);

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
        var runtimePlayer = f.GetPlayerData(playerLink.PlayerRef);

        if (!_localEntityRef.IsValid && QuantumRunner.DefaultGame.PlayerIsLocal(playerLink.PlayerRef))
        {
            _localPlayerRef = playerLink.PlayerRef;
            _localEntityRef = e.Mechanic;
            ChatUI.Connect(runtimePlayer.PlayerNickname);
        }

        var players = QuantumRunner.DefaultGame.GetLocalPlayers(); 
        Status* playerStatus = f.Unsafe.GetPointer<Status>(e.Mechanic);

        float currentHealthPlayer = playerStatus->CurrentHealth.AsFloat;
        float maxHealthPlayer = f.FindAsset<StatusData>(playerStatus->StatusData.Id).MaxHealth.AsFloat;

        int playerIndex = playerLink.PlayerRef._index - 1;
        var playerInfoUI = playerInfoUIs[playerIndex];
        playerHUDs[playerIndex].SetPlayer(runtimePlayer.PlayerNickname, e.Mechanic);
        playerHUDs[playerIndex].UpdateHealth(currentHealthPlayer, maxHealthPlayer);
        
        if (playerInfoUI.IsInitialized) return;

        playerInfoUI.SetPlayer(runtimePlayer.PlayerNickname);

        var playableMechanic = f.Get<PlayableMechanic>(e.Mechanic);

        Nexus* Nexus = null;

        var nexusComponents = f.Unsafe.GetComponentBlockIterator<Nexus>();

        foreach (var nexus in nexusComponents)
        {
            if (nexus.Component->Team != playableMechanic.Team) continue;
            Nexus = nexus.Component;
        }

        if (Nexus == null) return;

        float currentHealthNexus = Nexus->CurrentHealth.AsFloat;
        playerInfoUI.Initialized(currentHealthNexus, playableMechanic.Team);
    }

    private void OnNexusTakeDamage(EventOnNexusTakeDamage e)
    {
        var nexus = f.Get<Nexus>(e.Nexus);

        foreach (var ui in playerInfoUIs)
        {
            if(ui.Team == nexus.Team)
            {
                ui.UpdateHealth(nexus.CurrentHealth.AsFloat);
                return;
            }
        }
    }

    private void Start()
    {
        settingButton.onClick.AddListener(OnSettingButtonClicked);
        settingPopupCloseButton.onClick.AddListener(OnSettingButtonClicked);
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
        UpdateLocalSkill();
        UpdateSkillSFX();
    }

    private void UpdateLocalSkill()
    {
        if (_localEntityRef.IsValid)
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
                        weaponSkillButtons[i].IsCooldown = true;
                        weaponSkillButtons[i].OnActivate(false);
                        break;
                    case SkillStatus.CoolTime:
                        weaponSkillButtons[i].UpdateCooltime(skill->RemainingCoolTime.AsFloat, skillData.CoolTime.AsFloat);
                        break;
                    case SkillStatus.Ready:
                        weaponSkillButtons[i].IsCooldown = false;
                        weaponSkillButtons[i].OnActivate(true);
                        break;
                }
            }
        }
    }

    private void UpdateSkillSFX()
    {
        foreach (var entityRef in entityRefs)
        {
            SkillInventory* skillInventory = f.Unsafe.GetPointer<SkillInventory>(entityRef);
            var skills = f.ResolveList(skillInventory->Skills);
            for (int i = 0; i < skills.Count; i++)
            {
                var skill = skills.GetPointer(i);
                var skillData = f.FindAsset(skill->SkillData);
                var audioData = weaponSkillAudioDatas[i];
                switch (skill->Status)
                {
                    case SkillStatus.Casting:
                        if (weaponSkillAudioDatas[i].status == SkillStatus.Casting) return;
                        audioData.status = SkillStatus.Casting;
                        AudioManager.Instance.PlaySfx(audioData.castingClip);
                        break;
                    case SkillStatus.Ready:
                        if (weaponSkillAudioDatas[i].status == SkillStatus.Ready) return;
                        audioData.status = SkillStatus.Ready;
                        AudioManager.Instance.PlaySfx(audioData.readyClip);
                        break;
                }
            }
        }
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