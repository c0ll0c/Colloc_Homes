using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HandleRoomList : MonoBehaviour
{
    public TextMeshProUGUI RoomName;
    public TextMeshProUGUI RoomPlayers;
    public Button RoomButton;
    public RoomInfo RoomInfo { get; private set; }

    public void SetRoomInfo(RoomInfo _roomInfo)
    {
        RoomName.text = _roomInfo.CustomProperties[StaticCodes.PHOTON_R_RNAME].ToString();
        RoomPlayers.text = _roomInfo.PlayerCount.ToString() + "/" + _roomInfo.MaxPlayers.ToString();
        RoomInfo = _roomInfo;
        if ((bool)RoomInfo.CustomProperties[StaticCodes.PHOTON_R_MODE])
        {
            transform.GetChild(0).GetChild(1).gameObject.SetActive(true);
        }
    }

    bool joinRoom = false;
    public void OnRoomListClick()
    {
        if (!PhotonNetwork.InLobby) return;

        NetworkManager.Instance.LobbySceneManager.SelectedRoom = this;
        if ((bool)RoomInfo.CustomProperties[StaticCodes.PHOTON_R_MODE]) {
            NetworkManager.Instance.LobbySceneManager.CodeCheckPanel.SetTextAndOpen(string.Empty);
            NetworkManager.Instance.LobbySceneManager.CodeCheckPanel.SaveBtn.onClick.AddListener(() =>
            {
                string code = NetworkManager.Instance.LobbySceneManager.CodeCheckPanel.ReturnTextAndClose();
                if (string.IsNullOrEmpty(code)) return;

                if (Equals(string.Compare(RoomInfo.Name, code, true), 0))
                {
                    NetworkManager.Instance.JoinRoom(RoomInfo.Name);
                    joinRoom = true;
                }
                else
                {
                    AlertManager.Instance.WarnAlert("코드를 다시 확인해주세요");
                }
            });
        }
        else
        {
            NetworkManager.Instance.JoinRoom(RoomInfo.Name);
            joinRoom = true;
        }

        if (joinRoom)
        {
            // btn interactable = false
            RoomButton.interactable = false;
        }
    }

    private void OnDestroy()
    {
        if (
            NetworkManager.Instance.LobbySceneManager != null &&
            NetworkManager.Instance.LobbySceneManager.SelectedRoom == this
        )
        {
            NetworkManager.Instance.LobbySceneManager.SelectedRoom = null;
        }
    }
}
