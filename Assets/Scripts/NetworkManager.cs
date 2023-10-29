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

    // ���� ������, Ȥ�� �ڷ� ���� ��ư���� Intro Scene���� �ٽ� �̵��� �� �Ҹ���
    // PhotonView�� ���� ���� �� PhotonNetwork ���� ����
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

    // Master Server�� ���� �� lobby ����
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

    // �⺻ ���ӷ�(readyscene) ����
    // [TODO] ���Ŀ� �� ���� / �� ��� ���� �� ������ ���� ����
    public void JoinDefaultRoom()
    {
        PhotonNetwork.NickName = GameManager.Instance.PlayerName;

        RoomOptions roomOptions = new ()
        {
            IsOpen = true,
            IsVisible = true,
            MaxPlayers = maxPlayersPerRoom
        };
        PhotonNetwork.JoinOrCreateRoom("���ӷ�", roomOptions, TypedLobby.Default);
    }
    public override void OnJoinedRoom()
    {
        GameManager.Instance.ChangeScene(GameState.READY);
        PV.RPC("SyncPlayersData", RpcTarget.OthersBuffered);
    }

    // ReadySceneManager ����
    public ReadyManager ReadySceneManager;

    // PlayerData ���� �ø��� ReadyScene(or PlayScene)�� Manager Call
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
