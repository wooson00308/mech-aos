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
        var physicsCollider3D = VerifiedFrame.Get<PhysicsCollider3D>(EntityRef);
        if (Game.PlayerIsLocal(playerLink.PlayerRef))
        {
            ViewContext.LocalPlayerView = this;
        }

        QuantumEvent.Subscribe<EventOnMechanicDeath>(this, Death);
        QuantumEvent.Subscribe<EventOnMechanicRespawn>(this, Respawn);
    }
    
    private void Respawn(EventOnMechanicRespawn mechanicRespawn)
    {
        var playableMechanic = VerifiedFrame.Get<PlayableMechanic>(EntityRef);
        Debug.Log($"Team : {playableMechanic.Team}");
        
        if (mechanicRespawn.Mechanic != EntityRef) return;
        Body.gameObject.SetActive(true);
    }
    private void Death(EventOnMechanicDeath mechanicDeath)
    {
        if (mechanicDeath.Mechanic != EntityRef) return;
        Body.gameObject.SetActive(false);
    }
    
    private void OnDestroy()
    {
        QuantumEvent.UnsubscribeListener(this);
    }
    
}
