using System.Collections.Generic;
using Photon.Client;
using Photon.Realtime;
using Quantum;
using UnityEngine;

namespace QuantumUser
{
    public class Matchmaker : QuantumCallbacks, IConnectionCallbacks, IMatchmakingCallbacks, IInRoomCallbacks, IOnEventCallback
    {
        public static event System.Action OnQuantumGameStart;
        public static event System.Action OnRealtimeJoinedRoom;
        public static event System.Action<Player> OnRealtimePlayerJoined;
        public static event System.Action<Player> OnRealtimePlayerLeft;

        public static Matchmaker Instance { get; private set; }
        public static RealtimeClient Client { get; private set; }
        public static AppSettings AppSettings { get; private set; }
        
        
        static System.Action<ConnectionStatus> onStatusUpdated = null;
        
        [SerializeField] RuntimeConfig runtimeConfig;
        
        [SerializeField] RuntimePlayer runtimePlayer;
        
        [SerializeField] byte maxPlayers = 3;
        
        
        #region Type Definitions
        public struct ConnectionStatus
        {
            public string Message { get; }
            public State State { get; }

            public ConnectionStatus(string msg, State state)
            {
                Message = msg;
                State = state;
            }
        }

        public enum State
        {
            Undefined = 0,
            ConnectingToServer,
            ConnectingToRoom,
            JoinedRoom,
            GameStarted,
            Failed = -1
        }
        #endregion

        #region Unity

        private void Awake()
        {
            if (Instance)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);

            SceneLoader.OnSceneLoadDone += SendData;

            AppSettings = new AppSettings(PhotonServerSettings.Global.AppSettings);
            Client = new RealtimeClient();
            Client.AddCallbackTarget(Instance);
        }
        
        private void Update()
        {
            Client?.Service();
        }
        

        #endregion
        
        #region Public Methods
        public static void Connect(System.Action<ConnectionStatus> statusUpdatedCallback)
        {
            if (Client.IsConnected) return;

            onStatusUpdated = statusUpdatedCallback;

            if (Client.ConnectUsingSettings(AppSettings))
            {
                onStatusUpdated?.Invoke(new ConnectionStatus("Establishing Connection", State.ConnectingToServer));
            }
            else
            {
                onStatusUpdated?.Invoke(new ConnectionStatus("Unable to Connect", State.Failed));
                onStatusUpdated = null;
                LogWarning("Connect failed");
            }
        }
        
        public static void SendStartGameEvent()
        {
            Client.OpRaiseEvent(0, null, new RaiseEventArgs() { Receivers = ReceiverGroup.All }, SendOptions.SendReliable);
        }
        
        
        public static void Disconnect()
        {
            QuantumRunner.ShutdownAll();
            Log("Shutdown");
            Client.Disconnect();
        }
        
        #endregion
        
        #region Private Methods
        static void StartQuantumGame()
        {
            SessionRunner.Arguments arguments = new SessionRunner.Arguments()
            {
                RuntimeConfig = Instance.runtimeConfig,
                GameMode = Photon.Deterministic.DeterministicGameMode.Multiplayer,
                PlayerCount = Client.CurrentRoom.MaxPlayers,
                ClientId = Client.LocalPlayer.UserId,
                Communicator = new QuantumNetworkCommunicator(Client),
                SessionConfig = QuantumDeterministicSessionConfigAsset.DefaultConfig,
            };
		
            QuantumRunner.StartGame(arguments);
        }


        void SendData()
        {
            runtimePlayer.PlayerNickname = LocalData.nickname;
            QuantumRunner.Default.Game.AddPlayer(runtimePlayer);
        }

        static void Log(string msg)
        {
            Debug.Log($"<color=#03b1fc>{msg}</color>");
        }

        static void LogWarning(string msg)
        {
            Debug.Log($"<color=#fcca03>{msg}</color>");
        }

        #endregion
        
        
        public void OnConnected()
        {
            onStatusUpdated?.Invoke(new ConnectionStatus("Connecting To Server", State.ConnectingToServer));
            Log("Connected");
        }

