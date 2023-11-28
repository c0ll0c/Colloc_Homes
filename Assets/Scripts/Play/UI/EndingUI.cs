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
        "�ڳ�, ���� ���ΰ�?",
        "�̰̱�.",
        "��, ���� ���� ����! �̵� �� ���� �ݷ��ΰ�?",
        "... ���� �� ...",
        "�ڳ״� Ʋ�Ⱦ�. ������ �� �� ���� �ɼ�.",
        "��, ����! ��Ӵ�� ������� ����. �����߳�!"
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

        for (int i = 0; i < NetworkManager._currentPlayer; i++)             // ���� ����ִ� �ε����� ���ְ� 
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

    public void OnClickFound()          // ã�Ҿ�� ����
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

    public void OnClickUserButton()     // �����
    {
        GameObject clickedButton = EventSystem.current.currentSelectedGameObject;
        Debug.Log("Ŭ���� ��ư: " + clickedButton.name);

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
