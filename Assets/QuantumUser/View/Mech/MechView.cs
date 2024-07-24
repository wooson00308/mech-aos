using System.Collections;
using System.Collections.Generic;
using Quantum;
using Quantum.Mech;
using UnityEngine;

public class MechView : QuantumEntityViewComponent<CustomViewContext>
{
    public Transform Body;
    public Animator CharacterAnimator;

    public override void OnActivate(Frame frame)
    {
        var playerLink = VerifiedFrame.Get<PlayerLink>(EntityRef);
        if (Game.PlayerIsLocal(playerLink.PlayerRef))
        {
            ViewContext.LocalPlayerView = this;
        }
    }
}
