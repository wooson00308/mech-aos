using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Photon.Deterministic;

namespace Quantum.Mech
{
    public unsafe class GameStateSystem : SystemMainThread
    {
		static readonly ReadOnlyDictionary<GameState, Type> stateTable =
			new(new Dictionary<GameState, Type>()
			{
				{ GameState.Lobby, typeof(IGameState_Lobby) },
				{ GameState.Pregame, typeof(IGameState_Pregame) },
				{ GameState.Intro, typeof(IGameState_Intro) },
				{ GameState.Countdown, typeof(IGameState_Countdown) },
				{ GameState.Game, typeof(IGameState_Game) },
				{ GameState.Outro, typeof(IGameState_Outro) },
				{ GameState.Postgame, typeof(IGameState_Postgame) }
			});

        
        public override void OnInit(Frame f)
        {
            f.Global->DelayedState = 0;
            f.Global->StateTimer = 0;
            f.Global->PreviousState = 0;
            SetState(f, GameState.Lobby);
        }
        
        
        public override void Update(Frame f)
        {
            if (f.Global->StateTimer > 0)
            {
                f.Global->StateTimer -= f.DeltaTime;
                if (f.Global->StateTimer <= 0)
                {
                    f.Global->StateTimer = 0;
                    SetState(f, f.Global->DelayedState);
                    f.Global->DelayedState = 0;
                }
            }

            if (f.Global->CurrentState != f.Global->PreviousState)
            {
                if (stateTable.TryGetValue(f.Global->CurrentState, out Type t))
                {

                    foreach (SystemBase sys in f.SystemsAll
                                 .Where(s => 
                                     s.GetType().GetInterfaces().Contains(typeof(IGameState)) 
                                     && !s.GetType().GetInterfaces().Contains(t)))
                    {
                        f.SystemDisable(sys.GetType());
                    }

                    foreach (SystemBase sys in f.SystemsAll
                                 .Where(
                                     s => 
                                         s.GetType().GetInterfaces().Contains(typeof(IGameState)) 
                                         && s.GetType().GetInterfaces().Contains(t)))
                    {
                        if (!f.SystemIsEnabledSelf(sys.GetType()))
                            f.SystemEnable(sys.GetType());
                    }
                }

                f.Events.GameStateChanged(f.Global->CurrentState, f.Global->PreviousState);
                f.Global->PreviousState = f.Global->CurrentState;
            }
        }

        
        public static void SetStateDelayed(Frame f, GameState state, FP delay)
        {
            f.Global->DelayedState = state;
            f.Global->StateTimer = delay;
        }

        public static void SetState(Frame f, GameState state)
        {
            f.Global->CurrentState = state;
        }
    }
}