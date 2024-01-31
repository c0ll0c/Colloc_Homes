using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public enum LobbyBtnType
{
    CREATE,
    JOIN,
    RANDOM_JOIN,
    BACK,
    SET_ROOM,
    CLOSE_PANEL,
    SET_NAME,
    SAVE_NAME,
    FIND,
    ENTER,
    CHECK,
    SETTINGS,
}

public class LobbyBtnsOnClick : MonoBehaviour
{
    public LobbyBtnType BtnType;
    public void OnClickBtn()
    {
        switch (BtnType)
        {
            case LobbyBtnType.CREATE:
                transform.parent.GetComponent<CreateRoomOptionsUI>().CreateRoom();
                transform.GetComponent<Button>().interactable = false; break;
            case LobbyBtnType.JOIN:
                transform.parent.GetComponent<HandleRoomList>().JoinRoom();
                transform.GetComponent<Button>().interactable = false; break;
            case LobbyBtnType.CHECK:
                transform.parent.parent.GetComponent<HandleRoomList>().CheckCode(); break;
            case LobbyBtnType.BACK:
                GameManager.Instance.ChangeScene(GameState.INTRO); break;
            case LobbyBtnType.RANDOM_JOIN:
                PhotonNetwork.JoinRandomRoom();
                break;
            case LobbyBtnType.SET_ROOM:
                transform.parent.GetChild(9).gameObject.SetActive(true);
                break;
            case LobbyBtnType.SET_NAME:
                transform.parent.GetChild(7).gameObject.SetActive(true);
                break;
            case LobbyBtnType.SAVE_NAME:
                transform.parent.GetComponent<NicknameInputFieldUI>().SaveNickname();
                transform.parent.gameObject.SetActive(false);
                break;
            case LobbyBtnType.CLOSE_PANEL:
                transform.parent.gameObject.SetActive(false);
                break;
            case LobbyBtnType.FIND:
                transform.parent.GetChild(8).gameObject.SetActive(true);
                break;
            case LobbyBtnType.ENTER:
                transform.parent.GetComponent<CodeInputFieldUI>().FindRoom();
                break;
            case LobbyBtnType.SETTINGS:
                AudioManager.Instance.OpenAudioSettingPanel();
                break;
        }
    }
}
