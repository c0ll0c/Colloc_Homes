using Photon.Realtime;
using TMPro;
using UnityEngine;

public class HandleRoomList : MonoBehaviour
{
    public TextMeshProUGUI RoomName;
    public RoomInfo RoomInfo { get; private set; }

    public void SetRoomInfo(RoomInfo _roomInfo)
    {
        //RoomName.text = _roomInfo.CustomProperties["RoomName"].ToString();
        RoomName.text = _roomInfo.Name;
        RoomInfo = _roomInfo;
    }

    public void JoinRoom()
    {
        NetworkManager.Instance.JoinRoom(RoomInfo.Name);
    }
}
