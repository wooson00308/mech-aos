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
        QuantumEvent.Subscribe(this, (EventOnPlayerTeamWin e) => OnPlayerTeamWin(e));
    }

    private void OnPlayerTeamWin(EventOnPlayerTeamWin e)
    {
        var localEntity = gameUI.LocalEntityRef;
        var frame = QuantumRunner.DefaultGame.Frames.Predicted;
        var player = frame.Get<PlayableMechanic>(localEntity);

        //VictoryBG.SetActive(player.Team == e.team);
        DefeatBG.SetActive(player.Team != e.team);
    }
}
