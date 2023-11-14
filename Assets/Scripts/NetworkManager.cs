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

    static public int _currentPlayer;

    // Singleton ����
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
        int colloc = UnityEngine.Random.Range(0, PhotonNetwork.CurrentRoom.PlayerCount);
        bool isColloc = false;
        string code;
        Vector2[] randomCluePosition = ShufflePosition(StaticVars.CluePosition);

        foreach (Player player in PhotonNetwork.CurrentRoom.Players.Values)
        {
            isColloc = false;

            code = StaticFuncs.GeneratePlayerCode();

            if (i == colloc)
            {
                isColloc = true;
                ExitGames.Client.Photon.Hashtable properties = new ExitGames.Client.Photon.Hashtable();
                properties.Add("CollocCode", code);
                PhotonNetwork.CurrentRoom.SetCustomProperties(properties);
            }

            PV.RPC("SetPlayer", player, isColloc, i);
            i++;
            
            PV.RPC("SetUserClues", RpcTarget.AllBuffered, randomCluePosition, player.NickName, code);
        }

        PV.RPC("SetOtherClues", RpcTarget.AllBuffered, randomCluePosition);

        int randomDropTime = UnityEngine.Random.Range(0, 10);
        Vector3[] randomDropPos = new Vector3[3];
        for (i = 0; i < 3; i++)
        {
            randomDropPos[i] = SetDropPos();
        }

        double endTime = PhotonNetwork.Time + StaticVars.GAME_TIME;
        PV.RPC("SetItems", RpcTarget.AllBuffered, randomDropTime, randomDropPos, endTime);
    }

    // Set where clue will spawn
    public Vector2[] ShufflePosition(Vector2[] _position)
    {
        System.Random rand = new System.Random();

        for (int i = _position.Length - 1; i > 0; i--)
        {
            int index = rand.Next(i + 1);

            Vector2 temp = _position[index];
            _position[index] = _position[i];
            _position[i] = temp;
        }

        return _position;
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
    public void SetItems(int _dropTime, Vector3[] _dropPos, double _endTime)
    {
        PlaySceneManager.SetGame(_dropTime, _dropPos, _endTime);
    }

    [PunRPC]
    public void SetOtherClues(Vector2[] _position)
    {
        _currentPlayer = PhotonNetwork.CurrentRoom.PlayerCount;

        PlaySceneManager.MakeOtherClueInstance(_position, ClueType.FAKE, 4);
        PlaySceneManager.MakeOtherClueInstance(_position, ClueType.CODE, 5);
    }

    [PunRPC]
    public void SetUserClues(Vector2[] _position, string _nickname, string _code)
    {
        _currentPlayer = PhotonNetwork.CurrentRoom.PlayerCount;

        PlaySceneManager.MakeUserClueInstance(_position, ClueType.USER, _currentPlayer, _nickname, _code);
    }

    // To modify
    [PunRPC]
    public void SyncHiddenCode(int _index)
    {
        Clue clue = PlayManager.Instance.ClueInstances[_index].GetComponent<HandleClue>().clue;

        clue.IsHidden = true;

        StartCoroutine(UnHiddenClue(clue));
    }

    IEnumerator UnHiddenClue(Clue clue)
    {
        yield return new WaitForSeconds(15.0f);
        clue.IsHidden = false;
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
