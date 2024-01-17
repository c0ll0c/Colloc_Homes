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

    // Intro Scene���� ���� ��ư ���� �� Loading Text Ȱ��ȭ ��Ű��
    // ��ư�� ��� Ŭ���� ���ϵ��� ���´�.
    public void StartGame()
    {
        NetworkManager.Instance.Connect();
        transform.GetComponent<Button>().interactable = false;
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(true);
        if (string.IsNullOrEmpty(GameManager.Instance.PlayerName)){
            int rand = Random.Range(10000, 100000);
            PlayerPrefs.SetString(StaticVars.PREFS_NICKNAE, "P#"+rand.ToString());
            GameManager.Instance.PlayerName = "P#" + rand.ToString();
        }
    }
}