using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Quantum;
using QuantumUser;
using UnityEngine.SceneManagement;
using Quantum.Mech;
using Photon.Deterministic;

public class EventSubscriptions : MonoBehaviour
{
	private void Awake()
	{
        QuantumEvent.Subscribe<EventShutdown>(this, evt =>
		{
            // disconnect if we're connected, or just perform the logic if we're in singleplayer
            Shutdown();
        });

		QuantumEvent.Subscribe(this, (EventGameStateChanged evt) =>
		{
			Debug.Log($"Game State: {evt.OldState} => {evt.NewState}");

			if (evt.NewState == GameState.Game)
			{
				if (Matchmaker.Client?.CurrentRoom != null) Matchmaker.Client.CurrentRoom.IsOpen = false;

                var f = QuantumRunner.DefaultGame.Frames.Predicted;
                GameStateSystem.SetStateDelayed(f, GameState.Outro, FP._180);
            }

			if(evt.NewState == GameState.Outro)
			{
                if (isShutdown) return;
                var f = QuantumRunner.DefaultGame.Frames.Predicted;
                GameStateSystem.SetStateDelayed(f, GameState.Off, FP._10);
                StartCoroutine(ProcessShutdown());
            }
        });
	}

    bool isShutdown;

    private IEnumerator ProcessShutdown()
    {
        isShutdown = true;

        yield return new WaitForSeconds(10f);
        Shutdown();

        isShutdown = false;
    }

	private void Shutdown()
	{
        if (Matchmaker.Client.IsConnected)
            Matchmaker.Client.Disconnect();
        else
            Matchmaker.Instance.OnDisconnected(default);
    }
}
