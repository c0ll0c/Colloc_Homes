using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using System.Collections;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private byte maxPlayersPerRoom = StaticVars.MAX_PLAYERS_PER_ROOM;

    public PhotonView PV;

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

    #region SET LOBBY SCENE
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
    #endregion
    #region SET READY SCENE
    public override void OnJoinedRoom()
    {
        GameManager.Instance.ChangeScene(GameState.READY);
        PV.RPC("SyncPlayersData", RpcTarget.OthersBuffered);
    }

    public ReadyManager ReadySceneManager;
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
            // Check if player is ready
            if (!player.IsMasterClient && player.CustomProperties.TryGetValue("IsReady", out object IsReady))
            {
                if ((bool)IsReady == false) return;
            }
            else if (!player.IsMasterClient) return;
        }

        PhotonNetwork.LoadLevel("PlayScene");
    }
    #endregion
    #region SET PLAY SCENE
    // PlaySceneManager
    public PlayManager PlaySceneManager;

    // Generate players, codes, items(drop time, position): called by MasterClient
    public void GameSetting()
    {
        int i = 0;
        int colloc = Random.Range(0, PhotonNetwork.CurrentRoom.PlayerCount + 1);
        bool isColloc = false;
        string code;
        Dictionary<string, string> codes = new Dictionary<string, string>();

        foreach (Player player in PhotonNetwork.CurrentRoom.Players.Values)
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

        int randomDropTime = Random.Range(0, 10);
        Vector3[] randomDropPos = new Vector3[3];
        for (i = 0; i < 3; i++)
        {
            randomDropPos[i] = SetDropPos();
        }

        double endTime = PhotonNetwork.Time + StaticVars.GAME_TIME;
        PV.RPC("SetItems", RpcTarget.All, codes, randomDropTime, randomDropPos, endTime);
    }

    // Set where item will drop
    private Vector3 SetDropPos()
    {
        Vector3 randomDropPos = new Vector3(Random.Range(-3f, 10f), Random.Range(-8f, 17f), 0f);
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
    public void SetItems(Dictionary<string, string> _codes, int _dropTime, Vector3[] _dropPos, double _endTime)
    {
        PlaySceneManager.SetGame(_codes, _dropTime, _dropPos, _endTime);
    }

    // To modify
    [PunRPC]
    public void SyncHiddenCode(bool _isHidden, bool _state)
    {
        _isHidden = _state;
        StartCoroutine(UnHiddenClue());
    }

    IEnumerator UnHiddenClue()
    {
        yield return new WaitForSeconds(15.0f);
        // IsHidden = false;
    }
    #endregion

    #region MANAGE GAME PLAY
    public void SyncDetox(int _index) 
    {
        PV.RPC("DeactivateDetox", RpcTarget.All, _index);
    }
    [PunRPC]
    public void DeactivateDetox(int _index)
    {
        PlaySceneManager.UseOrDeactivateDetox(_index);
    }
    #endregion

    #region SERVER UTILS
    public double GetServerTime()
    {
        return PhotonNetwork.Time;
    }
    #endregion
}
