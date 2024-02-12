using Mirror;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Network
{
    public class Connection : MonoBehaviour
    {
        private NetworkManager _networkManager;
        private Button _clientBtn;
        private Button _hostBtn;

        [Inject]
        public void Construct(MainMenuData data)
        {
            _networkManager = data.NetworkManager;
            _clientBtn = data.ClientBtn;
            _hostBtn = data.HostBtn;

            _clientBtn.onClick.AddListener(JoinClient);
            _hostBtn.onClick.AddListener(_networkManager.StartHost);
        }


        private void OnDestroy()
        {
            _clientBtn.onClick.RemoveListener(JoinClient);
            _hostBtn.onClick.RemoveListener(_networkManager.StartHost);
        }

        private void JoinClient()
        {
            _networkManager.networkAddress = "localhost";
            _networkManager.StartClient();
        } 
    }
}