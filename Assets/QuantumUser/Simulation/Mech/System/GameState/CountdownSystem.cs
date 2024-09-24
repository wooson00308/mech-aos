using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Photon.Deterministic;
using Quantum.Mech;
using UnityEngine;

namespace Quantum
{
    public unsafe class CountdownSystem : SystemSignalsOnly, IGameState_Countdown, ISignalGameStateChanged
    {
        public override bool StartEnabled => false;

        public void GameStateChanged(Frame f, GameState state)
        {
            if (state == GameState.Outro)
            {
                var nexusBlockIterator = f.Unsafe.GetComponentBlockIterator<Nexus>();
                FP maxCurrentHealth = 0;
                Team team = Team.Blue;
                foreach (var entityComponentPointerPair in nexusBlockIterator)
                {
                    if (entityComponentPointerPair.Component->IsDestroy) continue;
                    var nexus = f.Get<Nexus>(entityComponentPointerPair.Entity);
                    
                    if (nexus.CurrentHealth > maxCurrentHealth)
                    {
                        maxCurrentHealth = nexus.CurrentHealth;
                        team = nexus.Team;
                    }
                }

                f.Events.OnPlayerTeamWin(team);
            }
        }

        public override void OnEnabled(Frame f)
        {
            GameStateSystem.SetStateDelayed(f, GameState.Game, 3);
        }
    }
}