using Quantum;
using UnityEngine;

namespace QuantumUser
{
    public class NexusView : QuantumEntityViewComponent
    {
        public Transform Model;
        public override void OnActivate(Frame frame)
        {
            QuantumEvent.Subscribe<EventOnNexusDestroy>(this, Destroy);
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