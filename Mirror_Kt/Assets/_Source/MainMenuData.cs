using Mirror;
using UnityEngine;
using UnityEngine.UI;


public class MainMenuData : MonoBehaviour
{
    [SerializeField] private NetworkManager networkManager;
    [SerializeField] private Button clientBtn;
    [SerializeField] private Button hostBtn;

    public NetworkManager NetworkManager => networkManager;
    public Button ClientBtn => clientBtn;
    public Button HostBtn => hostBtn;
}
