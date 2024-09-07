using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Scripting;

namespace Quantum
{
    [Preserve]
    public unsafe class UserSystem : SystemSignalsOnly, ISignalOnPlayerConnected, ISignalOnPlayerDisconnected
    {
        public override void OnInit(Frame f)
        {
            f.Global->teamData = f.AllocateDictionary(f.Global->teamData);
        }

        public void OnPlayerConnected(Frame f, PlayerRef player)
        {
            f.UserJoined();
        }

        public void OnPlayerDisconnected(Frame f, PlayerRef player)
        {
            // destroy any entities whose PlayerLink belongs to the disconnecting player
            ComponentFilter<PlayerLink> filter = f.Filter<PlayerLink>();
            while (filter.Next(out EntityRef e, out PlayerLink link))
            {
                if (link.PlayerRef == player)
                {
                    f.Destroy(e);
                }
            }

            f.Global->teamData.Resolve(f, out var playerData);
            playerData.Remove(player);

            f.Events.PlayerLeft(player);

            f.UserLeft();
        }
    }
}