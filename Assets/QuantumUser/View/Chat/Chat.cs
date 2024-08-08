using Photon.Chat;
using System;
using System.Collections.Generic;
using Photon.Client;
using QuantumUser;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class Chat : MonoBehaviour, IChatClientListener
{

    public ChatClient chatClient;
    
    private string ChannelToJoinOnConnect;
    [SerializeField] private int HistoryLengthToFetch;
    [SerializeField] private TMP_InputField InputFieldChat;
    [SerializeField] private RectTransform ChatPanel;
    [SerializeField] private TextMeshProUGUI CurrentChannelText;
    [SerializeField] private ScrollRect ChatScroll;
    
    private string _state = string.Empty;
    private string _selectedChannelName = string.Empty;
    private string _userName;
    private bool isConnected;
    #region Unity Event

    void Start()
    {
        var appIdPresent = string.IsNullOrEmpty(ChatSettings.Instance.AppId);

        _userName = string.Empty;
        _state = string.Empty;
        ChatPanel.gameObject.SetActive(false);
        isConnected = false;
        if (appIdPresent)
        {
            Debug.LogError("You need to set the chat app ID in the PhotonServerSettings file in order to continue.");
            return;
        }
        
    }
    
    public void Update()
    {
        if (this.chatClient != null)
        {
            this.chatClient.Service(); // make sure to call this regularly! it limits effort internally, so calling often is ok!
        }

        // check if we are missing context, which means we got kicked out to get back to the Photon Demo hub.
        // if (string.IsNullOrEmpty(_state))
        // {
        //     Destroy(this.gameObject);
        //     return;
        // }

        // this.StateText.gameObject.SetActive(ShowState); // this could be handled more elegantly, but for the demo it's ok.
    }
    
    
    public void OnDestroy()
    {
        isConnected = false;
        if (this.chatClient != null)
        {
            this.chatClient.Disconnect();
        }
    }

    public void OnApplicationQuit()
    {
        isConnected = false;
        if (this.chatClient != null)
        {
            this.chatClient.Disconnect();
        }
    }
    
    #endregion

    #region Public Methods
    public void Connect(string userName)
    {
        ChannelToJoinOnConnect = $"{Matchmaker.Client.CurrentRoom.Name}";
        this.chatClient = new ChatClient(this);
#if UNITY_WEBGL
        this.chatClient.UseBackgroundWorkerForSending = false;
#else
        this.chatClient.UseBackgroundWorkerForSending = true;
#endif
        this.chatClient.Connect(ChatSettings.Instance.AppId, "1.0", new AuthenticationValues(userName));
        _userName = userName;
    }

    public void ShowChannel(string channelName)
    {
        if (string.IsNullOrEmpty(channelName))
        {
            return;
        }

        ChatChannel channel = null;
        bool found = this.chatClient.TryGetChannel(channelName, out channel);
        if (!found)
        {
            Debug.Log("ShowChannel failed to find channel: " + channelName);
            return;
        }

        Debug.Log($"channelName : {channelName}");
        Debug.Log($"CurrentChannelText : {channel.ToStringMessages()}");

        _selectedChannelName = channelName;
        CurrentChannelText.text = channel.ToStringMessages();
        ScrollToBottomPosition();
    }

    public void OnEnterSend()
    {
        if (!isConnected) return;
        // if (Input.GetKey(KeyCode.Return) || Input.GetKey(KeyCode.KeypadEnter))
        // {
        //     SendChatMessage(this.InputFieldChat.text);
        //     this.InputFieldChat.text = "";
        // }
    }
    
    public void OnClickSend()
    {
        if (!isConnected) return;
        if (this.InputFieldChat != null)
        {
            SendChatMessage(this.InputFieldChat.text);
            this.InputFieldChat.text = "";
        }
    }
    
    public void ScrollToBottomPosition()
    {
        Canvas.ForceUpdateCanvases();
        ChatScroll.verticalNormalizedPosition = 0f;
    }
    public int TestLength = 2048;
    private byte[] testBytes = new byte[2048];

    private void SendChatMessage(string inputLine)
    {
        if (string.IsNullOrEmpty(inputLine))
        {
            return;
        }
        if ("test".Equals(inputLine))
        {
            if (this.TestLength != this.testBytes.Length)
            {
                this.testBytes = new byte[this.TestLength];
            }

            this.chatClient.SendPrivateMessage(this.chatClient.AuthValues.UserId, testBytes, true);
        }


        bool doingPrivateChat = this.chatClient.PrivateChannels.ContainsKey(_selectedChannelName);
        string privateChatTarget = string.Empty;
        if (doingPrivateChat)
        {
            // the channel name for a private conversation is (on the client!!) always composed of both user's IDs: "this:remote"
            // so the remote ID is simple to figure out
            string[] splitNames = _selectedChannelName.Split(new char[] { ':' });
            privateChatTarget = splitNames[1];
        }
        
        if (doingPrivateChat)
        {
            this.chatClient.SendPrivateMessage(privateChatTarget, inputLine);
        }
        else
        {
            this.chatClient.PublishMessage(_selectedChannelName, inputLine);
        }
        
    }
    
    #endregion
    
    
    public void DebugReturn(LogLevel level, string message)
    {
        switch (level)
        {
            case LogLevel.Error:
                UnityEngine.Debug.LogError(message);
                break;
            case LogLevel.Warning:
                UnityEngine.Debug.LogWarning(message);
                break;
            case LogLevel.Off:
            case LogLevel.Info:
            case LogLevel.Debug:
            default:
                UnityEngine.Debug.Log(message);
                break;
        }
    }

    public void OnDisconnected()
    {
        
    }

    public void OnConnected()
    {
        if (!string.IsNullOrEmpty(ChannelToJoinOnConnect))
        {
            this.chatClient.Subscribe(ChannelToJoinOnConnect, this.HistoryLengthToFetch);
        }
        
        ChatPanel.gameObject.SetActive(true);
        
        #region Friends

        // if (FriendsList!=null  && FriendsList.Length>0)
        // {
        //     this.chatClient.AddFriends(FriendsList); // Add some users to the server-list to get their status updates
        //
        //     // add to the UI as well
        //     foreach(string _friend in FriendsList)
        //     {
        //         if (this.FriendListUiItemtoInstantiate != null && _friend!= this.UserName)
        //         {
        //             this.InstantiateFriendButton(_friend);
        //         }
        //     }
        // }

        // if (this.FriendListUiItemtoInstantiate != null)
        // {
        //     this.FriendListUiItemtoInstantiate.SetActive(false);
        // }

        #endregion

        this.chatClient.SetOnlineStatus(ChatUserStatus.Online); // You can set your online state (without a mesage).
    }
    
    public void OnCustomAuthenticationResponse(Dictionary<string, object> data) { }

    public void OnCustomAuthenticationFailed(string debugMessage) {  }

    public void OnChatStateChange(ChatState state)
    {
        // use OnConnected() and OnDisconnected()
        // this method might become more useful in the future, when more complex states are being used.
        /*
            enum ChatState
            {
                Uninitialized,  /// 피어가 생성되었지만 아직 사용되지 않았습니다.
                ConnectingToNameServer, /// 네임 서버에 연결 중입니다.
                ConnectedToNameServer, /// 네임 서버에 연결되었습니다.
                Authenticating, /// 현재 서버에서 인증 중.
                Authenticated, /// 현재 서버에서 인증 완료.
                DisconnectingFromNameServer, /// 이름 서버에서 연결 해제 중입니다. 일반적으로 네임 서버에서 프론트엔드 서버로 전환하는 경우입니다.
                ConnectingToFrontEnd, /// 프론트엔드 서버에 연결 중.
                ConnectedToFrontEnd, /// 프론트엔드 서버에 연결됨.
                DisconnectingFromFrontEnd, /// 프론트엔드 서버에서 연결 해제 중.
                QueuedComingFromFrontEnd, /// 현재 사용되지 않음.
                Disconnecting, /// (모든 서버에서) 클라이언트가 연결을 끊습니다.
                Disconnected, /// 클라이언트가 더 이상 (모든 서버에) 연결되지 않았습니다.
                ConnectWithFallbackProtocol /// 클라이언트가 이름 서버에 연결할 수 없었으며 대체 네트워크 프로토콜(TCP)로 연결을 시도합니다.
            }
         */
        _state = state.ToString();
    }

    public void OnGetMessages(string channelName, string[] senders, object[] messages)
    {
        if (channelName.Equals(_selectedChannelName))
        {
            // update text
            ShowChannel(_selectedChannelName);
        }
    }

    public void OnPrivateMessage(string sender, object message, string channelName)
    {
        // ChatClient가 메시지를 버퍼링하고 있으므로 이 GUI는 여기서 아무것도 할 필요가 없습니다.
        // 직접 보낸 메시지도 받을 수 있습니다. 이 경우 채널 이름은 메시지의 대상에 따라 결정됩니다.
        
        // this.InstantiateChannelButton(channelName);
        //
        // byte[] msgBytes = message as byte[];
        // if (msgBytes != null)
        // {
        //     Debug.Log("Message with byte[].Length: "+ msgBytes.Length);
        // }
        // if (_selectedChannelName.Equals(channelName))
        // {
        //     ShowChannel(channelName);
        // }
    }

    public void OnSubscribed(string[] channels, bool[] results)
    {
        foreach (string channel in channels)
        {
            this.chatClient.PublishMessage(channel, $"{_userName} Join"); // you don't HAVE to send a msg on join but you could.
        }
        
        Debug.Log("OnSubscribed: " + string.Join(", ", channels));
        isConnected = true;
        ShowChannel(channels[0]);
    }

    public void OnUnsubscribed(string[] channels)
    {
        foreach (string channelName in channels)
        {
            
            if (channelName == _selectedChannelName)
            {
                ShowChannel(channelName);
            }
            else
            {
                Debug.Log("Can't unsubscribe from channel '" + channelName + "' because you are currently not subscribed to it.");
            }
        }
    }
    
    
    /// <summary>
    /// New status of another user (you get updates for users set in your friends list).
    /// </summary>
    /// <param name="user">Name of the user.</param>
    /// <param name="status">New status of that user.</param>
    /// <param name="gotMessage">True if the status contains a message you should cache locally. False: This status update does not include a
    /// message (keep any you have).</param>
    /// <param name="message">Message that user set.</param>

    public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
    {
        // 접속자 상태
        Debug.LogWarning("status: " + string.Format("{0} is {1}. Msg:{2}", user, status, message));

        // if (friendListItemLUT.ContainsKey(user))
        // {
        //     FriendItem _friendItem = friendListItemLUT[user];
        //     if ( _friendItem!=null) _friendItem.OnFriendStatusUpdate(status,gotMessage,message);
        // }
    }

    public void OnUserSubscribed(string channel, string user)
    {
    }
    public void OnUserUnsubscribed(string channel, string user)
    {
    }
}
