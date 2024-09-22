using System.Collections;
using System.Collections.Generic;
using Quantum;
using Quantum.Mech;
using UnityEngine;
using UnityEngine.Scripting;

[Preserve]
public unsafe class MechView : QuantumEntityViewComponent<CustomViewContext>
{
    public Nexus* Nexus;
    public Transform Body;
    public Animator CharacterAnimator;
    public List<GameObject> HitFxs;
    
    public override void OnActivate(Frame frame)
    {
        var playerLink = VerifiedFrame.Get<PlayerLink>(EntityRef);
        if (Game.PlayerIsLocal(playerLink.PlayerRef))
        {
            ViewContext.LocalPlayerView = this;
            ViewContext.EntityRef = EntityRef;
        }
        QuantumEvent.Subscribe<EventOnMechanicDeath>(this, Death);
        QuantumEvent.Subscribe<EventOnMechanicRespawn>(this, Respawn);
        QuantumEvent.Subscribe<EventOnMechanicTakeDamage>(this, Hit);

    }
    private void Hit(EventOnMechanicTakeDamage e)
    {
        if (e.Mechanic.ToString() != gameObject.name) return;
        int randomIndex = Random.Range(0, HitFxs.Count);
        HitFxs[randomIndex].SetActive(false);
        HitFxs[randomIndex].SetActive(true);
    }

    private void Respawn(EventOnMechanicRespawn mechanicRespawn)
    {
        if (mechanicRespawn.Mechanic != EntityRef) return;
        Body.gameObject.SetActive(true);
    }
    private void Death(EventOnMechanicDeath mechanicDeath)
    {
        if (mechanicDeath.Mechanic != EntityRef) return;
        Body.gameObject.SetActive(false);
        var playableMechanic = VerifiedFrame.Get<PlayableMechanic>(EntityRef);

        if (Nexus == null)
        {
            foreach (var nexus in VerifiedFrame.Unsafe.GetComponentBlockIterator<Nexus>())
            {
                if (nexus.Component->Team != playableMechanic.Team) continue;
                Nexus = nexus.Component;
                return;
            }
        }

    }
    

    
    private void OnDestroy()
    {
        QuantumEvent.UnsubscribeListener(this);
    }
    
}
