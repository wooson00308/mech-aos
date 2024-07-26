namespace Quantum.Mech
{
    using Photon.Deterministic;
    public class WeaponHelper
    {
        public static FPVector3 GetFireSpotWorldOffset(WeaponData weaponData, FPVector3 direction)
        {
            FPVector3 positionOffset = weaponData.PositionOffset;
            FPVector3 firespotVector = weaponData.FireSpotOffset;
            return positionOffset + direction + firespotVector;
        }
    }
}