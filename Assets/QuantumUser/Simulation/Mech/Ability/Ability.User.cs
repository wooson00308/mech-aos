using System;
using Photon.Deterministic;

namespace Quantum
{
    public unsafe partial struct Ability
    {
        public struct AbilityState
        {
            public bool IsDelayed;
            public bool IsActive;
            public bool IsActiveStartTick;
            public bool IsActiveEndTick;
            public bool IsOnCooldown;
        }

        public bool HasBufferedInput => InputBufferTimer.IsRunning;
        public bool IsDelayed => DelayTimer.IsRunning;
        public bool IsActive => DurationTimer.IsRunning;
        public bool IsDelayedOrActive => IsDelayed || IsActive;


        public bool TryActivateAbility(Frame frame, EntityRef entityRef, PlayerRef playerRef)
        {

            Status* playerStatus = frame.Unsafe.GetPointer<Status>(entityRef);

            if (playerStatus->IsDead)
            {
                return false;
            }

            AbilityInventory* abilityInventory = frame.Unsafe.GetPointer<AbilityInventory>(entityRef);

            if (abilityInventory->HasActiveAbility)
            {
                return false;
            }

            Transform3D* transform = frame.Unsafe.GetPointer<Transform3D>(entityRef);
            CharacterController3D* kcc = frame.Unsafe.GetPointer<CharacterController3D>(entityRef);
            AbilityData abilityData = frame.FindAsset<AbilityData>(AbilityData.Id);
            PlayerMovementData playerMovementData = frame.FindAsset<PlayerMovementData>(playerStatus->PlayerMovementData.Id);
            
            InputBufferTimer.Reset();
            DelayTimer.Start(abilityData.Delay);

            abilityInventory->ActiveAbilityInfo.ActiveAbilityIndex = (int)AbilityType;
            abilityInventory->ActiveAbilityInfo.CastDirection = GetCastDirection(frame, playerRef, abilityData, transform);
            abilityInventory->ActiveAbilityInfo.CastRotation = FPQuaternion.LookRotation(abilityInventory->ActiveAbilityInfo.CastDirection);
            abilityInventory->ActiveAbilityInfo.CastVelocity = kcc->Velocity;
            
            playerMovementData.UpdateKCCSettings(frame, entityRef);

            return true;
        }

        private FPVector3 GetCastDirection(Frame frame, PlayerRef playerRef, AbilityData abilityData, Transform3D* transform)
        {
            // Input* input = frame.GetPlayerInput(playerRef);
            // FPVector3 direction = input->Movement.XOY;
            // return direction.Normalized;
            return transform->Forward;
        }

        public AbilityState Update(Frame frame, EntityRef entityRef)
        {
            AbilityState state = new AbilityState();

            InputBufferTimer.Tick(frame.DeltaTime);

            if (IsDelayedOrActive)
            {
                Status* playerStatus = frame.Unsafe.GetPointer<Status>(entityRef);

                if (playerStatus->IsDead)
                {
                    StopAbility(frame, entityRef);

                    return state;
                }
                FP delayTimeLeft = DelayTimer.TimeLeft;

                if (IsDelayed)
                {
                    DelayTimer.Tick(frame.DeltaTime);

                    if (DelayTimer.IsRunning)
                    {
                        state.IsDelayed = true;
                    }
                    else
                    {
                        state.IsActiveStartTick = true;
                        AbilityData abilityData = frame.FindAsset<AbilityData>(AbilityData.Id);
                        DurationTimer.Start(abilityData.Duration);
                    }
                }

                if (IsActive)
                {
                    state.IsActive = true;

                    DurationTimer.Tick(frame.DeltaTime - delayTimeLeft);

                    if (DurationTimer.IsDone)
                    {
                        state.IsActiveEndTick = true;

                        StopAbility(frame, entityRef);
                    }
                }
            }

            return state;
        }

        public void BufferInput(Frame frame)
        {
            AbilityData abilityData = frame.FindAsset<AbilityData>(AbilityData.Id);

            InputBufferTimer.Start(abilityData.InputBuffer);
        }

        public void StopAbility(Frame frame, EntityRef entityRef)
        {
            Status* playerStatus = frame.Unsafe.GetPointer<Status>(entityRef);
            AbilityInventory* abilityInventory = frame.Unsafe.GetPointer<AbilityInventory>(entityRef);
            PlayerMovementData playerMovementData = frame.FindAsset<PlayerMovementData>(playerStatus->PlayerMovementData.Id);

            abilityInventory->ActiveAbilityInfo.ActiveAbilityIndex = -1;

            DelayTimer.Reset();
            DurationTimer.Reset();

            playerMovementData.UpdateKCCSettings(frame, entityRef);
        }
        
    }
}