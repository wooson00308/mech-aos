using System.Collections.Generic;
using Photon.Deterministic;

namespace Quantum.Mech
{
    public static unsafe class MechUtils
    {
        public static FPVector3 GetRandomSpots(Frame f, List<FPVector3> spots)
        {
            if(spots == null || spots.Count == 0)
                return FPVector3.Zero;
            
            return spots[f.RNG->Next(0, spots.Count - 1)];
        }
    }
}
