using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.U2D.Animation;

public class UIManager : MonoBehaviour
{
    public Transform[] CluePanelCanvas = new Transform[3];             // 탐정 카드, 코드 조각, 상태 표시
    public GameObject UserClueUI;
    public GameObject CollocClueUI;
    public Transform UserInfo;
    public Transform CodeInfo;
    public Transform HomesInfo;
    public Canvas ClueUI;

    public GameObject StartPanelObj;
    public GameObject NoticePanelObj;
    public Text NoticeText;

    private int i = 0;
    public bool isColloc;

    [SerializeField] private GameObject[] gameIcon = new GameObject[4];

    private static UIManager instance;

    public static UIManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<UIManager>();
                if (instance == null)
                {
                    GameObject obj = new GameObject();
                    obj.name = typeof(UIManager).Name;
                    instance = obj.AddComponent<UIManager>();
                }
            }
            return instance;
        }
    }

    protected virtual void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        ClueUI.gameObject.SetActive(false);

        for (int i = 0; i < gameIcon.Length - 1; i++)
        {
            gameIcon[i].SetActive(false);
        }

        for (int i = 0; i < 5; i++)
        {
            CodeInfo.GetChild(i).gameObject.SetActive(false);
        }

        NoticeText = NoticePanelObj.transform.GetChild(0).GetComponent<Text>();
        NoticePanelObj.SetActive(false);
    }

    public void SetGameUI(string _status)
    {
        if (_status.Equals(StaticVars.TAG_COLLOC))
        {
            gameIcon[0].SetActive(false);
            gameIcon[1].SetActive(false);
            gameIcon[2].SetActive(true);
            gameIcon[3].SetActive(false);
        }
        else if (_status.Equals(StaticVars.TAG_HOLMES))
        {
            gameIcon[0].SetActive(true);
            gameIcon[1].SetActive(false);
            gameIcon[2].SetActive(false);
            gameIcon[3].SetActive(true);
            gameIcon[3].GetComponent<Image>().color = Color.white;
            gameIcon[3].GetComponent<Button>().enabled = true;
        }
        else if (_status.Equals(StaticVars.TAG_INFECT))
        {
            gameIcon[0].SetActive(false);
            gameIcon[1].SetActive(true);
            gameIcon[2].SetActive(false);
            gameIcon[3].SetActive(true);
            gameIcon[3].GetComponent<Image>().color = Color.gray;
            gameIcon[3].GetComponent<Button>().enabled = false;
        }
    }

    public void LoadStartPanel(bool _isColloc)
    {
        StartPanelObj.SetActive(true);
        StartPanelObj.transform.GetChild(1).GetChild(0).gameObject.SetActive(true);
        StartPanelObj.transform.GetChild(2).GetChild(0).gameObject.SetActive(true);
        isColloc = _isColloc;
    }

    public void DeactivateStartPanel()
    {
        StartCoroutine(StartDeactiveCount());
    }

    private IEnumerator StartDeactiveCount()
    {
        StartPanelObj.transform.GetChild(3).gameObject.SetActive(false);

        TMP_Text countText = StartPanelObj.transform.GetChild(4).GetComponent<TMP_Text>();
        int count = StaticVars.START_PANEL_TIME;
        while (count > 0)
        {
            countText.text = count.ToString();
            yield return StaticFuncs.WaitForSeconds(1);
            count--;
        }
        countText.text = string.Empty;

        StartPanelObj.transform.GetChild(1).GetChild(0).gameObject.SetActive(false);
        StartPanelObj.transform.GetChild(2).GetChild(0).gameObject.SetActive(false);

        AudioManager.Instance.PlayEffect(EffectAudioType.STATE);

        // Status Text
        StartPanelObj.transform.GetChild(1).GetChild(1).gameObject.SetActive(isColloc);
        StartPanelObj.transform.GetChild(1).GetChild(2).gameObject.SetActive(!isColloc);
        // Status Img
        StartPanelObj.transform.GetChild(2).GetChild(1).gameObject.SetActive(isColloc);
        StartPanelObj.transform.GetChild(2).GetChild(2).gameObject.SetActive(!isColloc);
        yield return StaticFuncs.WaitForSeconds(1);

        StartPanelObj.SetActive(false);
    }

    public void ChangeUserClueUIText(string _username, string _usercode, int _index, string _color)
    {
        // 단서 보기 눌렀을 때 나오는 탐정 카드
        CluePanelCanvas[0].GetChild(1).GetComponent<Text>().text = _username;
        CluePanelCanvas[0].GetChild(2).GetComponent<Text>().text = _usercode;

        UserInfo.GetChild(_index).GetChild(1).GetComponent<Text>().text = _usercode;

        Image homesImg = CluePanelCanvas[0].GetChild(3).GetChild(0).GetComponent<Image>();
        SpriteLibraryAsset sprites = CluePanelCanvas[0].GetChild(3).GetChild(0).GetComponent<SpriteLibrary>().spriteLibraryAsset;
        homesImg.sprite = sprites.GetSprite("Color", _color);

        CluePanelCanvas[0].gameObject.SetActive(true);
    }

    public void ChangeCodeClueUIText(char _usercode)
    {
        // 단서 보기 눌렀을 때 나오는 단서 조각
        CluePanelCanvas[1].GetChild(1).GetComponent<Text>().text = _usercode.ToString();

        // gameUI 버튼 눌렀을 때 나오는 collocInfo 동기화 (get)
        CodeInfo.GetChild(i).gameObject.SetActive(true);
        CodeInfo.GetChild(i).GetChild(0).GetComponent<Text>().text = _usercode.ToString();

        if (i != 5) i++;

        CluePanelCanvas[1].gameObject.SetActive(true);
    }

    public void ChangeClueStatusUIText(string _status)
    {
        // 숨김, 이미 본 단서, 숨긴 단서 등 상태 표시
        CluePanelCanvas[2].GetChild(1).GetComponent<Text>().text = _status;
        CluePanelCanvas[2].gameObject.SetActive(true);
    }

    public void UserToColloc()
    {
        AudioManager.Instance.PlayEffect(EffectAudioType.PAPER);

        UserClueUI.SetActive(false);
        CollocClueUI.SetActive(true);
    }

    public void CollocToUser()
    {
        AudioManager.Instance.PlayEffect(EffectAudioType.PAPER);

        UserClueUI.SetActive(true);
        CollocClueUI.SetActive(false);
    }

    public void ShowClueUI()
    {
        for (int i = 0; i < NetworkManager._currentPlayer; i++)
        {
            UserInfo.GetChild(i).gameObject.SetActive(true);
        }

        // gameUI 단서 수첩 버튼 눌렀을 때 UI가 보이게
        ClueUI.gameObject.SetActive(true);
        UserClueUI.SetActive(true);
        CollocClueUI.SetActive(false);
    }

    public void OnClickDoneButton()
    {
        ClueUI.gameObject.SetActive(false);
    }
}
