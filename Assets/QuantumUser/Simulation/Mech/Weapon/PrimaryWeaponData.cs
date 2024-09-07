using System.Collections.Generic;
using UnityEngine.Serialization;

namespace Quantum
{
    using Photon.Deterministic;
#if QUANTUM_UNITY
    using UnityEngine;
#endif

    [System.Serializable]
    public class PrimaryWeaponData : WeaponData
    {
        public List<int> Skills;
    }
}