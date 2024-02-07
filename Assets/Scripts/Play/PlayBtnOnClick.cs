using Photon.Pun;
using UnityEngine;

public enum PlayBtnType
{
    BACK_TO_LOBBY,
    SPECTATOR_MODE,
    CLOSE_PANEL,
    ATTACK,
    INFECT,
}

public class PlayBtnOnClick : MonoBehaviour
{
    public PlayBtnType BtnType;

    public void OnClickBtn()
    {
        switch (BtnType)
        {
            case PlayBtnType.BACK_TO_LOBBY:
                Time.timeScale = 1;
                PhotonNetwork.LeaveRoom();
                break;
            case PlayBtnType.SPECTATOR_MODE:
                Time.timeScale = 1;
                break;
            case PlayBtnType.CLOSE_PANEL:
                transform.parent.gameObject.SetActive(false);
                break;
            case PlayBtnType.ATTACK:

                break;
            case PlayBtnType.INFECT:

                break;
        }
    }
}
