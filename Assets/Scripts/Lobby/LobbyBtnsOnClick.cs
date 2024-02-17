using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public enum LobbyBtnType
{
    CREATE,
    JOIN,
    BACK,
    CLOSE_PANEL,
    SAVE_NAME,
    ENTER,
    CHECK
}

public class LobbyBtnsOnClick : MonoBehaviour
{
    public LobbyBtnType BtnType;
    public Button JoinRoom;
    public void OnClickBtn()
    {
        switch (BtnType)
        {
            case LobbyBtnType.CREATE:
                if (!PhotonNetwork.InLobby) return;
                transform.parent.GetComponent<CreateRoomOptionsUI>().CreateRoom();
                transform.GetComponent<Button>().interactable = false; break;
            case LobbyBtnType.JOIN:
                if (!PhotonNetwork.InLobby) return;
                transform.parent.GetComponent<HandleRoomList>().JoinRoom();
                transform.GetComponent<Button>().interactable = false; 
                break;
            case LobbyBtnType.CHECK:
                transform.parent.parent.GetComponent<HandleRoomList>().CheckCode(); break;
            case LobbyBtnType.BACK:
                GameManager.Instance.ChangeScene(GameState.INTRO); break;
            case LobbyBtnType.SAVE_NAME:
                transform.parent.GetComponent<NicknameInputFieldUI>().SaveNickname();
                transform.parent.gameObject.SetActive(false);
                break;
            case LobbyBtnType.CLOSE_PANEL:
                if (JoinRoom != null) JoinRoom.interactable = true;
                transform.parent.gameObject.SetActive(false);
                break;
            case LobbyBtnType.ENTER:
                transform.parent.GetComponent<CodeInputFieldUI>().FindRoom();
                break;
        }
    }
}
