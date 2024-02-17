using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using System.Collections;
using System;
using UnityEngine.UI;
using UnityEngine.U2D.Animation;
using System.Linq;

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
    public Dictionary<string, RoomInfo> CachedRoomList = new Dictionary<string, RoomInfo>();

    public void SetupRoomList()
    {
        if(LobbySceneManager != null)
        {
            LobbySceneManager.RefreshRoom(CachedRoomList.Values.ToList<RoomInfo>());
        }
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (RoomInfo roomInfo in roomList)
        {
            if (roomInfo.RemovedFromList)
            {
                CachedRoomList.Remove(roomInfo.Name);
                if (LobbySceneManager != null)
                {
                    LobbySceneManager.RemoveRoom(roomInfo);
                }
            }
            else
            {
                CachedRoomList[roomInfo.Name] = roomInfo;
                if (LobbySceneManager != null)
                {
                    LobbySceneManager.AddRoom(roomInfo);
                }
            }
        }
    }

    public void CreateRoom(string _roomName, bool _isPrivate)
    {
        PhotonNetwork.NickName = GameManager.Instance.PlayerName;
        string randomCharacters = "0123456789ABCDEFGHIJYZ";
        string roomCode = StaticFuncs.ShuffleString(randomCharacters).Substring(0, 5);

        ExitGames.Client.Photon.Hashtable roomCustomProps = new ExitGames.Client.Photon.Hashtable();
        roomCustomProps.Add("RoomName", _roomName);
        roomCustomProps.Add("Private", _isPrivate);
        roomCustomProps.Add("State", "waiting");
        roomCustomProps.Add(StaticCodes.PHOTON_PROP_SLOTS, 0b111111);

        string[] customPropsForLobby = { "RoomName", "Private", "State" };

        RoomOptions roomOptions = new()
        {
            IsOpen = true,
            IsVisible = true,
            MaxPlayers = 6,
            CustomRoomProperties = roomCustomProps,
            CustomRoomPropertiesForLobby = customPropsForLobby,
        };

        PhotonNetwork.CreateRoom(roomCode, roomOptions, TypedLobby.Default);
    }

    public void JoinRoom(string _roomName)
    {
        PhotonNetwork.NickName = GameManager.Instance.PlayerName;
        PhotonNetwork.JoinRoom(_roomName);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log(returnCode);
        Debug.Log(message);
        if (returnCode == 32758)
        {
            AlertManager.Instance.WarnAlert("코드를 다시 확인해주세요");
        }
        if (returnCode == 32764)
        {
            AlertManager.Instance.WarnAlert("이미 게임이 시작된 방입니다");
        }
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
                    if (player.CustomProperties.TryGetValue(StaticCodes.PHOTON_PROP_COLOR, out object color))
                    {
                        ableColor |= (int)color;
                    }
                }
            }
        }

        ableColor ^= StaticVars.PLAYER_COLORS;
        int playerColor = ableColor & (-ableColor);

        GameManager.Instance.ChangeScene(GameState.READY);
        AddCustomPropertiesToPlayer(PhotonNetwork.LocalPlayer, StaticCodes.PHOTON_PROP_COLOR, playerColor);
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
            PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue("RoomName", out object roomNm);
            PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue(StaticCodes.PHOTON_PROP_SLOTS, out object slots);

            ReadySceneManager.SetUI((string)roomNm, PhotonNetwork.CurrentRoom.Name, playersStatus, (int)slots);
        }
    }

    // Player Ready
    public bool SetPlayerReady(bool _isReady)
    {
        PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue(StaticCodes.PHOTON_PROP_COLOR, out object localColor);

        // Check other players' color
        if (_isReady)
        {
            foreach (Player player in PhotonNetwork.CurrentRoom.Players.Values)
            {
                if ( !player.IsLocal && (
                        player.IsMasterClient
                        || (player.CustomProperties.TryGetValue(StaticCodes.PHOTON_PROP_ISREADY, out object isReady) && (bool)isReady)
                    ))
                {
                    if (player.CustomProperties.TryGetValue(StaticCodes.PHOTON_PROP_COLOR, out object color))
                    {
                        if ((int)color == (int)localColor)
                        {
                            AlertManager.Instance.ShowAlert("준비 불가", "다른 플레이어와 색상이 겹칩니다.");
                            return false;
                        }
                    }
                }
            }
        }

        AddCustomPropertiesToPlayer(PhotonNetwork.LocalPlayer, StaticCodes.PHOTON_PROP_ISREADY, _isReady);
        return _isReady;
    }

    public void ChangeLocalColor(int _color)
    {
        if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue(StaticCodes.PHOTON_PROP_COLOR, out object color)){
            if ((int)color == _color) return;

            if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue(StaticCodes.PHOTON_PROP_ISREADY, out object isReady))
            {
                if ((bool)isReady) return;
            }

            AddCustomPropertiesToPlayer(PhotonNetwork.LocalPlayer, StaticCodes.PHOTON_PROP_COLOR, _color);
        }
    }

    public void SetSlotAble(int _index, bool _makeAvailable)
    {
        if (!PhotonNetwork.IsMasterClient) return;

        bool res = PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue(StaticCodes.PHOTON_PROP_SLOTS, out object slots);
        if (!res) return;

        int availableSlots = (int)slots;
        if (_makeAvailable)
        {
            PhotonNetwork.CurrentRoom.MaxPlayers++;
            availableSlots |= (1 << _index);
        }
        else if (PhotonNetwork.CurrentRoom.MaxPlayers > 4)
        {
            PhotonNetwork.CurrentRoom.MaxPlayers--;
            availableSlots &= (~(1 << _index));
        }
        else return;

        // Set as Room's custom properties for new players
        var hash = PhotonNetwork.CurrentRoom.CustomProperties;
        hash[StaticCodes.PHOTON_PROP_SLOTS] = availableSlots;
        PhotonNetwork.CurrentRoom.SetCustomProperties(hash);

        // For current players in the room, notify blocked/unblocked playerSlots
        PV.RPC("SyncPlayerSlots", RpcTarget.All, availableSlots);
    }

    [PunRPC]
    public void SyncPlayerSlots(int _slots)
    {
        ReadySceneManager.SetSlotBlock(_slots);
    }

    public void KickPlayerOut(int _playerIndex)
    {
        if (!PhotonNetwork.IsMasterClient) return;

        int playerId = ReadySceneManager.GetPlayerId(_playerIndex);
        if (playerId == -1) return;

        Player player = PhotonNetwork.CurrentRoom.GetPlayer(playerId);

        AddCustomPropertiesToPlayer(player, StaticCodes.PHOTON_PROP_KICKED, true);
    }
    
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        if (changedProps.ContainsKey(StaticCodes.PHOTON_PROP_ISREADY) || changedProps.ContainsKey(StaticCodes.PHOTON_PROP_COLOR))
        {
            SyncPlayersData();
        }

        if (changedProps.TryGetValue(StaticCodes.PHOTON_PROP_SYNC, out object isSynced))
        {
            if (isSynced != null)
            {
                if ((bool)isSynced && PhotonNetwork.IsMasterClient) CheckStart();
            }
        }

        if (changedProps.TryGetValue(StaticCodes.PHOTON_PROP_KICKED, out object isKicked))
        {
            if (isKicked != null)
            {
                if ((bool)isKicked && targetPlayer.IsLocal)
                {
                    LeaveRoom();
                    AlertManager.Instance.ShowAlert("퇴장 알림", "방장으로부터 강제 퇴장당하였습니다.");
                }
            }
        }
    }

    // Start Game by Master
    public void StartGame()
    {
        PhotonNetwork.MasterClient.CustomProperties.TryGetValue(StaticCodes.PHOTON_PROP_COLOR, out object masterColor);

        foreach (Player player in PhotonNetwork.CurrentRoom.Players.Values)
        {
            if (!player.IsMasterClient)
            {
                // Check if player is ready
                if (player.CustomProperties.TryGetValue(StaticCodes.PHOTON_PROP_ISREADY, out object IsReady))
                {
                    if (!(bool)IsReady)
                    {
                        AlertManager.Instance.ShowAlert("시작 불가", "아직 준비가 안 된 플레이어들이 있습니다.");
                        return;
                    }
                }
                else
                {
                    AlertManager.Instance.ShowAlert("시작 불가", "아직 준비가 안 된 플레이어들이 있습니다.");
                    return;
                }
                
                if (player.CustomProperties.TryGetValue(StaticCodes.PHOTON_PROP_COLOR, out object color))
                {
                    if ((int)color == (int)masterColor)
                    {
                        AlertManager.Instance.ShowAlert("시작 불가", "다른 플레이어와 색상이 겹칩니다.");
                        return;
                    }
                }
            }
        }

        PhotonNetwork.LoadLevel("PlayScene");

        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.CurrentRoom.IsVisible = false;

        foreach (Player player in PhotonNetwork.CurrentRoom.Players.Values)
        {
            AddCustomPropertiesToPlayer(player, StaticCodes.PHOTON_PROP_ISREADY, false);
        }
    }

    // when player leave room
    public void LeaveRoom()
    {
        // CustomProperties Clear
        ExitGames.Client.Photon.Hashtable hash = PhotonNetwork.LocalPlayer.CustomProperties;
        hash[StaticCodes.PHOTON_PROP_COLOR] = null;
        hash[StaticCodes.PHOTON_PROP_ISREADY] = null;
        hash[StaticCodes.PHOTON_PROP_KICKED] = null;
        hash[StaticCodes.PHOTON_PROP_SYNC] = null;
        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);

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
        if (PlaySceneManager != null)
        {
            if (PhotonNetwork.PlayerList.Length == 1)
            {
                EndingManager.ShowResult(EndingType.Error, true);
            }
        }
    }
    #endregion
    #region SET PLAY SCENE
    // PlaySceneManager
    public PlayManager PlaySceneManager;
    public EndingManager EndingManager;

    // Generate players, codes, items(drop time, position): called by MasterClient
    public void GameSetting()
    {
        int i = 0;
        int colloc = UnityEngine.Random.Range(0, PhotonNetwork.CurrentRoom.PlayerCount);
        string code;
        Vector2[] randomCluePosition = ShufflePosition(StaticVars.CluePosition);
        Vector2[] randomSpawnPosition = ShufflePosition(StaticVars.SpawnPosition);

        string commonCharacters = "0123456789ABCDEX";

        commonCharacters = StaticFuncs.ShuffleString(commonCharacters).Substring(0, 3);          // 랜덤으로 세 글자

        foreach (Player player in PhotonNetwork.CurrentRoom.Players.Values)
        {
            bool isColloc = false;
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

    // Set where item will drop
    private Vector3 SetDropPos()
    {
        Vector3 randomDropPos = new Vector3(UnityEngine.Random.Range(0f, 6f), UnityEngine.Random.Range(-8f, 17f), 0f);
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
        AddCustomPropertiesToPlayer(PhotonNetwork.LocalPlayer, StaticCodes.PHOTON_PROP_SYNC, true);
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

        foreach (Player player in PhotonNetwork.CurrentRoom.Players.Values)
        {
            AddCustomPropertiesToPlayer(player, StaticCodes.PHOTON_PROP_SYNC, false);
        }
    }

    [PunRPC]
    public void SetGameStart(double _endTime)
    {
        if (PlaySceneManager == null) return;
        PlaySceneManager.StartGame(_endTime);
    }

    [PunRPC]
    public void SetUserSelect(string _nickname, string _color, int _index, string _code)
    {
        Transform homesInfo = UIManager.Instance.HomesInfo.GetChild(_index);
        homesInfo.GetChild(2).GetChild(0).GetComponent<Text>().text = _nickname;
        homesInfo.GetChild(2).GetChild(1).GetComponent<Text>().text = _code;

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
        yield return new WaitForSeconds(StaticVars.Hidden_TIME);
        clue.IsHidden = false;
    }


    [PunRPC]
    public void ShowResultRPC(EndingType _endType, bool invoker)
    {
        Debug.Log(_endType);
        Debug.Log(invoker);
        EndingManager.ShowResult(_endType, invoker);
    }

    [PunRPC]
    IEnumerator OutRPC()
    {
        UIManager.Instance.OutPanelObj.SetActive(true);
        yield return new WaitForSeconds(2.0f);
        UIManager.Instance.OutPanelObj.SetActive(false);
    }
    #endregion
    #region SERVER UTILS
    public double GetServerTime()
    {
        return PhotonNetwork.Time;
    }
    public void AddCustomPropertiesToPlayer(Player _player, string _key, object _value)
    {
        ExitGames.Client.Photon.Hashtable hash = _player.CustomProperties;
        hash[_key] = _value;
        _player.SetCustomProperties(hash);
    }
    #endregion
}
