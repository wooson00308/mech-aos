using Quantum;
using Quantum.Mech;
using QuantumUser;
using System;
using System.Collections.Generic;
using Photon.Deterministic;
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
public unsafe class GameUI : QuantumSceneViewComponent<CustomViewContext>
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
    public TextMeshProUGUI ammo;
    
    [Header("Popup")]
    public ResultUI resultPopup;
    public FixPopupUI fixPopup;

    [Header("----------Setting Screen-----------")]
    [SerializeField] private TMP_Dropdown regionDropdown;
    [SerializeField] private TMP_Dropdown graphicQualityDropdown;
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider sfxSlider;

    [Header("Test")]
    public float testTime;
    private float timeRemaining;
    private List<EntityRef> entityRefs = new();

    private PlayerRef _localPlayerRef;
    private Camera _camera;

    public Vector3 hudOffset;
    private Dictionary<string, GameObject> _entityObjDic = new();

    private void Fix(EventFix e)
    {
        if(e.Owner == ViewContext.LocalEntityRef)
        {
            fixPopup.ShowAndClose();
        }
    }

    private void OnEnableFix(EventOnEnableFix e)
    {
        if (e.mechanic!= ViewContext.LocalEntityRef) return;

        var mechanic = PredictedFrame.Get<PlayableMechanic>(e.mechanic);
        var nexusIndentifier = PredictedFrame.Get<NexusIdentifier>(e.nexusIndentifier);
        if (mechanic.Team != nexusIndentifier.Team) return;

        fixButton.gameObject.SetActive(e.isEnabled);

        if(!e.isEnabled)
        {
            fixPopup.GetComponent<CanvasGroup>().alpha = 0;
            fixPopup.GetComponent<CanvasGroup>().blocksRaycasts = false;
        }
    }

    private void OnChangeWeapon(EventOnChangeWeapon e)
    {
        if (e.Mechanic != ViewContext.LocalEntityRef) return;
        var weaponData = PredictedFrame.FindAsset<PrimaryWeaponData>(e.weapon.WeaponData.Id);
        var skills = weaponData.Skills;

        int index = 0;

        foreach(var skill in skills)
        {
            int buttonIndex = index + 1;
            if (index == 0) buttonIndex = 1;
            if (index == 1) buttonIndex = 0;
            if (index == 2) buttonIndex = 2;
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
        hud.level.text = $"{++level}";

        if (entity!= ViewContext.LocalEntityRef) return;

        if (level == 2)
        {
            weaponSkillButtons[1].levelLockImage.gameObject.SetActive(false);
        }
        if (level == 3)
        {
            weaponSkillButtons[2].levelLockImage.gameObject.SetActive(false);
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
        
        Status* playerStatus = PredictedFrame.Unsafe.GetPointer<Status>(Mechanic);
        float currentHealthPlayer = playerStatus->CurrentHealth.AsFloat;
        float maxHealthPlayer = PredictedFrame.FindAsset<StatusData>(playerStatus->StatusData.Id).MaxHealth.AsFloat;

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
            resultPopup.GetComponent<CanvasGroup>().blocksRaycasts = true;
            
            var nexusBlockIterator = PredictedFrame.Unsafe.GetComponentBlockIterator<Nexus>();
            FP maxCurrentHealth = 0;
            Team team = Team.Blue;
            foreach (var entityComponentPointerPair in nexusBlockIterator)
            {
                if (entityComponentPointerPair.Component->IsDestroy) continue;
                var nexus = PredictedFrame.Get<Nexus>(entityComponentPointerPair.Entity);
                if (nexus.CurrentHealth > maxCurrentHealth)
                {
                    maxCurrentHealth = nexus.CurrentHealth;
                    team = nexus.Team;
                }
            }
            resultPopup.OnPlayerTeamWin(team);
            timer.timerText = ResultNotiText;
        }
    } 

    private void OnMechanicRespawn(EventOnMechanicRespawn e)
    {
        entityRefs.Add(e.Mechanic);
        
        var playerLink = PredictedFrame.Unsafe.GetPointer<PlayerLink>(e.Mechanic);
        Status* playerStatus = PredictedFrame.Unsafe.GetPointer<Status>(e.Mechanic);
        var currentHealthPlayer = playerStatus->CurrentHealth.AsFloat;
        var maxHealthPlayer = PredictedFrame.FindAsset<StatusData>(playerStatus->StatusData.Id).MaxHealth.AsFloat;

        var playerIndex = playerLink->PlayerRef._index - 1;
        playerHUDs[playerIndex].UpdateHealth(currentHealthPlayer, maxHealthPlayer);
    }

    private void OnMechanicDeath(EventOnMechanicDeath e)
    {
        UpdateHUD(e.Mechanic, true);

        var killedPlayerLink = PredictedFrame.Get<PlayerLink>(e.Mechanic);
        var runtimeKilledPlayer = PredictedFrame.GetPlayerData(killedPlayerLink.PlayerRef);

        entityRefs.Remove(e.Mechanic);

        var killerPlayerLink = PredictedFrame.Get<PlayerLink>(e.Killer);
        var runtimeKillerPlayer = PredictedFrame.GetPlayerData(killerPlayerLink.PlayerRef);

        fixPopup.Levelup(e.Killer);
        UpdateLevelHUD(e.Killer);

        killLog.GetKillLogUI().Show(runtimeKilledPlayer.PlayerNickname, runtimeKillerPlayer.PlayerNickname);
    }

    private void OnMechanicCreated(EventOnMechanicCreated e)
    {
        var playerLink = PredictedFrame.Get<PlayerLink>(e.Mechanic);
        var runtimePlayer = PredictedFrame.GetPlayerData(playerLink.PlayerRef);

        if (!ViewContext.LocalEntityRef.IsValid && QuantumRunner.DefaultGame.PlayerIsLocal(playerLink.PlayerRef))
        {
            _localPlayerRef = playerLink.PlayerRef;
            ChatUI.Connect(runtimePlayer.PlayerNickname);
        }

        var players = QuantumRunner.DefaultGame.GetLocalPlayers(); 
        Status* playerStatus = PredictedFrame.Unsafe.GetPointer<Status>(e.Mechanic);

        float currentHealthPlayer = playerStatus->CurrentHealth.AsFloat;
        float maxHealthPlayer = PredictedFrame.FindAsset<StatusData>(playerStatus->StatusData.Id).MaxHealth.AsFloat;

        int playerIndex = playerLink.PlayerRef._index - 1;
        var playerInfoUI = playerInfoUIs[playerIndex];
        playerResultTexts[playerIndex].text = runtimePlayer.PlayerNickname;
        playerHUDs[playerIndex].SetPlayer(runtimePlayer.PlayerNickname, e.Mechanic);
        playerHUDs[playerIndex].UpdateHealth(currentHealthPlayer, maxHealthPlayer);
        
        if (playerInfoUI.IsInitialized) return;

        playerInfoUI.SetPlayer(runtimePlayer.PlayerNickname);

        var playableMechanic = PredictedFrame.Get<PlayableMechanic>(e.Mechanic);

        Nexus* Nexus = null;

        var nexusComponents = PredictedFrame.Unsafe.GetComponentBlockIterator<Nexus>();

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
        var nexus = PredictedFrame.Get<Nexus>(e.Nexus);

        foreach (var ui in playerInfoUIs)
        {
            if(ui.Team == nexus.Team)
            {
                ui.UpdateHealth(nexus.CurrentHealth.AsFloat);
                return;
            }
        }
    }

    public override void OnInitialize()
    {
        base.OnInitialize();
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
        QuantumEvent.Subscribe(this, (EventFix e) => { Fix(e); });


        foreach (var pair in _stateObjectPairs)
        {
            _stateObjectDictionary.Add(pair.State, pair.Object);
        }
    }

    public override void OnActivate(Frame frame)
    {
        base.OnActivate(frame);
        InitGraphicsDropdown();
        bgmSlider.SetValueWithoutNotify(AudioManager.Instance.bgmVol);
        sfxSlider.SetValueWithoutNotify(AudioManager.Instance.sfxVol);

        bgmSlider.onValueChanged.AddListener((value) =>SetBgmVolume(value));
        sfxSlider.onValueChanged.AddListener((value) =>SetSfxVolume(value));

        settingButton.onClick.AddListener(OnSettingButtonClicked);
        settingPopupCloseButton.onClick.AddListener(OnSettingButtonClicked);
    }
    
    private void UpdateLocalSkill()
    {
        if (ViewContext.LocalEntityRef.IsValid)
        {
            if (ViewContext.LocalEntityRef == EntityRef.None || !PredictedFrame.Has<Status>(ViewContext.LocalEntityRef)) return;
            var skillInventory = PredictedFrame.Unsafe.GetPointer<SkillInventory>(ViewContext.LocalEntityRef);
            if (skillInventory == null) return;
            var skills = PredictedFrame.ResolveList(skillInventory->Skills);
            for (int i = 0; i < skills.Count; i++)
            {
                int buttonIndex = i;
                var skill = skills.GetPointer(i);
                var skillData = PredictedFrame.FindAsset(skill->SkillData);

                if (buttonIndex == 0 || buttonIndex == 3)
                {
                    buttonIndex = 0;
                }
                else if (buttonIndex == 1 || buttonIndex == 4)
                {
                    buttonIndex = 1;
                }
                else if (buttonIndex == 5)
                {
                    buttonIndex = 2;
                }
                else continue;

                switch (skill->Status)
                {
                    case SkillStatus.Casting:
                        weaponSkillButtons[buttonIndex].IsCooldown = true;
                        weaponSkillButtons[buttonIndex].OnActivate(false);
                        break;
                    case SkillStatus.CoolTime:
                        weaponSkillButtons[buttonIndex].UpdateCooltime(skill->RemainingCoolTime.AsFloat, skillData.CoolTime.AsFloat);
                        break;
                    case SkillStatus.Ready:
                        weaponSkillButtons[buttonIndex].IsCooldown = false;
                        weaponSkillButtons[buttonIndex].OnActivate(true);
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
    public void Shutdown()
    {
        if (Matchmaker.Client.IsConnected)
            Matchmaker.Client.Disconnect();
        else
            Matchmaker.Instance.OnDisconnected(default);
    }

    public void OnExitButton()
    {
        Application.Quit();
    }


    public override unsafe void OnLateUpdateView()
    {
        
        base.OnLateUpdateView();
        UpdateLocalSkill();

        var frame = QuantumRunner.DefaultGame.Frames.Predicted;
        if (ViewContext != null && ViewContext?.LocalEntityRef != null)
        {
            ammo.enabled = PredictedFrame.Unsafe.TryGetPointer<WeaponInventory>(ViewContext.LocalEntityRef, out var weaponInventory);
            if (!ammo.enabled) return;
            
            var weapons = frame.ResolveList(weaponInventory->Weapons);
            var currentWeapon = weapons.GetPointer(weaponInventory->CurrentWeaponIndex);
            var weaponData = frame.FindAsset<WeaponData>(currentWeapon->WeaponData.Id);
            ammo.SetText($"{currentWeapon->CurrentAmmo} / {weaponData.MaxAmmo}");
        }
        else
        {
            ammo.enabled = false;
        }

        foreach (var entity in entityRefs)
        {
            // entity�� ��ȿ���� ���� Ȯ��
            if (!PredictedFrame.Exists(entity))
            {
                Debug.LogWarning($"Entity {entity}�� �������� �ʽ��ϴ�.");
                continue; // ��ȿ���� ������ ���� entity�� �Ѿ
            }

            var transform3D = PredictedFrame.Get<Transform3D>(entity);
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
}
