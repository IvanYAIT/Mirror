using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace PlayerCharacter
{
    public class Player : NetworkBehaviour
    {
        bool facingRight = true;
        public static Player localPlayer;
        public TextMesh NameDisplayText;
        [SyncVar] public Color PlayerColor;
        [SyncVar(hook = "DisplayPlayerName")] public string PlayerDisplayName;
        [SyncVar] public string matchID;

        [SyncVar] public Match CurrentMatch;
        public GameObject PlayerLobbyUI;
        public SpriteRenderer sprite;

        private Guid netIDGuid;

        [Inject] private GameObject _gameUI;

        private NetworkMatch networkMatch;
        private Animator animator;

        private void Awake()
        {
            networkMatch = GetComponent<NetworkMatch>();
            //_gameUI = GameObject.FindGameObjectWithTag("GameUI");
        }

        private void Start()
        {
            if (isLocalPlayer)
            {
                animator = GetComponent<Animator>();

                CmdSendName(ConnectionController.instance.DisplayName, ConnectionController.instance.PlayerColor);
            }
        }

        public override void OnStartServer()
        {
            netIDGuid = netId.ToString().ToGuid();
            networkMatch.matchId = netIDGuid;
        }

        public override void OnStartClient()
        {
            if (isLocalPlayer)
            {
                localPlayer = this;
            }
            else
            {
                PlayerLobbyUI = ConnectionController.instance.SpawnPlayerUIPrefab(this);
            }
        }

        public override void OnStopClient()
        {
            ClientDisconnect();
        }

        public override void OnStopServer()
        {
            ServerDisconnect();
        }

        [Command]
        void CmdSendColor(int index)
        {
            RpcSendColor(index);
        }

        [ClientRpc]
        void RpcSendColor(int index)
        {
            sprite.color = ConnectionController.instance.Characters[index].Color;
        }

        [Client]
        void SendSprites()
        {
            if (isLocalPlayer)
            {
                CmdSendColor(PlayerPrefs.GetInt("index"));
            }
        }

        [Command]
        public void CmdSendName(string name, Color color)
        {
            PlayerDisplayName = name;
            PlayerColor = color;
        }

        public void DisplayPlayerName(string name, string playerName)
        {
            name = PlayerDisplayName;
            NameDisplayText.text = playerName;
        }

        [Command]
        public void CmdHandleMessage(string message)
        {
            RpcHandleMessage($"<color=#{ColorUtility.ToHtmlStringRGB(PlayerColor)}>{PlayerDisplayName}:</color> {message}");
        }

        [ClientRpc]
        void RpcHandleMessage(string message)
        {
            ConnectionController.instance.SendMessageToServer(message);
        }

        public void HostGame(bool publicMatch, int mapIndex)
        {
            string ID = ConnectionController.GetRandomID();
            CmdHostGame(ID, publicMatch, mapIndex);
        }

        [Command]
        public void CmdHostGame(string ID, bool publicMatch, int mapIndex)
        {
            matchID = ID;
            if (ConnectionController.instance.HostGame(ID, gameObject, publicMatch, mapIndex))
            {
                networkMatch.matchId = ID.ToGuid();
                TargetHostGame(true, ID, mapIndex);
            }
            else
            {
                TargetHostGame(false, ID, mapIndex);
            }
        }

        [TargetRpc]
        void TargetHostGame(bool success, string ID, int mapIndex)
        {
            matchID = ID;
            ConnectionController.instance.HostSuccess(success, ID, mapIndex);
        }

        public void JoinGame(string inputID)
        {
            CmdJoinGame(inputID);
        }

        [Command]
        public void CmdJoinGame(string ID)
        {
            matchID = ID;
            if (ConnectionController.instance.JoinGame(ID, gameObject))
            {
                networkMatch.matchId = ID.ToGuid();
                TargetJoinGame(true, ID);
            }
            else
            {
                TargetJoinGame(false, ID);
            }
        }

        [TargetRpc]
        void TargetJoinGame(bool success, string ID)
        {
            matchID = ID;
            ConnectionController.instance.JoinSuccess(success, ID);
            Invoke(nameof(SetLobbyMap), 0.1f);
        }

        void SetLobbyMap()
        {
            ConnectionController.instance.SetLobbyMap(CurrentMatch.Map);
        }

        public void DisconnectGame()
        {
            CmdDisconnectGame();
        }

        [Command(requiresAuthority = false)]
        void CmdDisconnectGame()
        {
            ServerDisconnect();
        }

        void ServerDisconnect()
        {
            ConnectionController.instance.PlayerDisconnected(gameObject, matchID);
            RpcDisconnectGame();
            networkMatch.matchId = netIDGuid;
        }

        [ClientRpc]
        void RpcDisconnectGame()
        {
            ClientDisconnect();
        }

        void ClientDisconnect()
        {
            if (PlayerLobbyUI != null)
            {
                if (!isServer)
                {
                    Destroy(PlayerLobbyUI);
                }
                else
                {
                    PlayerLobbyUI.SetActive(false);
                }
            }
        }

        public void SearchGame()
        {
            CmdSearchGame();
        }

        [Command]
        void CmdSearchGame()
        {
            if (ConnectionController.instance.SearchGame(gameObject, out matchID))
            {
                networkMatch.matchId = matchID.ToGuid();
                TargetSearchGame(true, matchID);

                if (isServer && PlayerLobbyUI != null)
                {
                    PlayerLobbyUI.SetActive(true);
                }
            }
            else
            {
                TargetSearchGame(false, matchID);
            }
        }

        [TargetRpc]
        void TargetSearchGame(bool success, string ID)
        {
            matchID = ID;
            ConnectionController.instance.SearchGameSuccess(success, ID);
            Invoke(nameof(SetLobbyMap), 0.1f);
        }

        [Server]
        public void PlayerCountUpdated(int playerCount)
        {
            TargetPlayerCountUpdated(playerCount);
        }

        [TargetRpc]
        void TargetPlayerCountUpdated(int playerCount)
        {
            if (playerCount > 1)
            {
                ConnectionController.instance.SetBeginButtonActive(true);
            }
            else
            {
                ConnectionController.instance.SetBeginButtonActive(false);
            }
        }

        public void BeginGame()
        {
            CmdBeginGame();
        }

        [Command]
        public void CmdBeginGame()
        {
            ConnectionController.instance.BeginGame(matchID);
        }

        public void StartGame()
        {
            TargetBeginGame();
        }

        [TargetRpc]
        void TargetBeginGame()
        {
            Player[] players = FindObjectsOfType<Player>();
            for (int i = 0; i < players.Length; i++)
            {
                DontDestroyOnLoad(players[i]);
            }

            _gameUI.GetComponent<Canvas>().enabled = true;
            ConnectionController.instance.inGame = true;
            SendSprites();
            transform.localScale = new Vector3(0.41664f, 0.41664f, 0.41664f);
            facingRight = true;
            SceneManager.LoadScene(ConnectionController.instance.Maps[CurrentMatch.Map].MapScene, LoadSceneMode.Additive);
            Invoke(nameof(SetPlayer), 0.1f);
        }

        void SetPlayer()
        {
            GameObject[] spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");
            transform.position = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)].transform.position;
        }
    }
}