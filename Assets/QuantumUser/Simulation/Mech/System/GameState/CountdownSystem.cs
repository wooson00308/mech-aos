using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quantum.Mech;

namespace Quantum
{
    public unsafe class CountdownSystem : SystemSignalsOnly, IGameState_Countdown
    {
        public override bool StartEnabled => false;

        public override void OnEnabled(Frame f)
        {
            GameStateSystem.SetStateDelayed(f, GameState.Game, 3);
        }
    }
}