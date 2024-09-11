using Quantum;
using Quantum.Mech;
using TMPro;
using UnityEngine;

namespace Quantum
{
    public unsafe class TimerUI : QuantumSceneViewComponent
    {
        public TMP_Text timerText;
        public TMP_Text titleText;

        private GameState currentState;
        public override void OnActivate(Frame frame)
        {
            QuantumEvent.Subscribe(this, (EventGameStateChanged e) => OnGameStateChanged(e));
        }

        public override void OnUpdateView()
        {
            if (currentState == GameState.Game)
                UpdateTimerDisplay(VerifiedFrame.Global->clock.AsFloat);
            else
                UpdateTimerDisplay(VerifiedFrame.Global->StateTimer.AsFloat);
        }

        private void OnGameStateChanged(EventGameStateChanged e)
        {
            currentState = e.NewState;
        } 
        
        private void UpdateTimerDisplay(float time)
        {
            int minutes = Mathf.FloorToInt(time / 60);
            int seconds = Mathf.FloorToInt(time % 60);
            timerText.text = $"{minutes:D2}:{seconds:D2}";
        }
    }
}

