using Photon.Pun;
using UnityEngine;

public enum PlayBtnType
{
    BACK_TO_LOBBY,
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
        }
    }
}
