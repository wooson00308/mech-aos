using JetBrains.Annotations;
using Quantum;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;

public class GameUI : QuantumMonoBehaviour
{
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
