using System;
using System.Collections.Generic;
using Quantum;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace QuantumUser
{
    public class MainMenuUIHandler : QuantumCallbacks
    {

        [Header("Panels")] 
        [SerializeField] private Canvas mainCanvas;
        [SerializeField] private CanvasGroup mainPanel;
        [SerializeField] private CanvasGroup playerPanel;
        [SerializeField] private CanvasGroup searchingPanel;
        [SerializeField] private CanvasGroup loadingPanel;
        [SerializeField] private CanvasGroup settingPanel;


        [Header("----------HUD-----------")]
        [SerializeField] private Image profileIcon;
        [SerializeField] private TextMeshProUGUI playerNickname;
        [SerializeField] private TextMeshProUGUI playersFoundText;

        
        
        [Header("----------Nickname Screen-----------")] 
        [SerializeField] private TMP_InputField nicknameInputField;


        [Header("----------Setting Screen-----------")] 
        [SerializeField] private TMP_Dropdown regionDropdown;
        [SerializeField] private TMP_Dropdown graphicQualityDropdown;
        [SerializeField] private Slider bgmSlider;
        [SerializeField] private Slider sfxSlider;

        public void Start()
        {
            InitGraphicsDropdown();
            bgmSlider.SetValueWithoutNotify(AudioManager.Instance.bgmVol);
            sfxSlider.SetValueWithoutNotify(AudioManager.Instance.sfxVol);

            if (PlayerPrefs.HasKey("Nickname"))
            {
                Debug.Log("Nickname loaded");
                string nickname = PlayerPrefs.GetString("Nickname");
                LocalData.Nickname = nickname;
                SetNicknameInput(nickname);
            }
            else
            {
                Debug.Log("No Nickname found");
            }
            
            Matchmaker.OnRealtimeJoinedRoom += OnConnectUserChanged;

            Matchmaker.OnRealtimePlayerJoined += p =>
            {
                OnConnectUserChanged();
            };

            Matchmaker.OnRealtimePlayerLeft += p =>
            {
                OnConnectUserChanged();
            };
        }

        void OnConnectUserChanged()
        {
            playersFoundText.text = $"{Matchmaker.Client.CurrentRoom.PlayerCount}/{Matchmaker.Client.CurrentRoom.MaxPlayers}";
        }
        #region UI Code
        void HideAllPanels()
        {
            mainPanel.gameObject.SetActive(false);
            searchingPanel.gameObject.SetActive(false);
            loadingPanel.gameObject.SetActive(false);
        }

        public void HideAllPopupPanels()
        {
            settingPanel.gameObject.SetActive(false);
            playerPanel.gameObject.SetActive(false);
        }
        #endregion

        #region UI Event

        public void OnPlayerOpen()
        {
            HideAllPopupPanels();
            playerPanel.gameObject.SetActive(true);
        }
        public void OnSettingOpen()
        {
            HideAllPopupPanels();
            settingPanel.gameObject.SetActive(true);
        }
        
        public void SetNicknameInput(string nickname)
        {
            nicknameInputField.SetTextWithoutNotify(nickname);
            playerNickname.text = nickname;
        }
        public void SetNickname()
        {
            if (!string.IsNullOrWhiteSpace(nicknameInputField.text))
            {
                LocalData.Nickname = nicknameInputField.text;
                PlayerPrefs.SetString("Nickname", nicknameInputField.text);
            }
            else
            {
                if (PlayerPrefs.HasKey("Nickname"))
                {
                    LocalData.Nickname = PlayerPrefs.GetString("Nickname");
                    Debug.Log("Nickname loaded");
                }
                else
                {
                    LocalData.Nickname = null;
                }
            }
            
            playerNickname.text = LocalData.Nickname;

        }

        public void SetBgmVolume(float volume)
        {
            AudioManager.Instance.BgmVol(volume);
        }
        
        public void SetSfxVolume(float volume)
        {
            AudioManager.Instance.SfxVol(volume);

        }
        
        public void StartQueueHook()
        {
            StartQueue();
        }

        private void StartQueue()
        {
            Matchmaker.Connect(status =>
            {
                Debug.Log($"[{status.State}] {status.Message}");


                if (status.State == Matchmaker.State.ConnectingToServer)
                {

                }
                else if (status.State == Matchmaker.State.ConnectingToRoom)
                {

                }
                else if (status.State == Matchmaker.State.JoinedRoom)
                {
                    Debug.Log("<color=#FF8080>Game Started</color>");
                    HideAllPopupPanels();
                    searchingPanel.gameObject.SetActive(true);
                }
                else if (status.State == Matchmaker.State.GameStarted)
                {

                }
                else if (status.State == Matchmaker.State.Failed)
                {
                    Debug.Log($"<color=#FF8080>{status.State.ToString()} : {status.Message}</color>");
                }
            });
        }
        
        public void DisconnectHook()
        {
            // disconnect if we're connected, or just perform the logic if we're in singleplayer
            if (Matchmaker.Client.IsConnected)
                Matchmaker.Client.Disconnect();
            else
                Matchmaker.Instance.OnDisconnected(default);
        }
        
        public void InitGraphicsDropdown()
        {
            string[] names = QualitySettings.names;
            List<string> options = new List<string>();

            for (int i = 0; i < names.Length; i++)
            {
                options.Add(names[i]);
            }
            graphicQualityDropdown.AddOptions(options);
            QualitySettings.SetQualityLevel(graphicQualityDropdown.options.Count - 1);
            graphicQualityDropdown.value = graphicQualityDropdown.options.Count - 1;
        }

        
        public void SetGraphicsQuality()
        {
            QualitySettings.SetQualityLevel(graphicQualityDropdown.value);
        }
        #endregion


    }
}