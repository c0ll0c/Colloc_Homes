using Photon.Realtime;
using TMPro;
using UnityEngine;

public class HandleRoomList : MonoBehaviour
{
    public TMPro.TextMeshProUGUI RoomName;
    public RoomInfo RoomInfo { get; private set; }

    public void SetRoomInfo(RoomInfo roomInfo)
    {
        RoomName.text = roomInfo.Name;
        RoomInfo = roomInfo;
    }

    public void JoinRoom()
    {
        NetworkManager.Instance.JoinRoom(RoomName.text);
    }
}
