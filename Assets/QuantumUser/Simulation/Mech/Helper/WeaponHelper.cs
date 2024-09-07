namespace Quantum.Mech
{
    using Photon.Deterministic;
    public class WeaponHelper
    {
        public static FPVector3 GetFireSpotWorldOffset(WeaponData weaponData, Transform3D root, FPVector3 direction)
        {
            FPVector3 positionOffset = weaponData.PositionOffset;
            FPVector3 firespotVector = weaponData.FireSpotOffset;
            
            // 로컬 방향과 오프셋에 부모의 회전을 적용
            FPVector3 rotatedPositionOffset = root.Rotation * positionOffset;
            FPVector3 rotatedFirespotVector = root.Rotation * firespotVector;
            FPVector3 rotatedDirection = root.Rotation * direction;

            // 부모의 위치에 회전된 오프셋을 더함
            return rotatedPositionOffset + rotatedDirection + rotatedFirespotVector;
            
            
            // return positionOffset + direction + firespotVector;
        }
    }
}