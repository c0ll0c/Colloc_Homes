using TMPro;
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
    public TMP_Text BtnText;

    private bool isReady = false;
    public void OnClickBtn()
    {
        switch (BtnType)
        {
            case ReadyBtnType.READY:
                bool _isReady = NetworkManager.Instance.SetPlayerReady(!isReady);
                isReady = _isReady;
                BtnText.text = (isReady) ? "준비 해제" : "준비";
                NetworkManager.Instance.ReadySceneManager.DisactivateColorToggle(_isReady);
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
