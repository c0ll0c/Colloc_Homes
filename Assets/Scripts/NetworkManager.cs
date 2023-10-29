using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using System.Collections;


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
        PhotonNetwork.JoinOrCreateRoom("게임룸", roomOptions, TypedLobby.Default);
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

    [PunRPC]
    public void SyncHiddenCode(bool IsHidden, bool state)
    {
        IsHidden = state;
        StartCoroutine(UnHiddenClue());
    }

    IEnumerator UnHiddenClue()
    {
        yield return new WaitForSeconds(15.0f);
        // IsHidden = false;
    }
}
