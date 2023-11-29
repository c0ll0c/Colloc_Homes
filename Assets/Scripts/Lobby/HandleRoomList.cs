using Photon.Realtime;
using TMPro;
using UnityEngine;

public class HandleRoomList : MonoBehaviour
{
    public TMPro.TextMeshProUGUI RoomName;
    public RoomInfo RoomInfo { get; private set; }

    public void SetRoomInfo(RoomInfo _roomInfo)
    {
        RoomName.text = _roomInfo.Name;
        RoomInfo = _roomInfo;
    }

    public void JoinRoom()
    {
        NetworkManager.Instance.JoinRoom(RoomName.text);
    }
}
