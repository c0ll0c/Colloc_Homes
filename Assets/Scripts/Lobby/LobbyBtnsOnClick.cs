using UnityEngine;

public enum LobbyBtnType
{
    CREATE,
    JOIN,
    BACK,
    SAVE_NAME,
    SET_NAME,
    SET_ROOM,
    CLOSE_PANEL
}

public class LobbyBtnsOnClick : MonoBehaviour
{
    public LobbyBtnType BtnType;
    public void OnClickBtn()
    {
        switch (BtnType)
        {
            case LobbyBtnType.CREATE:
                transform.parent.GetComponent<RoomnameInputFieldUI>().CreateRoom(); break;
            case LobbyBtnType.JOIN:
                transform.parent.GetComponent<HandleRoomList>().JoinRoom(); break;
            case LobbyBtnType.BACK:
                GameManager.Instance.ChangeScene(GameState.INTRO); break;
            case LobbyBtnType.SAVE_NAME:
                transform.parent.GetComponent<NicknameInputFieldUI>().SaveNickname(); break;
            case LobbyBtnType.SET_NAME:
                transform.parent.GetChild(1).gameObject.SetActive(true);
                break;
            case LobbyBtnType.SET_ROOM:
                transform.parent.GetChild(1).gameObject.SetActive(true);
                break;
            case LobbyBtnType.CLOSE_PANEL:
                transform.parent.gameObject.SetActive(false);
                break;
        }
    }
}
