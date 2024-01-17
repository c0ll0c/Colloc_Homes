using Photon.Pun;
using UnityEngine;

public enum PlayBtnType
{
    BACK_TO_LOBBY,
    SPECTATOR_MODE,
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
        }
    }
}
