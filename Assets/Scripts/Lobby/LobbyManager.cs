using UnityEngine;
using System.Collections.Generic;
using Photon.Realtime;
using TMPro;

public class LobbyManager : MonoBehaviour
{
    public Transform Content;

    public HandleRoomList RoomListPrefab;

    public PanelTextFieldUI CodeCheckPanel;
    public HandleRoomList SelectedRoom;

    private List<HandleRoomList> roomCollection = new List<HandleRoomList>();
    private int index;

    public TMP_Text UserNicknameText;

    private void Start()
    {
        NetworkManager.Instance.LobbySceneManager = this;
        NetworkManager.Instance.SetupRoomList();

        UserNicknameText.text = GameManager.Instance.PlayerName;
    }

    public void RefreshRoom(List<RoomInfo> _roomInfos)
    {
        roomCollection.Clear(); 
        foreach (RoomInfo roomInfo in _roomInfos)
        {
            HandleRoomList newRoom = Instantiate(RoomListPrefab, Content);
            if (!Equals(newRoom, null))
            {
                newRoom.SetRoomInfo(roomInfo);
                roomCollection.Add(newRoom);
            }
        }
    }

    public void AddRoom(RoomInfo _roomInfo)
    {
        index = roomCollection.FindIndex(x => x.RoomInfo.Name == _roomInfo.Name);
        if (!Equals(index, -1))
        {
            roomCollection[index].SetRoomInfo(_roomInfo);
            return;
        }

        HandleRoomList newRoom = Instantiate(RoomListPrefab, Content);
        if (!Equals(newRoom, null))
        {
            newRoom.SetRoomInfo(_roomInfo);
            roomCollection.Add(newRoom);
        }
    }

    public void RemoveRoom(RoomInfo _roomInfo)
    {
        index = roomCollection.FindIndex(x => x.RoomInfo.Name == _roomInfo.Name);
        if (!Equals(index, -1))
        {
            Destroy(roomCollection[index].gameObject);
            roomCollection.RemoveAt(index);
        }
    }

    private void OnDestroy()
    {
        NetworkManager.Instance.LobbySceneManager = null;
    }
}
