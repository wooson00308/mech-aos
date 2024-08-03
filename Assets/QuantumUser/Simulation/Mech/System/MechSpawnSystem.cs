using System;
using UnityEngine;
using UnityEngine.Scripting;

namespace Quantum.Mech
{
    
    [Preserve]
    public unsafe class MechSpawnSystem : SystemSignalsOnly, ISignalOnPlayerAdded
    {
        // 여기서 플레이어 데이터를 뽑아낼수있음
        // https://doc.photonengine.com/ko-kr/quantum/current/tutorials/asteroids/5-player-spawning
        public void OnPlayerAdded(Frame frame, PlayerRef player, bool firstTime)
        {
            {
                var data = frame.GetPlayerData(player);
                var playerAvatarAssetRef = data.PlayerAvatar.IsValid ? data.PlayerAvatar : frame.RuntimeConfig.DefaultPlayerAvatar;
                var prototypeAsset = frame.FindAsset(playerAvatarAssetRef);
                SetPlayerCharacter(frame, player, prototypeAsset);
            }
        }
        
        private void SetPlayerCharacter(Frame frame, PlayerRef player, AssetRef<EntityPrototype> prototypeAsset)
        {
            EntityRef character = frame.Create(prototypeAsset);

            PlayerLink* playerLink = frame.Unsafe.GetPointer<PlayerLink>(character);
            playerLink->PlayerRef = player;

            PlayableMechanic* playableMechanic = frame.Unsafe.GetPointer<PlayableMechanic>(character);
            playableMechanic->Team = (Team)(frame.Global->TeamIndex % 3);
            frame.Global->TeamIndex++;
            
            RespawnHelper.RespawnMechanic(frame, character);
            // frame.Add(character, new PlayerLink { PlayerRef = player });
            frame.Events.OnMechanicCreated(character);
            frame.Signals.SpawnMechanic(character);
        }

    }
}
