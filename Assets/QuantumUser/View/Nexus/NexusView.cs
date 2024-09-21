using Quantum;
using System.Collections;
using UnityEngine;

namespace QuantumUser
{
    public class NexusView : QuantumEntityViewComponent
    {
        public Transform Model;
        public GameObject HitFx;

        public override void OnActivate(Frame frame)
        {
            QuantumEvent.Subscribe<EventOnNexusDestroy>(this, Destroy);
            QuantumEvent.Subscribe<EventOnNexusTakeDamage>(this, TakeDamage);
        }

        private void TakeDamage(EventOnNexusTakeDamage e)
        {
            if (e.Nexus.ToString() != gameObject.name) return;
            HitFx.SetActive(false);
            HitFx.SetActive(true);
        }
        
        private void Destroy(EventOnNexusDestroy nexusDestroy)
        {
            if (nexusDestroy.Nexus != EntityRef) return;
            Model.gameObject.SetActive(false);
        }
        
        private void OnDestroy()
        {
            QuantumEvent.UnsubscribeListener(this);
        }
        
    }
}