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
        dialogBody[0] = ", ������ ã�� �ǰ�?";
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

    // next ��ư ������ ��, 
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
        // ���� �ε����� �����ֵ�, �� �Ŷ�� �� �� �� --, ���� count�� 0���� ũ�ų� ���ٸ� �����ֱ�
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

    public void OnClickFound()          // ã�Ҿ�� ����
    {
        dialogText.GetComponent<Text>().text = dialogBody[2];
        FirstSelect.SetActive(false);
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
            dialogText.GetComponent<Text>().text = dialogBody[5];               // ����� ���ϰ� ���� 2�� �ڿ� win, lose�� �ߵ���!
            yield return new WaitForSeconds(2f);
            endingManager.ShowResult(EndingType.CatchColloc, true);             // ���� ����... �ٸ� �ֵ��� false�� �������� ��
        }

        else
        {
            dialogText.GetComponent<Text>().text = dialogBody[4];
            yield return new WaitForSeconds(2f);
            endingManager.ShowResult(EndingType.FalseAlarm, true);
        }
    }
}
