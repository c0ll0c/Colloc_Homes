using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public enum LobbyBtnType
{
    CREATE,
    BACK,
    CHANGE_NAME,
    CODE_JOIN
}

public class LobbyBtnsOnClick : MonoBehaviour
{
    public LobbyBtnType BtnType;
    public GameObject ActivePanel;
    private PanelTextFieldUI panelUI;

    public void Start()
    {
        if (ActivePanel != null)
        {
            panelUI = ActivePanel.GetComponent<PanelTextFieldUI>();
        }
    }

    public void OnClickBtn()
    {
        switch (BtnType)
        {
            case LobbyBtnType.CREATE:
                if (!PhotonNetwork.InLobby) return;
                transform.parent.GetComponent<CreateRoomOptionsUI>().CreateRoom();
                transform.GetComponent<Button>().interactable = false; break;
            case LobbyBtnType.BACK:
                GameManager.Instance.ChangeScene(GameState.INTRO); break;
            case LobbyBtnType.CHANGE_NAME:
                if (panelUI == null) return;
                panelUI.SetTextAndOpen(GameManager.Instance.PlayerName);
                panelUI.SaveBtn.onClick.AddListener(() =>
                {
                    string changedName = panelUI.ReturnTextAndClose();
                    if (string.IsNullOrEmpty(changedName)) return;
                    PlayerPrefs.SetString(StaticVars.PREFS_NICKNAE, changedName);
                    GameManager.Instance.PlayerName = changedName;
                    NetworkManager.Instance.LobbySceneManager.UserNicknameText.text = changedName;
                });
                break;
            case LobbyBtnType.CODE_JOIN:
                if (panelUI == null) return;
                panelUI.SetTextAndOpen(string.Empty);
                panelUI.SaveBtn.onClick.AddListener(() =>
                {
                    string roomCode = panelUI.ReturnTextAndClose();
                    if (string.IsNullOrEmpty(roomCode)) return;
                    NetworkManager.Instance.JoinRoom(roomCode.ToUpper());
                });
                break;
        }
    }
}
