using UnityEngine;

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
}

public class LobbyBtnsOnClick : MonoBehaviour
{
    public LobbyBtnType BtnType;
    public void OnClickBtn()
    {
        switch (BtnType)
        {
            case LobbyBtnType.CREATE:
                transform.parent.GetComponent<CreateRoomOptionsUI>().CreateRoom(); break;
            case LobbyBtnType.JOIN:
                transform.parent.GetComponent<HandleRoomList>().JoinRoom(); break;
            case LobbyBtnType.BACK:
                GameManager.Instance.ChangeScene(GameState.INTRO); break;
            case LobbyBtnType.RANDOM_JOIN:

                break;
            case LobbyBtnType.SET_ROOM:
                transform.parent.GetChild(1).gameObject.SetActive(true);
                break;
            case LobbyBtnType.SET_NAME:
                transform.parent.GetChild(1).gameObject.SetActive(true);
                break;
            case LobbyBtnType.SAVE_NAME:
                transform.parent.GetComponent<NicknameInputFieldUI>().SaveNickname();
                transform.parent.gameObject.SetActive(false);
                break;
            case LobbyBtnType.CLOSE_PANEL:
                transform.parent.gameObject.SetActive(false);
                break;
        }
    }
}
