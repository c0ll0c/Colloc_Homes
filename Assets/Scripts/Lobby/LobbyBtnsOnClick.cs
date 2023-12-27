using UnityEngine;

public enum LobbyBtnType
{
    CREATE,
    JOIN,
    RANDOM_JOIN,
    BACK,
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
            case LobbyBtnType.RANDOM_JOIN:

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
