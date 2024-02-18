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
                break;
            case LobbyBtnType.CHECK:
                transform.parent.GetComponent<HandleCodeCheck>().CheckCode(); break;
            case LobbyBtnType.BACK:
                GameManager.Instance.ChangeScene(GameState.INTRO); break;
            case LobbyBtnType.SAVE_NAME:
                transform.parent.GetComponent<NicknameInputFieldUI>().SaveNickname();
                transform.parent.gameObject.SetActive(false);
                break;
            case LobbyBtnType.CLOSE_PANEL:
                transform.parent.gameObject.SetActive(false);
                break;
            case LobbyBtnType.ENTER:
                transform.parent.GetComponent<CodeInputFieldUI>().FindRoom();
                break;
        }
    }
}
