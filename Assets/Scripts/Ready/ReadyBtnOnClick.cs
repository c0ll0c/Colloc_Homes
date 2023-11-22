using Photon.Pun;
using UnityEngine;

public enum ReadyBtnType
{
    BACK,
    READY,
    START
}

public class ReadyBtnOnClick : MonoBehaviour
{
    public ReadyBtnType BtnType;

    private bool isReady = false;
    public void OnClickBtn()
    {
        switch (BtnType)
        {
            case ReadyBtnType.READY:
                isReady = !isReady;
                NetworkManager.Instance.SetPlayerReady(isReady);
                break;

            case ReadyBtnType.START:
                NetworkManager.Instance.StartGame();
                break;

            case ReadyBtnType.BACK:
                NetworkManager.Instance.LeaveRoom();
                break;
        }
    }

}
