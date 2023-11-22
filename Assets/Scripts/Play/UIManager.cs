using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoSingleton<UIManager>
{
    public Transform[] CluePanelCanvas = new Transform[3];             // Ž�� ī��, �ڵ� ����, ���� ǥ��
    public GameObject UserClueUI;
    public GameObject CollocClueUI;
    public Transform UserInfo;
    public Transform CodeInfo;
    public Canvas ClueUI;
    public Sprite[] playerSprite;

    private int i = 0;

    [SerializeField] private GameObject[] gameIcon = new GameObject[5];

    protected override void Awake()
    {
        base.Awake();

        ClueUI.gameObject.SetActive(false);
        for (int i = 0; i < gameIcon.Length - 1; i++)
        {
            gameIcon[i].SetActive(false);
        }
        
        for(int i = 0; i < 5; i++)
        {
            CodeInfo.GetChild(i).gameObject.SetActive(false);
        }
    }

    public void SetGameUI(string _status)
    {
        switch (_status)
        {
            case "Colloc":
                gameIcon[2].SetActive(true);
                gameIcon[3].SetActive(false);
                gameIcon[4].GetComponent<Image>().color = Color.red;
                break;

            case "Homes":
                gameIcon[0].SetActive(true);
                gameIcon[1].SetActive(false);
                gameIcon[3].SetActive(true);
                gameIcon[3].GetComponent<Image>().color = Color.white;
                gameIcon[3].GetComponent<Button>().enabled = true;
                gameIcon[4].GetComponent<Image>().color = Color.blue;
                break;

            case "Infect":
                gameIcon[0].SetActive(false);
                gameIcon[1].SetActive(true);
                gameIcon[3].GetComponent<Image>().color = Color.gray;
                gameIcon[3].GetComponent<Button>().enabled = false;
                gameIcon[4].GetComponent<Image>().color = Color.red;
                break;
        }
    }

    public void ChangeUserClueUIText(string _username, string _usercode, int _index, string _color)
    {
        Image cardSourceImage = CluePanelCanvas[0].GetChild(2).GetChild(0).GetComponent<Image>();

        // �ܼ� ���� ������ �� ������ Ž�� ī��
        CluePanelCanvas[0].GetChild(0).GetComponent<Text>().text = _username;
        CluePanelCanvas[0].GetChild(1).GetComponent<Text>().text = _usercode;

        UserInfo.GetChild(_index).GetChild(1).GetComponent<Text>().text = _usercode;        // �길 �� �൵ �ǰԲ�

        switch (_color)
        {
            case "Brown":
                cardSourceImage.sprite = playerSprite[0];
                break;
            case "Blue":
                cardSourceImage.sprite = playerSprite[1];
                break;
            case "Gray":
                cardSourceImage.sprite = playerSprite[2];
                break;
            case "Green":
                cardSourceImage.sprite = playerSprite[3];
                break;
            case "Orange":
                cardSourceImage.sprite = playerSprite[4];
                break;
            case "Pink":
                cardSourceImage.sprite = playerSprite[5];
                break;
            case "Purple":
                cardSourceImage.sprite = playerSprite[6];
                break;
            case "Yellow":
                cardSourceImage.sprite = playerSprite[7];
                break;
        }

        CluePanelCanvas[0].gameObject.SetActive(true);
    }

    public void ChangeCodeClueUIText(char _usercode)
    {
        // �ܼ� ���� ������ �� ������ �ܼ� ����
        CluePanelCanvas[1].GetChild(0).GetComponent<Text>().text = _usercode.ToString();

        // gameUI ��ư ������ �� ������ collocInfo ����ȭ (get)
        CodeInfo.GetChild(i).gameObject.SetActive(true);
        CodeInfo.GetChild(i).GetChild(0).GetComponent<Text>().text = _usercode.ToString();

        if(i!=5) i++;

        CluePanelCanvas[1].gameObject.SetActive(true);
    }

    public void ChangeClueStatusUIText(string _status)
    {
        // ����, �̹� �� �ܼ�, ���� �ܼ� �� ���� ǥ��
        CluePanelCanvas[2].GetChild(0).GetComponent<Text>().text = _status;
        CluePanelCanvas[2].gameObject.SetActive(true);
    }

    public void UnactivePanel(int _index)
    {
        // �ܼ� �����տ� �پ� �ִ� ��ư ������ �� �� �ǳ� false
        CluePanelCanvas[_index].gameObject.SetActive(false);
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

        for (int i = 0; i < NetworkManager._currentPlayer; i++)
        {
            UserInfo.GetChild(i).gameObject.SetActive(true);
        }

        // gameUI �ܼ� ��ø ��ư ������ �� UI�� ���̰�
        ClueUI.gameObject.SetActive(true);
        UserClueUI.SetActive(true);
        CollocClueUI.SetActive(false);
    }

    public void OnClickDoneButton()
    {
        ClueUI.gameObject.SetActive(false);
    }
}
