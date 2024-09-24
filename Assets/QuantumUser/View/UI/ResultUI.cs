using Quantum;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultUI : QuantumMonoBehaviour
{
    public GameUI gameUI;

    //public GameObject VictoryBG;
    public GameObject DefeatBG;

    private void Awake()
    {
        // QuantumEvent.Subscribe(this, (EventOnPlayerTeamWin e) => OnPlayerTeamWin(e));
    }

    public void OnPlayerTeamWin(Team team)
    {
        var localEntity = gameUI.LocalEntityRef;
        var frame = QuantumRunner.DefaultGame.Frames.Predicted;
        var player = frame.Get<PlayableMechanic>(localEntity);

        DefeatBG.SetActive(player.Team != team);
    }
}
