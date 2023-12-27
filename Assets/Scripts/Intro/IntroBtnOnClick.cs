using UnityEngine;
using UnityEngine.UI;


public enum IntroBtnType
{
    START,
    SAVE_NAME,
    CLOSE_PANEL
}

public class IntroBtnOnClick : MonoBehaviour
{
    public IntroBtnType BtnType;
    public GameObject StartBtn;

    public void OnClickBtn()
    {
        switch (BtnType)
        {
            case IntroBtnType.START:
                transform.parent.GetChild(2).gameObject.SetActive(true); break;
            case IntroBtnType.SAVE_NAME:
                transform.parent.GetComponent<NicknameInputFieldUI>().SaveNickname();
                transform.parent.gameObject.SetActive(false);
                StartGame();
                break;
            case IntroBtnType.CLOSE_PANEL:
                transform.parent.gameObject.SetActive(false);
                break;
        }
    }

    // Intro Scene에서 시작 버튼 누를 시 Loading Text 활성화 시키고
    // 버튼을 잠시 클릭을 못하도록 막는다.
    public void StartGame()
    {
        NetworkManager.Instance.Connect();
        StartBtn.GetComponent<Button>().interactable = false;
        StartBtn.transform.GetChild(0).gameObject.SetActive(false);
        StartBtn.transform.GetChild(1).gameObject.SetActive(true);
    }
}