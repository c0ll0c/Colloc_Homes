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
        RoomName.text = _roomInfo.CustomProperties[StaticCodes.PHOTON_R_RNAME].ToString();
        RoomPlayers.text = _roomInfo.PlayerCount.ToString() + "/" + _roomInfo.MaxPlayers.ToString();
        RoomInfo = _roomInfo;
        if ((bool)RoomInfo.CustomProperties[StaticCodes.PHOTON_R_MODE])
        {
            transform.GetChild(1).gameObject.SetActive(true);
        }
    }

    public void JoinRoom()
    {
        if ((bool)RoomInfo.CustomProperties[StaticCodes.PHOTON_R_MODE])
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
        if (Equals(string.Compare(RoomInfo.Name, CodeField.text, false), 0))
        {
            NetworkManager.Instance.JoinRoom(RoomInfo.Name);
        }
        else
        {
            AlertManager.Instance.WarnAlert("코드를 다시 확인해주세요");
        }
    }
}
