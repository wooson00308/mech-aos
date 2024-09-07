using Photon.Deterministic;
using UnityEngine;
using UnityEngine.Scripting;

namespace Quantum.Mech
{
    [Preserve]
    public unsafe class WeaponSystem : SystemMainThreadFilter<WeaponSystem.Filter>, ISignalOnMechanicRespawn, ISignalOnGameEnded
    {
        public struct Filter
        {
            public EntityRef Entity;
            public PlayerLink* PlayerLink;
            public Status* Status;
            public WeaponInventory* WeaponInventory;
        }
        
        void ISignalOnGameEnded.OnGameEnded(Frame frame, GameController* gameController)
        {
            frame.SystemDisable<WeaponSystem>();
        }
        
        void ISignalOnMechanicRespawn.OnMechanicRespawn(Frame frame, EntityRef robot)
        {
            // WeaponInventory* weaponInventory = frame.Unsafe.GetPointer<WeaponInventory>(robot);
            //
            // for (var i = 0; i < weaponInventory->Weapons.Length; i++)
            // {
            //     Weapon* weapon = weaponInventory->Weapons.GetPointer(i);
            //     var weaponData = frame.FindAsset<WeaponData>(weapon->WeaponData.Id);
            //
            //     weapon->IsRecharging = false;
            //     weapon->CurrentAmmo = weaponData.MaxAmmo;
            //     weapon->FireRateTimer = FP._0;
            //     weapon->DelayToStartRechargeTimer = FP._0;
            //     weapon->RechargeRate = FP._0;
            // }
        }
        
        public override void Update(Frame frame, ref Filter filter)
        {
            var mechanic = filter.Entity;
            var playerLink = filter.PlayerLink;
            var status = filter.Status;
            var weaponInventory = filter.WeaponInventory;

            if (status->IsDead)
            {
                return;
            }

            var weapons = frame.ResolveList(weaponInventory->Weapons);

            Weapon* currentWeapon = weapons.GetPointer(weaponInventory->CurrentWeaponIndex);
            currentWeapon->FireRateTimer -= frame.DeltaTime;
            currentWeapon->DelayToStartRechargeTimer -= frame.DeltaTime;
            currentWeapon->RechargeRate -= frame.DeltaTime;
            
            WeaponData weaponData = frame.FindAsset<WeaponData>(currentWeapon->WeaponData.Id);
            if (currentWeapon->DelayToStartRechargeTimer < 0 && currentWeapon->RechargeRate <= 0 &&
                currentWeapon->CurrentAmmo < weaponData.MaxAmmo)
            {
                IncreaseAmmo(currentWeapon, weaponData);
            }
            
            if (currentWeapon->FireRateTimer <= FP._0 && !currentWeapon->IsRecharging && currentWeapon->CurrentAmmo > 0)
            {
                Input* i = frame.GetPlayerInput(playerLink->PlayerRef);
            
                if (i->MainWeaponFire.IsDown)
                {
                    var transform3D = frame.Unsafe.GetPointer<Transform3D>(mechanic);
                    SpawnBullet(frame, mechanic, currentWeapon, transform3D->Forward);
                    currentWeapon->FireRateTimer = FP._1 / weaponData.FireRate;
                    currentWeapon->ChargeTime = FP._0;
                    frame.Events.WeaponFire(filter.Entity, weaponData);
                }
            }
        }
        
        private void IncreaseAmmo(Weapon* weapon, WeaponData data)
        {
            weapon->RechargeRate = data.RechargeTimer / (FP)data.MaxAmmo;
            Debug.Log($"weapon->RechargeRate : {weapon->RechargeRate}");
            weapon->CurrentAmmo++;

            if (weapon->CurrentAmmo == data.MaxAmmo)
            {
                weapon->IsRecharging = false;
            }
        }
        private void SpawnBullet(Frame frame, EntityRef mechanic, Weapon* weapon, FPVector3 direction)
        {
            weapon->CurrentAmmo -= 1;
            if (weapon->CurrentAmmo == 0)
            {
                weapon->IsRecharging = true;
                weapon->DelayToStartRechargeTimer = -1;
            }

            WeaponData weaponData = frame.FindAsset<WeaponData>(weapon->WeaponData.Id);
            weapon->DelayToStartRechargeTimer = weaponData.TimeToRecharge;

            frame.Events.OnWeaponShoot(mechanic);
            
            
            BulletData bulletData = frame.FindAsset<BulletData>(weaponData.BulletData.Id);

            EntityPrototype prototypeAsset = frame.FindAsset<EntityPrototype>(new AssetGuid(bulletData.BulletPrototype.Id.Value));
            EntityRef bullet = frame.Create(prototypeAsset);

            BulletFields* bulletFields = frame.Unsafe.GetPointer<BulletFields>(bullet);
            Transform3D* bulletTransform = frame.Unsafe.GetPointer<Transform3D>(bullet);

            bulletFields->BulletData = bulletData;
            Transform3D* transform = frame.Unsafe.GetPointer<Transform3D>(mechanic);

            var fireSpotWorldOffset = WeaponHelper.GetFireSpotWorldOffset(
                weaponData,
                *transform,
                direction
            );

            bulletTransform->Position = transform->Position + fireSpotWorldOffset;
            // Debug.Log($"Root Transform : {transform->Position}, Bullet Transform : {bulletTransform->Position}");
            bulletFields->Direction = direction * weaponData.ShootForce;
            bulletFields->Source = mechanic;
            bulletFields->Time = FP._0;
        }
    }
}