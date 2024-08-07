namespace Quantum.Mech
{
    
    public unsafe class PostgameCleanupSystem : SystemSignalsOnly, IGameState_Postgame
    {
        public override bool StartEnabled => false;
        public override void OnEnabled(Frame f)
        {
            {
                var filteredShips = f.Unsafe.FilterStruct<StatusSystem.Filter>();
                StatusSystem.Filter filter = default;
                while (filteredShips.Next(&filter))
                {
                    f.Destroy(filter.Entity);
                }
            }
        }
    }
}