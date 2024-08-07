using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quantum
{
    public unsafe class PostgameRequeueSystem : SystemMainThread, IGameState_Postgame
    {
        public override bool StartEnabled => false;

        public override void OnEnabled(Frame f)
        {
            var config = f.FindAsset(f.RuntimeConfig.MechGameConfig);
            f.Global->clock = config.postgameDuration;
        }

        public override void Update(Frame f)
        {
            if (f.Global->clock > 0)
            {
                f.Global->clock -= f.DeltaTime;
                if (f.Global->clock <= 0)
                {
                    f.Global->clock = 0;
                    f.Events.Shutdown();
                }
            }
        }
    }
}