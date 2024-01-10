using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System;
using UnityEngine.UI;
using System.Text;
using UnityEngine.U2D.Animation;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private byte maxPlayersPerRoom = StaticVars.MAX_PLAYERS_PER_ROOM;

    public PhotonView PV;

    static public int _currentPlayer;

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
    public LobbyManager LobbySceneManager;

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

    public void CreateRoom(string _roomName, bool _isVisible, string password)
    {
        PhotonNetwork.NickName = GameManager.Instance.PlayerName;

        ExitGames.Client.Photon.Hashtable hash = new ExitGames.Client.Photon.Hashtable();
        hash.Add(StaticCodes.PHOTON_PROP_SLOTS, 0b111111);
        
        RoomOptions roomOptions = new()
        {
            IsOpen = true,
            IsVisible = _isVisible,
            MaxPlayers = 6,
            CustomRoomProperties = hash,
        };
        PhotonNetwork.CreateRoom(_roomName, roomOptions, TypedLobby.Default);
    }

    public void JoinRoom(string _roomName)
    {
        PhotonNetwork.NickName = GameManager.Instance.PlayerName;

        PhotonNetwork.JoinRoom(_roomName);
    }
    #endregion
    #region SET READY SCENE
    public override void OnJoinedRoom()
    {
        int ableColor = 0;

        if (PhotonNetwork.CurrentRoom != null)
        {
            foreach (Player player in PhotonNetwork.CurrentRoom.Players.Values)
            {
                if (player != PhotonNetwork.LocalPlayer)
                {
                    if (player.NickName == PhotonNetwork.NickName)
                    {
                        PhotonNetwork.NickName = player.NickName + "1";
                    }
                    player.CustomProperties.TryGetValue(StaticCodes.PHOTON_PROP_COLOR, out object color);
                    ableColor |= (int)color;
                }
            }
        }

        ableColor ^= StaticVars.PLAYER_COLORS;
        int playerColor = ableColor & (-ableColor);

        AddCustomPropertiesToLocal(StaticCodes.PHOTON_PROP_COLOR, playerColor);

        GameManager.Instance.ChangeScene(GameState.READY);

        PV.RPC("SyncPlayersData", RpcTarget.All);
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
        playersStatus.Sort(delegate (PlayerData p1, PlayerData p2) { return p1.Id.CompareTo(p2.Id); });

        if (ReadySceneManager != null)
        {
            PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue(StaticCodes.PHOTON_PROP_SLOTS, out object slots);
            ReadySceneManager.SetUI(PhotonNetwork.CurrentRoom.Name, "ABCDEF", playersStatus, (int)slots, PhotonNetwork.IsMasterClient);
        }
    }

    // Player Ready
    public bool SetPlayerReady(bool _isReady)
    {
        // Check other players' color
        foreach (Player player in PhotonNetwork.CurrentRoom.Players.Values)
        {
            if (!player.IsLocal && player.CustomProperties.TryGetValue(StaticCodes.PHOTON_PROP_COLOR, out object color))
            {
                if ((int)color == ReadySceneManager.color) 
                {
                    Debug.LogError("other player occupied that color");
                    return false; 
                }
            }
        }

        AddCustomPropertiesToLocal(StaticCodes.PHOTON_PROP_ISREADY, _isReady);
        AddCustomPropertiesToLocal(StaticCodes.PHOTON_PROP_COLOR, ReadySceneManager.color);
        return _isReady;
    }

    private int maxPlayers = StaticVars.MAX_PLAYERS_PER_ROOM;
    // _index: 해제될 슬롯
    // _isAble: true(x->?), false(?->x)
    public bool SetSlotAble(int _index, bool _makeAvailable)
    {
        if (!PhotonNetwork.IsMasterClient) return false;

        bool res = PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue(StaticCodes.PHOTON_PROP_SLOTS, out object slots);
        if (!res) return false;

        int availableSlots = (int)slots;
        if (_makeAvailable)
        {
            maxPlayers++;
            availableSlots |= (1 << _index);
        }
        else if (maxPlayers > 4)
        {
            maxPlayers--;
            availableSlots &= (~(1 << _index));
        }
        else return false;

        // Set as Room's custom properties for new players
        var hash = PhotonNetwork.CurrentRoom.CustomProperties;
        hash[StaticCodes.PHOTON_PROP_SLOTS] = availableSlots;
        PhotonNetwork.CurrentRoom.SetCustomProperties(hash);

        // For current players in the room, notify blocked/unblocked playerSlots
        PV.RPC("SyncPlayerSlots", RpcTarget.All, availableSlots);
        return true;
    }

    [PunRPC]
    public void SyncPlayerSlots(int _slots)
    {
        ReadySceneManager.SetSlotBlock(_slots);
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        if (changedProps.ContainsKey(StaticCodes.PHOTON_PROP_ISREADY))
        {
            SyncPlayersData();
        }

        if (changedProps.ContainsKey(StaticCodes.PHOTON_PROP_SYNC))
        {
            if(PhotonNetwork.IsMasterClient) CheckStart();
        }
    }

    // Start Game by Master
    public void StartGame()
    {
        foreach (Player player in PhotonNetwork.CurrentRoom.Players.Values)
        {
            if (!player.IsMasterClient)
            {
                // Check if player is ready
                if (player.CustomProperties.TryGetValue(StaticCodes.PHOTON_PROP_ISREADY, out object IsReady))
                {
                    Debug.Log(IsReady);
                    if (!(bool)IsReady) return;
                }
                else return;
                if (player.CustomProperties.TryGetValue(StaticCodes.PHOTON_PROP_COLOR, out object color))
                {
                    if ((int)color == ReadySceneManager.color)
                    {
                        Debug.LogError("other player occupied that color");
                        return;
                    }
                }
            }
        }

        PhotonNetwork.LoadLevel("PlayScene");

        foreach (Player player in PhotonNetwork.CurrentRoom.Players.Values)
        {
            ExitGames.Client.Photon.Hashtable properties = new ExitGames.Client.Photon.Hashtable();
            properties.Add(StaticCodes.PHOTON_PROP_ISREADY, false);
            player.SetCustomProperties(properties);
        }
    }

    public void KickPlayerOut(int _playerIndex) 
    {
        // TODO;
    }

    // when player leave room
    public void LeaveRoom()
    {
        SetPlayerReady(false);
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

        string commonCharacters = "0123456789ABCDEX";

        commonCharacters = ShuffleCharacter(commonCharacters).Substring(0, 3);          // 랜덤으로 세 글자

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
            player.CustomProperties.TryGetValue(StaticCodes.PHOTON_PROP_COLOR, out object color);
            string colorStr = StaticFuncs.GetColorName((int)color);

            PV.RPC("SetPlayer", player, isColloc, randomSpawnPosition[i], colorStr);
            PV.RPC("SetUserClue", RpcTarget.AllBuffered, randomCluePosition, player.NickName, code, colorStr);
            PV.RPC("SetClueNote", RpcTarget.All, player.NickName, colorStr, i);
            PV.RPC("SetUserSelect", RpcTarget.All, player.NickName, colorStr, i, code);

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
        Transform userInfo = UIManager.Instance.UserInfo.GetChild(_index);

        userInfo.GetChild(0).GetComponent<Text>().text = _nickname;

        Image noteImage = userInfo.GetChild(2).GetChild(0).GetComponent<Image>();
        SpriteLibraryAsset sprites = userInfo.GetChild(2).GetChild(0).GetComponent<SpriteLibrary>().spriteLibraryAsset;
        noteImage.sprite = sprites.GetSprite("Color", _color);
        
        PlaySceneManager.CheckReady(PlayManager.GameSettings.READY_CLUENOTE);
    }

    public void SetPlayerSettingDone()
    {
        AddCustomPropertiesToLocal(StaticCodes.PHOTON_PROP_SYNC, true);
    }

    private void CheckStart()
    {
        foreach (Player player in PhotonNetwork.CurrentRoom.Players.Values)
        {
            // Check if player is ready
            if (player.CustomProperties.TryGetValue(StaticCodes.PHOTON_PROP_SYNC, out object isDone))
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
    public void SetUserSelect(string _nickname, string _color, int _index, string _code)
    {
        // 콜록 고발할 때 -> 몇 번째에 누가 있는지를 네트워크 매니저에서 그냥 박아 둠.
        Transform homesInfo = UIManager.Instance.HomesInfo.GetChild(_index);
        homesInfo.GetChild(0).GetChild(0).GetComponent<Text>().text = _nickname;
        homesInfo.GetChild(0).GetChild(1).GetComponent<Text>().text = _code;

        Image userImage = homesInfo.GetChild(1).GetChild(0).GetComponent<Image>();
        SpriteLibraryAsset sprites = homesInfo.GetChild(1).GetChild(0).GetComponent<SpriteLibrary>().spriteLibraryAsset;
        userImage.sprite = sprites.GetSprite("Color", _color);
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
    public void AddCustomPropertiesToLocal(string _key, object _value)
    {
        var hash = PhotonNetwork.LocalPlayer.CustomProperties;
        if (hash.ContainsKey(_key))
        {
            hash[_key] = _value;
        }
        else
        {
            hash.Add(_key, _value);
        }
        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
    }
    #endregion
}
