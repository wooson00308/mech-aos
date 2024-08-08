using Quantum;
using TMPro;
using UnityEngine;

public class TimerUI : QuantumMonoBehaviour
{
    public TMP_Text timerText;
    public TMP_Text titleText;

    private float timeRemaining;

    public void SetTimerDisplay(float time)
    {
        timeRemaining = time;
    }

    private void UpdateTimerDisplay(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        timerText.text = $"{minutes:D2}:{seconds:D2}";
    }

    private void FixedUpdate()
    {
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            UpdateTimerDisplay(timeRemaining);
        }
        else
        {
            timeRemaining = 0;
        }
    }
}
