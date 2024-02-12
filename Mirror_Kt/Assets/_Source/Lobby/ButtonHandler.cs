using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Lobby
{
    public class ButtonHandler : MonoBehaviour
    {
        [SerializeField] private Button hostBtn;
        [SerializeField] private Button joinBtn;
        [SerializeField] private Button startBtn;
        [SerializeField] private Button applyBtn;
        [SerializeField] private Button searchBtn;
        [SerializeField] private Button gameLeaveBtn;
        [SerializeField] private Button lobbyLeaveBtn;
        [SerializeField] private Button cancelBtn;
        [SerializeField] private Button errorCloseBtn;
        [SerializeField] private Button exitBtn;
        [SerializeField] private Button chooseBtn;
        [SerializeField] private Button nextCharacterBtn;
        [SerializeField] private Button previousCharacterBtn;
        [SerializeField] private Button sendBtn;
        [SerializeField] private Button nextMapBtn;
        [SerializeField] private Button previousMapBtn;
        [SerializeField] private TMP_InputField nicknameInput;
        [SerializeField] private TMP_InputField messageInput;

        [SerializeField] private ConnectionController connectionController;

        private void Start()
        {
            hostBtn.onClick.AddListener(connectionController.Host);
            joinBtn.onClick.AddListener(connectionController.Join);
            startBtn.onClick.AddListener(connectionController.StartGame);
            applyBtn.onClick.AddListener(connectionController.SaveName);
            searchBtn.onClick.AddListener(connectionController.SearchGame);
            lobbyLeaveBtn.onClick.AddListener(connectionController.DisconnectGame);
            gameLeaveBtn.onClick.AddListener(connectionController.DisconnectGame);
            cancelBtn.onClick.AddListener(connectionController.CancelSearchGame);
            errorCloseBtn.onClick.AddListener(connectionController.Enable);
            exitBtn.onClick.AddListener(connectionController.Exit);
            chooseBtn.onClick.AddListener(connectionController.Choose);
            nextCharacterBtn.onClick.AddListener(delegate { connectionController.ChangeIndex(false); });
            previousCharacterBtn.onClick.AddListener(delegate { connectionController.ChangeIndex(true); });
            sendBtn.onClick.AddListener(connectionController.HandleMessage);
            nextMapBtn.onClick.AddListener(delegate { connectionController.ChangeMapIndex(false); });
            previousMapBtn.onClick.AddListener(delegate { connectionController.ChangeMapIndex(true); });
            nicknameInput.onValueChanged.AddListener(connectionController.SetName);
            messageInput.onValueChanged.AddListener(connectionController.SetSendButtonActive);
        }

        private void OnDestroy()
        {
            hostBtn.onClick.RemoveListener(connectionController.Host);
            joinBtn.onClick.RemoveListener(connectionController.Join);
            startBtn.onClick.RemoveListener(connectionController.StartGame);
            applyBtn.onClick.RemoveListener(connectionController.SaveName);
            searchBtn.onClick.RemoveListener(connectionController.SearchGame);
            gameLeaveBtn.onClick.RemoveListener(connectionController.DisconnectGame);
            lobbyLeaveBtn.onClick.RemoveListener(connectionController.DisconnectGame);
            cancelBtn.onClick.RemoveListener(connectionController.CancelSearchGame);
            errorCloseBtn.onClick.RemoveListener(connectionController.Enable);
            exitBtn.onClick.RemoveListener(connectionController.Exit);
            chooseBtn.onClick.RemoveListener(connectionController.Choose);
            nextCharacterBtn.onClick.RemoveListener(delegate { connectionController.ChangeIndex(false); });
            previousCharacterBtn.onClick.RemoveListener(delegate { connectionController.ChangeIndex(true); });
            sendBtn.onClick.AddListener(connectionController.HandleMessage);
            nicknameInput.onValueChanged.RemoveListener(connectionController.SetName);
            messageInput.onValueChanged.RemoveListener(connectionController.SetSendButtonActive);
        }
    }
}