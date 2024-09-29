using Quantum;
using System.Collections;
using System.Collections.Generic;
using Quantum.Mech;
using UnityEngine;

public class ResultUI : QuantumSceneViewComponent<CustomViewContext>
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
        var localEntity = ViewContext.LocalEntityRef;
        var player = PredictedFrame.Get<PlayableMechanic>(localEntity);

        DefeatBG.SetActive(player.Team != team);
    }
}
