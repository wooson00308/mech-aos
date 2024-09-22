namespace Quantum
{
    using Photon.Deterministic;
    using UnityEngine;
    public unsafe class BulletSkillData : SkillData
    {
        public AssetRef<WeaponData> WeaponData;
        
                
        public override void Action(Frame frame, EntityRef mechanic)
        {
            var transform3D = frame.Unsafe.GetPointer<Transform3D>(mechanic);
            var weaponData = frame.FindAsset<WeaponData>(WeaponData.Id);
            frame.Events.OnWeaponShoot(mechanic);
            var bulletData = frame.FindAsset<BulletData>(weaponData.BulletData.Id);
            bulletData.SpawnBullet(frame, weaponData, mechanic, transform3D->Forward);
            
        }
        
    }
}