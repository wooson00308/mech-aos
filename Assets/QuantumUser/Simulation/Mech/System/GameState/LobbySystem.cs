using Quantum.Mech;
using UnityEngine;

namespace Quantum
{
    public unsafe class LobbySystem : SystemMainThread, IGameState_Lobby
    {
        public override void OnEnabled(Frame f)
        {
            var config = f.FindAsset(f.RuntimeConfig.MechGameConfig);
            if (f.SessionConfig.PlayerCount > 1)
                f.Global->clock = config.lobbyingDuration;
            else
                GameStateSystem.SetState(f, GameState.Pregame);
        
        
        }
        
        public override void Update(Frame f)
        {
     
            // if (f.ActiveUsers >= f.PlayerCount)
            // {
                if (f.Global->clock > 0)
                {
                    f.Global->clock -= f.DeltaTime;
                    if (f.Global->clock <= 0)
                    {
                        f.Global->clock = 0;
                        f.Global->ClientConnectUsers = 0;
                        GameStateSystem.SetState(f, GameState.Pregame);
                    }
                }
            
            // }

        }

        // public override void OnEnabled(Frame f)
        // {
        //     if (f.SessionConfig.PlayerCount > 1)
        //         f.Global->clock = 20;
        //     else
        //         GameStateSystem.SetState(f, GameState.Pregame);
        // }
        //
        // public override void Update(Frame f)
        // {
        //     if (f.Global->clock > 0)
        //     {
        //         f.Global->clock -= f.DeltaTime;
        //         if (f.Global->clock <= 0)
        //         {
        //             f.Global->clock = 0;
        //             GameStateSystem.SetState(f, GameState.Pregame);
        //         }
        //     }
        // }


        
    }
}