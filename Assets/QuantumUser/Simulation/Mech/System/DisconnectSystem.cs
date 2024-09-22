using UnityEngine.Scripting;

namespace Quantum.Mech {
    using Photon.Deterministic;

    [Preserve]
    public unsafe class DisconnectSystem  : SystemMainThreadFilter<DisconnectSystem.Filter>
    {
        public struct Filter
        {
            public EntityRef Entity;
            public PlayerLink* PlayerLink;
            public Status* Status;
        }
        
        public override void Update(Frame frame, ref Filter filter) 
        {
            var playerID = filter.PlayerLink;
            var status = filter.Status;

            if (frame.IsPredicted) return;

            var flags = frame.GetPlayerInputFlags(playerID->PlayerRef);

            if ((flags & DeterministicInputFlags.PlayerNotPresent) == DeterministicInputFlags.PlayerNotPresent)
            {
                status->DisconnectedTicks++;
            }
            else
            {
                status->DisconnectedTicks = 0;
            }

            if (status->DisconnectedTicks >= 15)
            {
                status->IsDisconnect = true;
                frame.Destroy(filter.Entity);
            }
        }
    }
}
