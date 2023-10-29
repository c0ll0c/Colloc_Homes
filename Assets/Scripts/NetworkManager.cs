using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using System;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private byte maxPlayersPerRoom = 6;

    public PhotonView PV;

    // Singleton 선언
    public static NetworkManager Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        PhotonNetwork.AutomaticallySyncScene = true;
    }

    // 게임 끝나고, 혹은 뒤로 가기 버튼으로 Intro Scene으로 다시 이동할 때 불린다
    // PhotonView의 복제 방지 및 PhotonNetwork 연결 해제
    public void DisconnectAndDestroy()
    {
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.Disconnect();
        }
        if (Instance != null)
        {
            Destroy(gameObject);
        }
    }

    // Master Server에 연결 후 lobby 입장
    public void Connect()
    {
        PhotonNetwork.NickName = GameManager.Instance.PlayerName;
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        GameManager.Instance.ChangeScene(GameState.LOBBY);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogErrorFormat("Disconnected from server cause of {0}", cause);
        GameManager.Instance.ChangeScene(GameState.INTRO);
    }


    // 기본 게임룸(readyscene) 입장
    // [TODO] 추후에 방 생성 / 방 목록 보고 방 참가로 변경 예정
    public void JoinDefaultRoom()
    {
        PhotonNetwork.NickName = GameManager.Instance.PlayerName;

        RoomOptions roomOptions = new ()
        {
            IsOpen = true,
            IsVisible = true,
            MaxPlayers = maxPlayersPerRoom
        };
        PhotonNetwork.JoinOrCreateRoom("gameroom", roomOptions, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        GameManager.Instance.ChangeScene(GameState.READY);
        PV.RPC("SyncPlayersData", RpcTarget.OthersBuffered);
    }


    // ReadySceneManager 관리
    public ReadyManager ReadySceneManager;

    // PlayerData 변경 시마다 ReadyScene(or PlayScene)의 Manager Call
    [PunRPC]
    public void SyncPlayersData()
    {
        List<PlayerData> playersStatus = new List<PlayerData>();
        
        // Check if CurrentRoom is not null
        if (PhotonNetwork.CurrentRoom != null)
        {
            foreach (Player player in PhotonNetwork.CurrentRoom.Players.Values)
            {
                // Check if player is not null
                if (player != null)
                {
                    PlayerData playerData = new PlayerData(player);
                    playerData.IsMaster = player.IsMasterClient;
                    playersStatus.Add(playerData);
                }
            }
        }

        if (ReadySceneManager != null)
        {
            ReadySceneManager.SetUI(playersStatus, PhotonNetwork.CurrentRoom.PlayerCount, PhotonNetwork.IsMasterClient);
        }
    }

    // Player Ready
    public void SetPlayerReady(bool _isReady)
    {
        ExitGames.Client.Photon.Hashtable properties = new ExitGames.Client.Photon.Hashtable();
        properties.Add("IsReady", _isReady);
        PhotonNetwork.LocalPlayer.SetCustomProperties(properties);
    }

    // Start Game by Master
    public void StartGame()
    {
        foreach (Player player in PhotonNetwork.CurrentRoom.Players.Values)
        {
            object IsReady;
            // Check if player is ready
            if (!player.IsMasterClient && player.CustomProperties.TryGetValue("IsReady", out IsReady))
            {
                if ((bool)IsReady == false) return;
            }
            else if (!player.IsMasterClient) return;
        }

        PhotonNetwork.LoadLevel("PlayScene");
    }


    // PlaySceneManager
    public PlayManager PlaySceneManager;

    // Generate players, codes, items(drop time, position): called by MasterClient
    public void GameSetting()
    {
        int i = 0;
        int colloc = UnityEngine.Random.Range(0, PhotonNetwork.CurrentRoom.PlayerCount + 1);
        bool isColloc = false;
        string code;
        Dictionary<string, string> codes = new Dictionary<string, string>();

        foreach (Player player in PhotonNetwork.CurrentRoom.Players.Values )
        {
            code = StaticFuncs.GeneratePlayerCode();
            codes.Add(player.NickName, code);
            if (i == colloc)
            {
                isColloc = true;
                ExitGames.Client.Photon.Hashtable properties = new ExitGames.Client.Photon.Hashtable();
                properties.Add("CollocCode", code);
                PhotonNetwork.CurrentRoom.SetCustomProperties(properties);
            }
            PV.RPC("SetPlayer", player, isColloc, i);
            i++; isColloc = false;
        }

        int randomDropTime = UnityEngine.Random.Range(0, 10);
        Vector3[] randomDropPos = new Vector3[3];
        for (i = 0; i < 3; i++)
        {
            randomDropPos[i] = SetDropPos();
        }

        PV.RPC("SetItems", RpcTarget.AllBuffered, codes, randomDropTime, randomDropPos);
    }

    // Set where item will drop
    private Vector3 SetDropPos()
    {
        Vector3 randomDropPos = new Vector3(UnityEngine.Random.Range(-3f, 10f), UnityEngine.Random.Range(-8f, 17f), 0f);
        if (StaticFuncs.CheckOnWall(randomDropPos))
        {
            return SetDropPos();
        }
        return randomDropPos;
    }

    // Set each player status, spawn position
    [PunRPC]
    public void SetPlayer(bool _isColloc, int _idx)
    {
        PlaySceneManager.SpawnHomes(_isColloc, _idx);
    }

    // Inform game item setting to all players (clue code, item time & position)
    [PunRPC]
    public void SetItems(Dictionary<string, string> _codes, int _dropTime, Vector3[] _dropPos)
    {
        PlaySceneManager.SetGame(_codes, _dropTime, _dropPos);
    }
}
