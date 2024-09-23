using Quantum;
using Quantum.Mech;
using System;
using System.Collections.Generic;
using TMPro;
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

[Serializable]
struct StateObjectPair
{
    public UIState State;
    public GameObject Object;
}

[Serializable]
public class SkillAudioData
{
    public int index;
    public AudioClip castingClip;
    public AudioClip readyClip;
}

[Serializable]
public class SkillIcon
{
    public int id;
    public Sprite icon;
}

[Preserve]
public unsafe class GameUI : QuantumViewComponent<CustomViewContext>
{
    [SerializeField] private List<StateObjectPair> _stateObjectPairs = new();
    private Dictionary<UIState, GameObject> _stateObjectDictionary = new();

    public List<SkillIcon> skillIconList;

    [Header("Header")]
    public List<PlayerInfoUI> playerInfoUIs;
    public List<CharacterHUD> playerHUDs;
    public List<TMP_Text> playerResultTexts;
    public TimerUI timer;
    public Button fixButton;
    public Button settingButton;
    public Button settingPopupCloseButton;
    public GameObject settingPanel;
    public KillLogStorage killLog;
    public Chat ChatUI;
    public TMP_Text ResultNotiText; 
    
    [Header("Controller")]
    public List<SkillButton> weaponSkillButtons;
    public List<SkillAudioData> weaponSkillAudioDatas;

    [Header("Popup")]
    public GameObject resultPopup;
    public FixPopupUI fixPopup;

    [Header("----------Setting Screen-----------")]
    [SerializeField] private TMP_Dropdown regionDropdown;
    [SerializeField] private TMP_Dropdown graphicQualityDropdown;
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider sfxSlider;

    [Header("Test")]
    public float testTime;
    private float timeRemaining; 

    private Frame f;

    private List<EntityRef> entityRefs = new();

    private Camera _camera;

    public Vector3 hudOffset;

    private PlayerRef _localPlayerRef;
    private EntityRef _localEntityRef = default;

    private Dictionary<string, GameObject> _entityObjDic = new();

    public PlayerRef GetPlayerRef()
    {
        return _localPlayerRef;
    }

    public EntityRef LocalEntityRef => _localEntityRef;

    private void Awake()
    {
        _camera = FindObjectOfType<Camera>();
        QuantumEvent.Subscribe(this, (EventOnMechanicCreated e) => OnMechanicCreated(e));
        QuantumEvent.Subscribe(this, (EventOnNexusTakeDamage e) => OnNexusTakeDamage(e));
        QuantumEvent.Subscribe(this, (EventOnMechanicDeath e) => OnMechanicDeath(e));
        QuantumEvent.Subscribe(this, (EventOnMechanicRespawn e) => OnMechanicRespawn(e));
        QuantumEvent.Subscribe(this, (EventGameStateChanged e) => OnGameStateChanged(e));
        QuantumEvent.Subscribe(this, (EventOnMechanicTakeDamage e) => OnMechanicTakeDamage(e));
        QuantumEvent.Subscribe(this, (EventUseSkill e) => OnUseSkill(e));
        QuantumEvent.Subscribe(this, (EventOnChangeWeapon e) => OnChangeWeapon(e));
        QuantumEvent.Subscribe(this, (EventOnEnableFix e) => OnEnableFix(e));

        foreach (var pair in _stateObjectPairs)
        {
            _stateObjectDictionary.Add(pair.State, pair.Object);
        }
        f = QuantumRunner.DefaultGame.Frames.Predicted;
    }

    private void OnEnableFix(EventOnEnableFix e)
    {
        if (e.mechanic.ToString() != _localEntityRef.ToString()) return;

        var mechanic = f.Get<PlayableMechanic>(e.mechanic);
        var nexusIndentifier = f.Get<NexusIdentifier>(e.nexusIndentifier);
        if (mechanic.Team != nexusIndentifier.Team) return;

        fixButton.gameObject.SetActive(e.isEnabled);
    }

    private void OnChangeWeapon(EventOnChangeWeapon e)
    {
        var weaponData = f.FindAsset<PrimaryWeaponData>(e.weapon.WeaponData.Id);
        var skills = weaponData.Skills;

        int index = 0;

        foreach(var skill in skills)
        {
            int buttonIndex = index + 1;

            if (index == 0) buttonIndex = 1;
            if (index == 1) buttonIndex = 2;
            if (index == 2) buttonIndex = 0;
            var skillButton = weaponSkillButtons[buttonIndex];
            var id = skills[index++];
            var skillIcon = skillIconList.Find(x => x.id == id);
            skillButton.icon.sprite = skillIcon.icon;
            skillButton.id = id;
        }
    }

