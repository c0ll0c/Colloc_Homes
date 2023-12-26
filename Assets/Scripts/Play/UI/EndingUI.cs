using ExitGames.Client.Photon;
using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EndingUI : MonoBehaviour
{
    public GameObject dialogText;
    private string[] dialogBody = {
        ", 범인을 찾은 건가?",
        "싱겁긴.",
        "오, 빨리 말해 보게! 이들 중 누가 콜록인가?",
        "... 수사 중 ...",
        "자네는 틀렸어. 앞으로 볼 일 없을 걸세.",
        "오, 역시! 약속대로 현상금을 주지. 수고했네!"
    };
    public GameObject FirstSelect;
    public GameObject SecondSelect;
    public string HomesName;
    public Image HomesImage;
    public GameObject[] UserButton;

    private void OnEnable()
    {
        SetupDialog();
    }

    private void SetupDialog()
    {
        HomesName = GameManager.Instance.PlayerName;
        dialogBody[0] = ", 범인을 찾은 건가?";
        dialogBody[0] = HomesName + dialogBody[0];
        dialogText.GetComponent<Text>().text = dialogBody[0];
        FirstSelect.SetActive(false);
        SecondSelect.SetActive(false);

        for (int i = 0; i < NetworkManager._currentPlayer; i++)             // 내가 들어있는 인덱스는 화면에 띄워 주지 마셈
        {
            Debug.Log("닉네임 확인 중");
            Debug.Log(UIManager.Instance.HomesInfo.GetChild(i).GetChild(0).GetChild(0).GetComponent<Text>().text);
            if (UIManager.Instance.HomesInfo.GetChild(i).GetChild(0).GetChild(0).GetComponent<Text>().text == GameManager.Instance.PlayerName)
            {
                Debug.Log("내 거");
                HomesImage.sprite = UIManager.Instance.HomesInfo.GetChild(i).GetChild(1).GetChild(0).GetComponent<Image>().sprite;
                // UIManager.Instance.HomesInfo.GetChild(i).gameObject.SetActive(false);
            }
        }
    }

    public void onClickNo()
    {
        dialogText.GetComponent<Text>().text = dialogBody[1];
        FirstSelect.SetActive(false);
    }

    // next 버튼 눌럿을 때, 
    public void OnClickNextButton()
    {
        if (dialogText.GetComponent<Text>().text == dialogBody[0])
        {
            Debug.Log("범인을 찾앗는지 아닌지...");
            FirstSelect.SetActive(true);
        }
        else if (dialogText.GetComponent<Text>().text == dialogBody[1])
        {
            Debug.Log("취소하고 나가기");
            gameObject.SetActive(false);
        }
        else if (dialogText.GetComponent<Text>().text == dialogBody[2])
        {
            Debug.Log("범인 고르는 중");
            SecondSelect.SetActive(true);
        }
    }

    public void OnClickFound()          // 찾았어요 누름
    {
        dialogText.GetComponent<Text>().text = dialogBody[2];
        FirstSelect.SetActive(false);

        print(NetworkManager._currentPlayer);
        for (int i = 0; i < NetworkManager._currentPlayer - 1; i++)
        {
            SecondSelect.transform.GetChild(0).GetChild(i).gameObject.SetActive(true);
        }
    }

    public void OnClickUserButton()     // 고발함
    {
        GameObject clickedButton = EventSystem.current.currentSelectedGameObject;
        Debug.Log("클릭된 버튼: " + clickedButton.name);

        dialogText.GetComponent<Text>().text = dialogBody[3];
        SecondSelect.SetActive(false);

        StartCoroutine(ShowResult(clickedButton.transform.GetChild(0).GetComponent<Text>().text));
    }

    // TODO: win, lose로 넘어가게
    public IEnumerator ShowResult(string _name)
    {
        yield return new WaitForSeconds(2f);

        if (_name == PhotonNetwork.CurrentRoom.CustomProperties["CollocName"].ToString())
        {
            dialogText.GetComponent<Text>().text = dialogBody[5];
        }

        else
        {
            dialogText.GetComponent<Text>().text = dialogBody[4];

        }
    }
}
