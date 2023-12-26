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
        ", ������ ã�� �ǰ�?",
        "�̰̱�.",
        "��, ���� ���� ����! �̵� �� ���� �ݷ��ΰ�?",
        "... ���� �� ...",
        "�ڳ״� Ʋ�Ⱦ�. ������ �� �� ���� �ɼ�.",
        "��, ����! ��Ӵ�� ������� ����. �����߳�!"
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
        dialogBody[0] = ", ������ ã�� �ǰ�?";
        dialogBody[0] = HomesName + dialogBody[0];
        dialogText.GetComponent<Text>().text = dialogBody[0];
        FirstSelect.SetActive(false);
        SecondSelect.SetActive(false);

        for (int i = 0; i < NetworkManager._currentPlayer; i++)             // ���� ����ִ� �ε����� ȭ�鿡 ��� ���� ����
        {
            Debug.Log("�г��� Ȯ�� ��");
            Debug.Log(UIManager.Instance.HomesInfo.GetChild(i).GetChild(0).GetChild(0).GetComponent<Text>().text);
            if (UIManager.Instance.HomesInfo.GetChild(i).GetChild(0).GetChild(0).GetComponent<Text>().text == GameManager.Instance.PlayerName)
            {
                Debug.Log("�� ��");
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

    // next ��ư ������ ��, 
    public void OnClickNextButton()
    {
        if (dialogText.GetComponent<Text>().text == dialogBody[0])
        {
            Debug.Log("������ ã�Ѵ��� �ƴ���...");
            FirstSelect.SetActive(true);
        }
        else if (dialogText.GetComponent<Text>().text == dialogBody[1])
        {
            Debug.Log("����ϰ� ������");
            gameObject.SetActive(false);
        }
        else if (dialogText.GetComponent<Text>().text == dialogBody[2])
        {
            Debug.Log("���� ���� ��");
            SecondSelect.SetActive(true);
        }
    }

    public void OnClickFound()          // ã�Ҿ�� ����
    {
        dialogText.GetComponent<Text>().text = dialogBody[2];
        FirstSelect.SetActive(false);

        print(NetworkManager._currentPlayer);
        for (int i = 0; i < NetworkManager._currentPlayer - 1; i++)
        {
            SecondSelect.transform.GetChild(0).GetChild(i).gameObject.SetActive(true);
        }
    }

    public void OnClickUserButton()     // �����
    {
        GameObject clickedButton = EventSystem.current.currentSelectedGameObject;
        Debug.Log("Ŭ���� ��ư: " + clickedButton.name);

        dialogText.GetComponent<Text>().text = dialogBody[3];
        SecondSelect.SetActive(false);

        StartCoroutine(ShowResult(clickedButton.transform.GetChild(0).GetComponent<Text>().text));
    }

    // TODO: win, lose�� �Ѿ��
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
