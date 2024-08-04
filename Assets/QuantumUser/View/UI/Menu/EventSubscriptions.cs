using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Quantum;
using QuantumUser;

public class EventSubscriptions : MonoBehaviour
{
	private void Awake()
	{
		QuantumEvent.Subscribe(this, (EventGameStateChanged evt) =>
		{
			Debug.Log($"Game State: {evt.OldState} => {evt.NewState}");

			if (evt.NewState == GameState.Game)
			{
				if (Matchmaker.Client?.CurrentRoom != null) Matchmaker.Client.CurrentRoom.IsOpen = false;
			}

		});

	}
}
