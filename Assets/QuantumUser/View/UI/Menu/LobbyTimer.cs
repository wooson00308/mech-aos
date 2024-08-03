using Quantum;
using TMPro;
using UnityEngine;

namespace QuantumUser
{
    public unsafe class LobbyTimer : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI sessionWaitingText;
        private void Update()
        {
            if (QuantumRunner.Default == null) return;
            if (QuantumRunner.Default.Game.Frames.Verified.Global->CurrentState != Quantum.GameState.Lobby) return;

            var clock = QuantumRunner.Default.Game.Frames.Verified.Global->clock.AsInt;
            sessionWaitingText.text = $"Searching for opponent… {clock:00}";
        }
    }
}