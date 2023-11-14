using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoSingleton<UIManager>
{
    public Transform CluePanelCanvas;
    public GameObject UserClueUI;
    public GameObject CollocClueUI;
    public Canvas ClueUI;

    [SerializeField] private GameObject[] gameIcon = new GameObject[4];

    protected override void Awake()
    {
        base.Awake();

        ClueUI.gameObject.SetActive(false);
        for (int i = 0; i < gameIcon.Length; i++)
        {
            gameIcon[i].SetActive(false);
        }
    }

    public void SetGameUI(string _status)
    {
        switch (_status) {
            case "Colloc":
                gameIcon[2].SetActive(true);
                gameIcon[3].SetActive(false);
                break;

            case "Homes":
                gameIcon[0].SetActive(true);
                gameIcon[1].SetActive(false);
                gameIcon[3].SetActive(true);
                gameIcon[3].GetComponent<Image>().color = Color.white;
                gameIcon[3].GetComponent<Button>().enabled = true;
                break;

            case "Infect":
                gameIcon[0].SetActive(false);
                gameIcon[1].SetActive(true);
                gameIcon[3].GetComponent<Image>().color = Color.gray;
                gameIcon[3].GetComponent<Button>().enabled = false;
                break;
        }
    }

    public void ChangeUserClueUIText(string _username, string _usercode, int _index)
    {
        CluePanelCanvas.GetChild(0).GetChild(0).GetComponent<Text>().text = _username;
        CluePanelCanvas.GetChild(0).GetChild(1).GetComponent<Text>().text = _usercode;
        UserClueUI.transform.GetChild(0).GetChild(_index).GetChild(0).GetComponent<Text>().text = _username;
        UserClueUI.transform.GetChild(0).GetChild(_index).GetChild(1).GetComponent<Text>().text = _usercode;

        CluePanelCanvas.GetChild(0).gameObject.SetActive(true);
    }

    public void ChangeCodeClueUIText(char _usercode, int _index)
    {
        CluePanelCanvas.GetChild(1).GetChild(0).GetComponent<Text>().text = _usercode.ToString();
        CollocClueUI.transform.GetChild(0).GetChild(_index).GetChild(0).GetComponent<Text>().text = _usercode.ToString();

        CluePanelCanvas.GetChild(1).gameObject.SetActive(true);
    }

    public void ChangeClueStatusUIText(string _status)
    {
        CluePanelCanvas.GetChild(2).GetChild(0).GetComponent<Text>().text = _status;
        CluePanelCanvas.GetChild(2).gameObject.SetActive(true);
    }

    public void UnactivePanel(int _index)
    {
        CluePanelCanvas.GetChild(_index).gameObject.SetActive(false);
    }

    public void UserToColloc()
    {
        UserClueUI.SetActive(false);
        CollocClueUI.SetActive(true);
    }

    public void CollocToUser()
    {
        UserClueUI.SetActive(true);
        CollocClueUI.SetActive(false);
    }

    public void ShowClueUI()
    {
        for (int i = NetworkManager._currentPlayer; i < 6; i++)
        {
            ClueUI.transform.GetChild(0).GetChild(0).GetChild(i).gameObject.SetActive(false);
        }

        ClueUI.gameObject.SetActive(true);
        UserClueUI.SetActive(true);
        CollocClueUI.SetActive(false);
    }

    public void OnClickDoneButton()
    {
        ClueUI.gameObject.SetActive(false);
    }
}