    private void OnMechanicTakeDamage(EventOnMechanicTakeDamage e)
    {
        UpdateHUD(e.Mechanic);
    }

    private void UpdateLevelHUD(EntityRef entity)
    {
        var hud = playerHUDs.Find(x => entity == x.entityRef);
        if (hud == null) return;

        int level = int.Parse(hud.level.text);

        hud.level.text = $"{level + 1}";

        if(level == 2)
        {
            weaponSkillButtons[1].levelLockImage.enabled = false;
        }
        if (level == 3)
        {
            weaponSkillButtons[2].levelLockImage.enabled = false;
        }
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
            resultPopup.GetComponent<CanvasGroup>().alpha = 1;
            timer.timerText = ResultNotiText;
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

        fixPopup.Levelup(e.Killer);
        UpdateLevelHUD(e.Killer);

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
        playerResultTexts[playerIndex].text = runtimePlayer.PlayerNickname;
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
        InitGraphicsDropdown();
        bgmSlider.SetValueWithoutNotify(AudioManager.Instance.bgmVol);
        sfxSlider.SetValueWithoutNotify(AudioManager.Instance.sfxVol);

        bgmSlider.onValueChanged.AddListener((value) =>SetBgmVolume(value));
        sfxSlider.onValueChanged.AddListener((value) =>SetSfxVolume(value));

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
        //UpdateSkillSFX();
    }

    private void UpdateLocalSkill()
    {
        if (_localEntityRef.IsValid)
        {
            SkillInventory* skillInventory = f.Unsafe.GetPointer<SkillInventory>(_localEntityRef);
            var skills = f.ResolveList(skillInventory->Skills);
            for (int i = 0; i < 2; i++)
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

    private void OnUseSkill(EventUseSkill e)
    {
        var entity = e.Owner;
        var skill = e.skill;
        var weapon = e.weapon;
        var index = e.index.AsInt;
        var audioData = weaponSkillAudioDatas.Find(x => x.index == e.index.AsInt);

        GameObject unit;

        string entitiyId = entity.ToString();

        if (_entityObjDic.TryGetValue(entitiyId, out GameObject getUnit))
        {
            unit = getUnit;
        }
        else
        {
            unit = GameObject.Find(entitiyId);
            if (unit == null) return;
            _entityObjDic.Add(entitiyId, unit);
        }

        if (audioData == null) return;

        switch (skill.Status)
        {
            case SkillStatus.CoolTime:
                if (audioData.castingClip == null) return;
                AudioManager.Instance.PlaySfx(audioData.castingClip, unit);
                break;
            case SkillStatus.Ready:
                if (audioData.readyClip == null) return;
                AudioManager.Instance.PlaySfx(audioData.readyClip, unit);
                break;
        }
    }
    private void TimerEnded()
    {
        Debug.Log("Time's up!");
    }

    bool _isShowSettings;

    public void OnSettingButtonClicked()
    {
        _isShowSettings = !_isShowSettings;
        settingPanel.GetComponent<CanvasGroup>().alpha = _isShowSettings ? 1 : 0;
        settingPanel.GetComponent<CanvasGroup>().blocksRaycasts = _isShowSettings;
    }

    public void InitGraphicsDropdown()
    {
        string[] names = QualitySettings.names;
        List<string> options = new List<string>();

        for (int i = 0; i < names.Length; i++)
        {
            options.Add(names[i]);
        }
        graphicQualityDropdown.AddOptions(options);
        QualitySettings.SetQualityLevel(graphicQualityDropdown.options.Count - 1);
        graphicQualityDropdown.value = graphicQualityDropdown.options.Count - 1;
    }


    public void SetGraphicsQuality()
    {
        QualitySettings.SetQualityLevel(graphicQualityDropdown.value);
    }

    public void SetBgmVolume(float volume)
    {
        AudioManager.Instance.BgmVol(volume);
    }

    public void SetSfxVolume(float volume)
    {
        AudioManager.Instance.SfxVol(volume);

    }
}
