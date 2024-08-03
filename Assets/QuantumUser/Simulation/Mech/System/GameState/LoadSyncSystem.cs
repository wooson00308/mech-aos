using System;
using Quantum.Mech;

namespace Quantum
{
    public unsafe class LoadSyncSystem : SystemSignalsOnly, ISignalOnPlayerAdded, IGameState_Pregame
    {
        public override bool StartEnabled => false;

        public void OnPlayerAdded(Frame frame, PlayerRef player, bool firstTime)
        {
            frame.Global->teamData.Resolve(frame, out var dict);

            if (!dict.TryGetValuePointer(player, out var pd))
            {
                dict.Add(player, new PlayerData());
                dict.TryGetValuePointer(player, out pd);
            }

            Log.Debug($"Player {player} is ready");
            pd->ready = true;
            pd->Team = (Team)(frame.Global->TeamIndex % 3);
            frame.Global->TeamIndex++;


            if (CheckAllReady(frame, dict))
            {
                Log.Debug("Ready");
                GameStateSystem.SetState(frame, GameState.Countdown);
                // GameStateSystem.SetState(frame, frame.SessionConfig.PlayerCount > 1 ? GameState.Intro : GameState.Countdown);
            }
        }

        private static bool CheckAllReady(Frame f, Collections.QDictionary<int, PlayerData> dict)
        {
            int redCount = 0, blueCount = 0, greenCount = 0;
            for (int i = 0; i < f.ActiveUsers; i++)
            {
                if (dict.TryGetValuePointer(i, out PlayerData* pd))
                {
                    if (pd->ready == false)
                    {
                        Log.Debug($"player {i} is not ready");
                        return false;
                    }
                    
                    switch (pd->Team)
                    {
                        case Team.Red:
                            redCount++;
                            break;
                        case Team.Blue:
                            blueCount++;
                            break;
                        case Team.Green:
                            greenCount++;
                            break;
                    }
                    
                }
                else
                {
                    Log.Debug($"player {i} has no playerdata");
                    return false;
                }
            }
            
            Log.Debug($"player Team All Ready : {(redCount == 1 && blueCount == 1 && greenCount == 1)}");
            return (redCount == 1 && blueCount == 1 && greenCount == 1);
        }
    }
}