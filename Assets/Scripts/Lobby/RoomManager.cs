using UnityEngine;
using System.Collections.Generic;
using Photon.Realtime;

public class RoomManager : MonoBehaviour
{
    public Transform Content;
    public HandleRoomList RoomList;
    private List<HandleRoomList> roomCollection = new List<HandleRoomList>();
    private void Start()
    {
        NetworkManager.Instance.LobbySceneManager = this;
    }
    public void AddRoom(RoomInfo _roomInfo)
    {
        int index = roomCollection.FindIndex(x => x.RoomInfo.Name == _roomInfo.Name);
        if (index != -1)
        {
            return;
        }
        HandleRoomList newRoom = Instantiate(RoomList, Content);
        if (newRoom != null)
        {
            newRoom.SetRoomInfo(_roomInfo);
            roomCollection.Add(newRoom);
        }
    }

    public void RemoveRoom(RoomInfo _roomInfo)
    {
        int index = roomCollection.FindIndex(x => x.RoomInfo.Name == _roomInfo.Name);
        if (index != -1)
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
