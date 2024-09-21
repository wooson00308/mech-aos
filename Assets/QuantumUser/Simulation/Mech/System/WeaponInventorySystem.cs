using Photon.Deterministic;
using UnityEngine.Scripting;

namespace Quantum.Mech
{
    [Preserve]
    public unsafe class WeaponInventorySystem : SystemMainThreadFilter<WeaponInventorySystem.Filter>, ISignalOnGameEnded
    {
        public struct Filter
        {
            public EntityRef Entity;
            public PlayerLink* PlayerLink;
            public Status* Status;
            public WeaponInventory* WeaponInventory;
        }

        public override void Update(Frame frame, ref Filter filter)
        {
            var robot = filter.Entity;
            var playerLink = filter.PlayerLink;
            var status = filter.Status;
            var weaponInventory = filter.WeaponInventory;

            if (status->IsDead)
            {
                return;
            }

            var input = frame.GetPlayerInput(playerLink->PlayerRef);
            if (input->ChangeWeapon.WasPressed)
            {
                ChangeWeapon(frame, robot, weaponInventory);
            }
        }

        private void ChangeWeapon(Frame frame, EntityRef robot, WeaponInventory* weaponInventory)
        {
            var weapons = frame.ResolveList(weaponInventory->Weapons);
            weaponInventory->CurrentWeaponIndex = (weaponInventory->CurrentWeaponIndex + 1) % weapons.Count;
            Weapon* currentWeapon = weapons.GetPointer(weaponInventory->CurrentWeaponIndex);
            currentWeapon->ChargeTime = FP._0;

            frame.Events.OnChangeWeapon(robot);
        }

        
        public unsafe void OnGameEnded(Frame frame, GameController* gameController)
        {
            frame.SystemDisable<WeaponInventorySystem>();
        }
    }
}