using Quantum;
using TMPro;
using UnityEngine;

public class TimerUI : QuantumMonoBehaviour
{
    public TMP_Text timerText;
    public TMP_Text titleText;

    public void UpdateTimerDisplay(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        timerText.text = $"{minutes:D2}:{seconds:D2}";
    }
}