        public void OnConnectedToMaster()
        {
            Log("OnConnectedToMaster");

            JoinRandomRoomArgs joinRandomParams = new JoinRandomRoomArgs();
            EnterRoomArgs enterRoomParams = new EnterRoomArgs()
            {
                RoomOptions = new RoomOptions()
                {
                    IsVisible = true,
                    MaxPlayers = maxPlayers,
                    Plugins = new string[] { "QuantumPlugin" },
                    PlayerTtl = PhotonServerSettings.Global.PlayerTtlInSeconds * 1000,
                    EmptyRoomTtl = PhotonServerSettings.Global.EmptyRoomTtlInSeconds * 1000
                }
            };

            if (Client.OpJoinRandomOrCreateRoom(joinRandomParams, enterRoomParams))
            {
                onStatusUpdated?.Invoke(new ConnectionStatus("Connecting To Room", State.ConnectingToRoom));
                Log("Joining a room");
            }
            else
            {
                LogWarning("Failed to join a room");
                Client.Disconnect();
            }
        }

        public void OnDisconnected(DisconnectCause cause)
        {
            LogWarning($"Disconnected: {cause}");
            QuantumRunner.ShutdownAll();
		
            // InterfaceManager.Instance.elevatorObj.SetActive(false);
            // AudioManager.LerpVolume(AudioManager.Instance.crowdSource, 0f, 0.5f);
            // AudioManager.SetSnapshot("Default", 0.5f);
            // if (CameraController.Instance) CameraController.Instance.Effects.Unblur(0);
            // UnityEngine.SceneManagement.SceneManager.LoadScene(menuScene);

            // if (isRequeueing) return;

            // UIScreen.activeScreen.BackTo(InterfaceManager.Instance.mainMenuScreen);
            // UIScreen.Focus(InterfaceManager.Instance.playmodeScreen);
        }

        #region IConnectionCallbacks
        public void OnRegionListReceived(RegionHandler regionHandler)
        {
        }

        public void OnCustomAuthenticationResponse(Dictionary<string, object> data)
        {
        }

        public void OnCustomAuthenticationFailed(string debugMessage)
        {
        }
        #endregion
        
        #region IMatchmakingCallbacks
        public void OnFriendListUpdate(List<FriendInfo> friendList) { }
        public void OnCreatedRoom() { }

        public void OnCreateRoomFailed(short returnCode, string message) { }

        public void OnLeftRoom() { }
        #endregion
        public void OnJoinedRoom()
        {
            onStatusUpdated?.Invoke(new ConnectionStatus("Joined Room", State.JoinedRoom));
            Log("Joined room");
            OnRealtimeJoinedRoom?.Invoke();
            StartQuantumGame();
        }

        public void OnJoinRoomFailed(short returnCode, string message)
        {
            onStatusUpdated?.Invoke(new ConnectionStatus($"Failed Joining Room: {message}", State.Failed));
            LogWarning("OnJoinRoomFailed");
        }

        public void OnJoinRandomFailed(short returnCode, string message)
        {
            onStatusUpdated?.Invoke(new ConnectionStatus("Failed Joining Random Room", State.Failed));
            LogWarning("OnJoinRandomFailed");
        }


        public void OnPlayerEnteredRoom(Player newPlayer)
        {
            Log($"Player {newPlayer} entered the room");
            OnRealtimePlayerJoined?.Invoke(newPlayer);
        }

        public void OnPlayerLeftRoom(Player otherPlayer)
        {
            Log($"Player {otherPlayer} left the room");
            OnRealtimePlayerLeft?.Invoke(otherPlayer);
        }


        public void OnPlayerPropertiesUpdate(Player targetPlayer, PhotonHashtable changedProps)
        {
            Log($"Properties updated for player: {targetPlayer}, {changedProps.ToStringFull()}");

        }

        public void OnMasterClientSwitched(Player newMasterClient) { }
        public void OnRoomPropertiesUpdate(PhotonHashtable propertiesThatChanged) { }

        public void OnEvent(EventData photonEvent)
        {
            if (photonEvent.Code == 0)
            {
                StartQuantumGame();
            }
        }
        
        
        #region Quantum Callbacks
        public override void OnGameStart(QuantumGame game, bool isResync)
        {
            if (game.Session.IsPaused)
            {
                LogWarning("GameStart but session is paused");
                return;
            }

            Log("Quantum Game Started");
            onStatusUpdated?.Invoke(new ConnectionStatus("Game Started", State.GameStarted));
            onStatusUpdated = null;
            OnQuantumGameStart?.Invoke();
        }
        #endregion
    }
}