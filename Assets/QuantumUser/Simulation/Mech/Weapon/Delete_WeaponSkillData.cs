namespace Quantum
{
    using Photon.Deterministic;
#if QUANTUM_UNITY
    using UnityEngine;
#endif

    [System.Serializable]
    public class Delete_WeaponSkillData : AssetObject
    {
#if QUANTUM_UNITY
        [Header("View Configuration")] 
        // public Blueless.AudioConfiguration ShootAudioInfo;
        public Sprite UIIcon;
        public GameObject Prefab;
#endif
    
        // 쿨타임
        public FP CastingTime;
        public FP CoolTime;
        
        public AssetRef<SkillData> Data; 
    }
}