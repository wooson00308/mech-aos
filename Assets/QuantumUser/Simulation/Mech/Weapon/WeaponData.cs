namespace Quantum
{
    using Photon.Deterministic;
#if QUANTUM_UNITY
    using UnityEngine;
#endif

    [System.Serializable]
    public class WeaponData : AssetObject
    {
#if QUANTUM_UNITY
        [Header("View Configuration")] 
        // public Blueless.AudioConfiguration ShootAudioInfo;
        public Sprite UIIcon;
        public GameObject Prefab;
#endif
    
        public FP FireRate;
        public FP ShootForce;
        public int MaxAmmo;
        public FP RechargeTimer;
        public FP TimeToRecharge;
        public FPVector3 FireSpotOffset;
        public FPVector3 PositionOffset;

        public AssetRef<BulletData> BulletData; 
    }
}