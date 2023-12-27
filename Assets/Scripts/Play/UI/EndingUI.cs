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
    public GameObject EndingCanvasObj;
    private EndingManager endingManager;

    int count = 0;

    private void Start()
    {
        endingManager = EndingCanvasObj.GetComponent<EndingManager>();
    }

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

        for (int i = 0; i < NetworkManager._currentPlayer; i++)
        {
            Debug.Log(UIManager.Instance.HomesInfo.GetChild(i).GetChild(0).GetChild(0).GetComponent<Text>().text);
            if (UIManager.Instance.HomesInfo.GetChild(i).GetChild(0).GetChild(0).GetComponent<Text>().text == GameManager.Instance.PlayerName)
            {
                HomesImage.sprite = UIManager.Instance.HomesInfo.GetChild(i).GetChild(1).GetChild(0).GetComponent<Image>().sprite;
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
            FirstSelect.SetActive(true);
        }
        else if (dialogText.GetComponent<Text>().text == dialogBody[1])
        {
            gameObject.SetActive(false);
        }
        else if (dialogText.GetComponent<Text>().text == dialogBody[2])
        {
            SecondSelect.SetActive(true);
            for (int i = 0; i < 6; i++)
                UIManager.Instance.HomesInfo.GetChild(i).gameObject.SetActive(false);
            if (UIManager.Instance.HomesInfo.GetChild(0).GetChild(0).GetChild(0).GetComponent<Text>().text != GameManager.Instance.PlayerName)
                UIManager.Instance.HomesInfo.GetChild(0).gameObject.SetActive(true);
            else
                UIManager.Instance.HomesInfo.GetChild(1).gameObject.SetActive(true);
        }
    }

    public void OnClickLeft()
    {
        // 이전 인덱스를 보여주되, 내 거라면 한 번 더 --, 이후 count가 0보다 크거나 같다면 보여주기
        if (count != 0)
        {
            count--;
            
            if (UIManager.Instance.HomesInfo.GetChild(count).GetChild(0).GetChild(0).GetComponent<Text>().text == GameManager.Instance.PlayerName)
                count--;

            if (count >= 0)
            {
                for (int i = 0; i < 6; i++)
                    UIManager.Instance.HomesInfo.GetChild(i).gameObject.SetActive(false);
                UIManager.Instance.HomesInfo.GetChild(count).gameObject.SetActive(true);
            }
        }
    }

    public void OnClickRight()
    {
        if (count != NetworkManager._currentPlayer - 1)
        {
            count++;

            if (UIManager.Instance.HomesInfo.GetChild(count).GetChild(0).GetChild(0).GetComponent<Text>().text == GameManager.Instance.PlayerName)
                count++;

            if (count < NetworkManager._currentPlayer)
            {
                for (int i = 0; i < 6; i++)
                    UIManager.Instance.HomesInfo.GetChild(i).gameObject.SetActive(false);
                UIManager.Instance.HomesInfo.GetChild(count).gameObject.SetActive(true);
            }
        }
    }

    public void OnClickFound()          // 찾았어요 누름
    {
        dialogText.GetComponent<Text>().text = dialogBody[2];
        FirstSelect.SetActive(false);
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
            dialogText.GetComponent<Text>().text = dialogBody[5];               // 결과를 말하고 나서 2초 뒤에 win, lose가 뜨도록!
            yield return new WaitForSeconds(2f);
            endingManager.ShowResult(EndingType.CatchColloc, true);             // 내가 맞음... 다른 애들은 false가 보내져야 됨
        }

        else
        {
            dialogText.GetComponent<Text>().text = dialogBody[4];
            yield return new WaitForSeconds(2f);
            endingManager.ShowResult(EndingType.FalseAlarm, true);
        }
    }
}
