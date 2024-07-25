using Photon.Deterministic;
using System.Collections;
using System.Collections.Generic;
using System.Security.Permissions;
using UnityEngine;
using UnityEngine.Scripting;

namespace Quantum.Mech.Projectile
{
    [Preserve]
    public unsafe class ProjectileSystem : SystemMainThreadFilter<MechSystem.Filter>
    {
        public override void Update(Frame f, ref MechSystem.Filter filter)
        {
            //filter.Transform->Position += new FPVector3(0, 0, 1);
        }
        
        public struct Filter
        {
            public EntityRef Entity;
            public MechProjectile* Projectile;
        }
    }
}
