using Photon.Realtime;
using TMPro;
using UnityEngine;

public class HandleRoomList : MonoBehaviour
{
    public TextMeshProUGUI RoomName;
    public TextMeshProUGUI RoomPlayers;
    public RoomInfo RoomInfo { get; private set; }
    public TMP_InputField CodeField;

    public void SetRoomInfo(RoomInfo _roomInfo)
    {
        //RoomName.text = _roomInfo.CustomProperties["RoomName"].ToString();
        RoomName.text = _roomInfo.Name;
        RoomPlayers.text = _roomInfo.PlayerCount.ToString() + "/" + _roomInfo.MaxPlayers.ToString();
        RoomInfo = _roomInfo;
        if ((bool)RoomInfo.CustomProperties["Private"])
        {
            transform.GetChild(1).gameObject.SetActive(true);
        }
    }

    public void JoinRoom()
    {
        if ((bool)RoomInfo.CustomProperties["Private"])
        {
            transform.GetChild(5).gameObject.SetActive(true);
        }
        else
        {
            NetworkManager.Instance.JoinRoom(RoomInfo.Name);
        }
    }

    public void CheckCode()
    {
        if (RoomInfo.Name == CodeField.text)
        {
            NetworkManager.Instance.JoinRoom(RoomInfo.Name);
        }
    }
}
