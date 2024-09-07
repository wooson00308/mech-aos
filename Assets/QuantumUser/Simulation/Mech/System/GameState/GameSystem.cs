using System;
using System.Collections.Generic;
using System.Linq;
using Photon.Deterministic;
using UnityEngine;
using UnityEngine.Scripting;

namespace Quantum.Mech
{
    [Preserve]
    public unsafe class GameSystem : SystemSignalsOnly, IGameState_Game, ISignalOnTeamDefeat
    {
        public override bool StartEnabled => false;
        private Dictionary<Team, bool> isTeamDefeated;
        public override void OnEnabled(Frame f)
        {
            isTeamDefeated = new Dictionary<Team, bool>();
            foreach (var pair in f.Unsafe.GetComponentBlockIterator<PlayableMechanic>())
            {
                isTeamDefeated[pair.Component->Team] = false;
                Debug.Log(pair.Component->Team);
            }
        }

        public void OnTeamDefeat(Frame f, Team Team)
        {
            foreach (var mechanic in f.Unsafe.GetComponentBlockIterator<PlayableMechanic>())
            {
                if (mechanic.Component->Team != Team) continue;
                var status = f.Unsafe.GetPointer<Status>(mechanic.Entity);
                status->RespawnTimer = FP._10000;
            }

            isTeamDefeated[Team] = true;

            Debug.Log($"OnTeamDefeat : {IsOnlyOneTeamNotDefeated()}");
            if (IsOnlyOneTeamNotDefeated())
            {
                Debug.Log("게임 끝났어요!!!! 호호호!!");
                //f.Events.Shutdown();
                // 게임 종료
                GameStateSystem.SetState(f, GameState.Outro);
            }    
        }
        
        public bool IsOnlyOneTeamNotDefeated()
        {
            var falseCount = 0;

            foreach (var teamStatus in isTeamDefeated.Values.Where(teamStatus => !teamStatus))
            {
                falseCount++;
                if (falseCount > 1)
                {
                    return false;
                }
            }
            
            Debug.Log($"falseCount : {falseCount}");
            return falseCount == 1;
        }
    }
}