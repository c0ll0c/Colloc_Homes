using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HandleRoomList : MonoBehaviour
{
    public TextMeshProUGUI RoomName;
    public TextMeshProUGUI RoomPlayers;
    public RoomInfo RoomInfo { get; private set; }
    private HandleCodeCheck roomCodeCheckUI;

    private void Start()
    {
        roomCodeCheckUI = NetworkManager.Instance.LobbySceneManager.RoomCodeCheckUI.GetComponent<HandleCodeCheck>();
    }

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
            roomCodeCheckUI.RoomButton = this.gameObject;
            roomCodeCheckUI.gameObject.SetActive(true);
        }
        else
        {
            NetworkManager.Instance.JoinRoom(RoomInfo.Name);
            if (RoomInfo.IsOpen)
            {
                transform.GetComponentInChildren<Button>().interactable = false;
            }
        }
    }

    public void CheckCode(string _codefield)
    {
        if (Equals(string.Compare(RoomInfo.Name, _codefield, true), 0))
        {
            NetworkManager.Instance.JoinRoom(RoomInfo.Name);
        }
        else
        {
            AlertManager.Instance.WarnAlert("코드를 다시 확인해주세요");
        }
    }

    private void OnDestroy()
    {
        if (Equals(roomCodeCheckUI.RoomButton, this.gameObject)) {
            roomCodeCheckUI.gameObject.SetActive(false);
        }
    }
}
