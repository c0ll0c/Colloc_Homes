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
        "자네, 무슨 일인가?",
        "싱겁긴.",
        "오, 빨리 말해 보게! 이들 중 누가 콜록인가?",
        "... 수사 중 ...",
        "자네는 틀렸어. 앞으로 볼 일 없을 걸세.",
        "오, 역시! 약속대로 현상금을 주지. 수고했네!"
    };
    public GameObject FirstSelect;
    public GameObject SecondSelect;
    public Text HomesName;
    public Image HomesImage;
    public GameObject DoneButton;
    public GameObject[] UserButton;

    private void OnEnable()
    {
        SetupDialog();
    }

    private void SetupDialog()
    {
        HomesName.text = GameManager.Instance.PlayerName;
        dialogText.transform.GetChild(1).GetComponent<Text>().text = dialogBody[0];
        FirstSelect.SetActive(true);
        SecondSelect.SetActive(false);
        DoneButton.SetActive(false);

        for (int i = 0; i < NetworkManager._currentPlayer; i++)             // 내가 들어있는 인덱스를 없애고 
        {
            print(GameManager.Instance.PlayerName);
            print(UIManager.Instance.HomesInfo.GetChild(i).GetChild(0).GetComponent<Text>().text);
            if (UIManager.Instance.HomesInfo.GetChild(i).GetChild(0).GetComponent<Text>().text == GameManager.Instance.PlayerName)
            {
                HomesImage.sprite = UIManager.Instance.HomesInfo.GetChild(i).GetChild(1).GetComponent<Image>().sprite;
                for (int j = i; j < UIManager.Instance.HomesInfo.childCount - 1; j++)
                {
                    Transform currentElement = UIManager.Instance.HomesInfo.GetChild(j);
                    Transform nextElement = UIManager.Instance.HomesInfo.GetChild(j + 1);

                    currentElement.GetChild(0).GetComponent<Text>().text = nextElement.GetChild(0).GetComponent<Text>().text;
                    currentElement.GetChild(1).GetComponent<Image>().sprite = nextElement.GetChild(1).GetComponent<Image>().sprite;
                }
            }
        }
    }
    
    public void onClickNo()
    {
        dialogText.transform.GetChild(1).GetComponent<Text>().text = dialogBody[1];
        FirstSelect.SetActive(false);
        DoneButton.SetActive(true);
    }

    public void OnClickDone()
    {
        gameObject.SetActive(false);
    }

    public void OnClickFound()          // 찾았어요 누름
    {
        dialogText.transform.GetChild(1).GetComponent<Text>().text = dialogBody[2];
        FirstSelect.SetActive(false);

        print(NetworkManager._currentPlayer);
        for (int i = 0; i < NetworkManager._currentPlayer - 1; i++)
        {
            SecondSelect.transform.GetChild(0).GetChild(i).gameObject.SetActive(true);
        }

        SecondSelect.SetActive(true);
    }

    public void OnClickUserButton()     // 고발함
    {
        GameObject clickedButton = EventSystem.current.currentSelectedGameObject;
        Debug.Log("클릭된 버튼: " + clickedButton.name);

        dialogText.transform.GetChild(1).GetComponent<Text>().text = dialogBody[3];
        SecondSelect.SetActive(false);

        StartCoroutine(ShowResult(clickedButton.transform.GetChild(0).GetComponent<Text>().text));
    }

    public IEnumerator ShowResult(string _name)
    {
        yield return new WaitForSeconds(2f);

        if (_name == PhotonNetwork.CurrentRoom.CustomProperties["CollocName"].ToString())
        {
            dialogText.transform.GetChild(1).GetComponent<Text>().text = dialogBody[5];
        }

        else
        {
            dialogText.transform.GetChild(1).GetComponent<Text>().text = dialogBody[4];

        }
    }
}
