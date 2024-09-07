
namespace Quantum
{
    interface IGameState { }
    interface IGameState_Lobby : IGameState { }
    interface IGameState_Pregame : IGameState { }
    interface IGameState_Intro : IGameState { }
    interface IGameState_Countdown : IGameState { }
    interface IGameState_Game : IGameState { }
    interface IGameState_Outro : IGameState { }
    interface IGameState_Postgame : IGameState { }
}

