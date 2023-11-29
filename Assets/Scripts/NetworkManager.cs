using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System;
using UnityEngine.UI;
using System.Text;

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
        PhotonNetwork.JoinLobby();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogErrorFormat("Disconnected from server cause of {0}", cause);
        GameManager.Instance.ChangeScene(GameState.INTRO);
    }

    #region SET LOBBY SCENE
    public RoomManager LobbySceneManager;
    public void JoinDefaultRoom()
    {
        PhotonNetwork.NickName = GameManager.Instance.PlayerName;

        RoomOptions roomOptions = new()
        {
            IsOpen = true,
            IsVisible = true,
            MaxPlayers = maxPlayersPerRoom
        };
        PhotonNetwork.JoinOrCreateRoom("gameroom", roomOptions, TypedLobby.Default);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (RoomInfo roomInfo in roomList)
        {
            if (roomInfo.RemovedFromList)
            {
                if (LobbySceneManager != null)
                {
                    LobbySceneManager.RemoveRoom(roomInfo);
                }
            }
            else
            {
                if (LobbySceneManager != null)
                {
                    LobbySceneManager.AddRoom(roomInfo);
                }
            }
        }
    }

    public void CreateRoom(string _roomName)
    {
        RoomOptions roomOptions = new()
        {
            IsOpen = true,
            IsVisible = true,
            MaxPlayers = maxPlayersPerRoom
        };
        PhotonNetwork.CreateRoom(_roomName, roomOptions, TypedLobby.Default);
    }

    public void JoinRoom(string _roomName)
    {
        PhotonNetwork.JoinRoom(_roomName);
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
            ReadySceneManager.SetUI(playersStatus, playersStatus.Count, PhotonNetwork.IsMasterClient);
        }
    }

    // Player Ready
    public void SetPlayerReady(bool _isReady)
    {
        ExitGames.Client.Photon.Hashtable properties = new ExitGames.Client.Photon.Hashtable();
        properties.Add("IsReady", _isReady);
        PhotonNetwork.LocalPlayer.SetCustomProperties(properties);
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        if (changedProps.ContainsKey("IsReady"))
        {
            SyncPlayersData();
        }

        if (changedProps.ContainsKey("SettingDone"))
        {
            if(PhotonNetwork.IsMasterClient) CheckStart();
        }
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

    // when player leave room
    public void LeaveRoom()
    {
        ExitGames.Client.Photon.Hashtable properties = new ExitGames.Client.Photon.Hashtable();
        properties.Add("IsReady", false);
        PhotonNetwork.LocalPlayer.SetCustomProperties(properties);

        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        GameManager.Instance.ChangeScene(GameState.LOBBY);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if (otherPlayer == PhotonNetwork.LocalPlayer) return;
        SyncPlayersData();
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
        Vector2[] randomSpawnPosition = ShufflePosition(StaticVars.SpawnPosition);
        List<string> randomColor = StaticVars.Colors.OrderBy(_ => new System.Random().Next()).ToList();

        string commonCharacters = "0123456789ABCDEX";

        commonCharacters = ShuffleCharacter(commonCharacters).Substring(0, 3);          // 랜덤으로 세 글자
        Debug.Log(commonCharacters);

        foreach (Player player in PhotonNetwork.CurrentRoom.Players.Values)
        {
            isColloc = false;
            code = StaticFuncs.GeneratePlayerCode(commonCharacters);

            if (i == colloc)
            {
                isColloc = true;
                ExitGames.Client.Photon.Hashtable properties = new ExitGames.Client.Photon.Hashtable();
                properties.Add("CollocCode", code);
                properties.Add("CollocName", player.NickName);
                PhotonNetwork.CurrentRoom.SetCustomProperties(properties);
            }

            PV.RPC("SetPlayer", player, isColloc, randomSpawnPosition[i], randomColor[i]);
            PV.RPC("SetUserClue", RpcTarget.AllBuffered, randomCluePosition, player.NickName, code, randomColor[i]);
            PV.RPC("SetClueNote", RpcTarget.All, player.NickName, randomColor[i], i);
            PV.RPC("SetUserSelect", RpcTarget.All, player.NickName, randomColor[i], i);

            i++;
        }

        PV.RPC("SetOtherClues", RpcTarget.AllBuffered, randomCluePosition);

        int randomDropTime = UnityEngine.Random.Range(0, 10);
        Vector3[] randomDropPos = new Vector3[3];
        for (i = 0; i < 3; i++)
        {
            randomDropPos[i] = SetDropPos();
        }

        PV.RPC("SetItems", RpcTarget.AllBuffered, randomDropTime, randomDropPos);
    }

    // Set where gameobject will spawn
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

    // just shuffle...
    public string ShuffleCharacter(string _characters)
    {
        System.Random rand = new System.Random();

        StringBuilder result = new StringBuilder(_characters);

        for (int i = result.Length - 1; i > 0; i--)
        {
            int index = rand.Next(i + 1);

            char temp = result[index];
            result[index] = result[i];
            result[i] = temp;
        }

        return result.ToString();
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
    public void SetPlayer(bool _isColloc, Vector2 _spawnPos, String _color)
    {
        PlaySceneManager.SpawnHomes(_isColloc, _spawnPos, _color);
    }

    // Inform game item setting to all players (clue code, item time & position)
    [PunRPC]
    public void SetItems(int _dropTime, Vector3[] _dropPos)
    {
        PlaySceneManager.SetGame(_dropTime, _dropPos);
    }

    [PunRPC]
    public void SetOtherClues(Vector2[] _position)
    {
        _currentPlayer = PhotonNetwork.CurrentRoom.PlayerCount;

        PlaySceneManager.MakeOtherClueInstance(_position, ClueType.FAKE, 4);
        PlaySceneManager.MakeOtherClueInstance(_position, ClueType.CODE, 5);
    }

    [PunRPC]
    public void SetUserClue(Vector2[] _position, string _nickname, string _code, string _color)
    {
        _currentPlayer = PhotonNetwork.CurrentRoom.PlayerCount;
        PlaySceneManager.MakeUserClueInstance(_position, ClueType.USER, _nickname, _code, _color);
    }

    [PunRPC]
    public void SetClueNote(string _nickname, string _color, int _index)
    {
        Image noteSourceImage = UIManager.Instance.UserInfo.GetChild(_index).GetChild(2).GetChild(0).GetComponent<Image>();

        UIManager.Instance.UserInfo.GetChild(_index).GetChild(0).GetComponent<Text>().text = _nickname;
        switch (_color)
        {
            case "Brown":
                noteSourceImage.sprite = UIManager.Instance.playerSprite[0];
                break;
            case "Blue":
                noteSourceImage.sprite = UIManager.Instance.playerSprite[1];
                break;
            case "Gray":
                noteSourceImage.sprite = UIManager.Instance.playerSprite[2];
                break;
            case "Green":
                noteSourceImage.sprite = UIManager.Instance.playerSprite[3];
                break;
            case "Orange":
                noteSourceImage.sprite = UIManager.Instance.playerSprite[4];
                break;
            case "Pink":
                noteSourceImage.sprite = UIManager.Instance.playerSprite[5];
                break;
            case "Purple":
                noteSourceImage.sprite = UIManager.Instance.playerSprite[6];
                break;
            case "Yellow":
                noteSourceImage.sprite = UIManager.Instance.playerSprite[7];
                break;
        }

        PlaySceneManager.CheckReady(PlayManager.GameSettings.READY_CLUENOTE);
    }

    public void SetPlayerSettingDone()
    {
        ExitGames.Client.Photon.Hashtable properties = new ExitGames.Client.Photon.Hashtable();
        properties.Add("SettingDone", true);
        PhotonNetwork.LocalPlayer.SetCustomProperties(properties);
    }

    private void CheckStart()
    {
        foreach (Player player in PhotonNetwork.CurrentRoom.Players.Values)
        {
            // Check if player is ready
            if (player.CustomProperties.TryGetValue("SettingDone", out object isDone))
            {
                if ((bool)isDone == false) return;
            }
        }

        double endTime = PhotonNetwork.Time + StaticVars.GAME_TIME + StaticVars.START_PANEL_TIME;
        PV.RPC("SetGameStart", RpcTarget.AllBuffered, endTime);
    }

    [PunRPC]
    public void SetGameStart(double _endTime)
    {
        PlaySceneManager.StartGame(_endTime);
    }

    [PunRPC]
    public void SetUserSelect(string _nickname, string _color, int _index)
    {
        Image selectSourceImage = UIManager.Instance.HomesInfo.GetChild(_index).GetChild(1).GetComponent<Image>();

        UIManager.Instance.HomesInfo.GetChild(_index).GetChild(0).GetComponent<Text>().text = _nickname;
        switch (_color)
        {
            case "Brown":
                selectSourceImage.sprite = UIManager.Instance.playerSprite[0];
                break;
            case "Blue":
                selectSourceImage.sprite = UIManager.Instance.playerSprite[1];
                break;
            case "Gray":
                selectSourceImage.sprite = UIManager.Instance.playerSprite[2];
                break;
            case "Green":
                selectSourceImage.sprite = UIManager.Instance.playerSprite[3];
                break;
            case "Orange":
                selectSourceImage.sprite = UIManager.Instance.playerSprite[4];
                break;
            case "Pink":
                selectSourceImage.sprite = UIManager.Instance.playerSprite[5];
                break;
            case "Purple":
                selectSourceImage.sprite = UIManager.Instance.playerSprite[6];
                break;
            case "Yellow":
                selectSourceImage.sprite = UIManager.Instance.playerSprite[7];
                break;
        }
    }

    // To modify
    [PunRPC]
    public void SyncHiddenCode(int _index)
    {
        Clue clue = PlaySceneManager.ClueInstances[_index].GetComponent<HandleClue>().clue;

        clue.IsHidden = true;

        StartCoroutine(UnHiddenClue(clue));
    }

    IEnumerator UnHiddenClue(Clue clue)
    {
        yield return new WaitForSeconds(15.0f);
        clue.IsHidden = false;
    }
    #endregion
    #region SERVER UTILS
    public double GetServerTime()
    {
        return PhotonNetwork.Time;
    }
    #endregion
}
