using Photon.Deterministic;
using Quantum.Mech;
using UnityEngine.Scripting;

namespace Quantum.Mech
{
    [Preserve]
    public unsafe class MechWaveSpawnerSystem : SystemSignalsOnly
    {
        public override void OnInit(Frame f)
        {
            SpawnMechWave(f);
        }
        public void SpawnMech(Frame f, AssetRef<EntityPrototype> childPrototype)
        {
            // FindAsset 매우 효율적
            MechGameConfig config = f.FindAsset(f.RuntimeConfig.MechGameConfig);
            var mech = f.Create(childPrototype);
            var mechTransform = f.Unsafe.GetPointer<Transform3D>(mech);
            mechTransform->Position = GetRandomEdgePointOnCircle(f, 10); 
            
        }
        

        public static FPVector3 GetRandomEdgePointOnCircle(Frame f, FP radius)
        {
            var rotate = FPVector2.Rotate(FPVector2.Up * radius, f.RNG->Next() * FP.PiTimes2);
            
            return new FPVector3(rotate.X,1,rotate.Y) ;
        }
        private void SpawnMechWave(Frame f)
        {
            MechGameConfig config = f.FindAsset(f.RuntimeConfig.MechGameConfig);
            for (int i = 0; i < f.Global->AsteroidsWaveCount + 2; i++)
            {
                SpawnMech(f, config.MechPrototype);
            }
            
            f.Global->AsteroidsWaveCount++;
        }
    }
}