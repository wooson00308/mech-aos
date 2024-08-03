using JetBrains.Annotations;
using Quantum;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
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

public class GameUI : QuantumCallbacks
{
    [SerializeField] private List<StateObjectPair> _stateObjectPairs = new();
    private Dictionary<UIState, GameObject> _stateObjectDictionary = new();

    [Header("Header")]
    public List<PlayerInfoUI> playerInfoUIs;
    public TimerUI timer;
    public Button settingButton;
    public Button settingPopupCloseButton;
    public GameObject settingPanel;

    [Header("Test")]
    public float testTime;
    private float timeRemaining; 
    private bool isTimerRunning = false;

    private void Awake()
    {
        QuantumEvent.Subscribe(this, (EventOnMechanicCreated e) => OnMechanicCreated(e));
        QuantumEvent.Subscribe(this, (EventOnMechanicTakeDamage e) => OnMechanicTakeDamage(e));

        foreach (var pair in _stateObjectPairs)
        {
            _stateObjectDictionary.Add(pair.State, pair.Object);
        }
    }

    private void OnMechanicCreated(EventOnMechanicCreated e)
    {
        var frame = QuantumRunner.Default.Game.Frames.Predicted; 
        
        int i = 0;
        foreach (var playerRef in QuantumRunner.DefaultGame.GetLocalPlayers())
        {
            var player = frame.GetPlayerData(playerRef);
            playerInfoUIs[i].SetPlayer(player.PlayerNickname);
        }
    }

    private void OnMechanicTakeDamage(EventOnMechanicTakeDamage e)
    {

    }

    private void Start()
    {
        settingButton.onClick.AddListener(OnSettingButtonClicked);
        settingPopupCloseButton.onClick.AddListener(OnSettingButtonClicked);

        SetTimer(testTime);
    }

    private void Update()
    {
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
