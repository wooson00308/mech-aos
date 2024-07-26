using UnityEngine.Scripting;

namespace Quantum.Mech
{
    using Photon.Deterministic;

    [Preserve]
    public unsafe class BoundarySystem : SystemMainThreadFilter<BoundarySystem.Filter>
    {
        public struct Filter
        {
            public EntityRef Entity;
            public Transform3D* Transform;
        }

        public override void Update(Frame f, ref Filter filter)
        {
            MechGameConfig config = f.FindAsset(f.RuntimeConfig.MechGameConfig);

            // if (IsOutOfBounds(filter.Transform->Position, config.MapExtends, out FPVector3 newPosition))
            // {
            //     filter.Transform->Position = newPosition;
            //     filter.Transform->Teleport(f, newPosition);
            // }
        }

        /// <summary>
        /// Test if a position is out of bounds and provide a warped position.
        /// When the entity leaves the bounds it will emerge on the other side.
        /// </summary>
        public bool IsOutOfBounds(FPVector3 position, FPVector3 mapExtends, out FPVector3 newPosition)
        {
            newPosition = position;

            if (position.X >= -mapExtends.X && position.X <= mapExtends.X &&
                position.Z >= -mapExtends.Z && position.Z <= mapExtends.Z)
            {
                // position is inside map bounds
                return false;
            }

            // warp x position
            if (position.X < -mapExtends.X)
            {
                newPosition.X = mapExtends.X;
            }
            else if (position.X > mapExtends.X)
            {
                newPosition.X = -mapExtends.X;
            }

            // warp y position
            if (position.Z < -mapExtends.Z)
            {
                newPosition.Z = mapExtends.Z;
            }
            else if (position.Z > mapExtends.Z)
            {
                newPosition.Z = -mapExtends.Z;
            }

            return true;
        }
    }
}