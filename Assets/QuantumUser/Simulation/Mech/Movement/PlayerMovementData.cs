using Photon.Deterministic;

namespace Quantum
{
    public class PlayerMovementData : AssetObject
    {
        public FP NoMovementBraking = 5;
        public AssetRef<CharacterController3DConfig> DefaultKCCSettings;
        public AssetRef<CharacterController3DConfig> NoMovementKCCSettings;
        
        public unsafe void UpdateKCCSettings(Frame frame, EntityRef playerEntityRef)
        {
            Status* playerStatus = frame.Unsafe.GetPointer<Status>(playerEntityRef);
            AbilityInventory* abilityInventory = frame.Unsafe.GetPointer<AbilityInventory>(playerEntityRef);
            CharacterController3D* kcc = frame.Unsafe.GetPointer<CharacterController3D>(playerEntityRef);

            CharacterController3DConfig config;

            if (playerStatus->IsDead || abilityInventory->HasActiveAbility)
            {
                config = frame.FindAsset<CharacterController3DConfig>(NoMovementKCCSettings.Id);
            }
            else
            {
                config = frame.FindAsset<CharacterController3DConfig>(DefaultKCCSettings.Id);
            }

            kcc->SetConfig(frame, config);
        }
    }
}