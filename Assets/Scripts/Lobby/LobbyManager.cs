using UnityEngine;
using System.Collections.Generic;
using Photon.Realtime;
using Photon.Pun;
using System.Linq;

public class LobbyManager : MonoBehaviour
{
    public Transform Content;
    public HandleRoomList RoomList;
    private List<HandleRoomList> roomCollection = new List<HandleRoomList>();
    private int index;

    private void Start()
    {
        NetworkManager.Instance.LobbySceneManager = this;
        NetworkManager.Instance.SetupRoomList();
    }

    public void RefreshRoom(List<RoomInfo> _roomInfos)
    {
        roomCollection.Clear(); 
        foreach (RoomInfo roomInfo in _roomInfos)
        {
            HandleRoomList newRoom = Instantiate(RoomList, Content);
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

        HandleRoomList newRoom = Instantiate(RoomList, Content);
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
