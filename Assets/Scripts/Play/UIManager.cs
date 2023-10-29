using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoSingleton<UIManager>
{
    public Transform CluePanelCanvas;
    public Transform ClueUIButton;

    private void Start()
    {
        ClueUIButton.gameObject.SetActive(false);
    }

    public void ChangeUserClueUIText(string _username, string _usercode, int _index)
    {
        CluePanelCanvas.GetChild(0).GetChild(0).GetComponent<Text>().text = _username;
        CluePanelCanvas.GetChild(0).GetChild(1).GetComponent<Text>().text = _usercode;
        ClueUIButton.GetChild(0).GetChild(_index).GetChild(0).GetComponent<Text>().text = _username;
        ClueUIButton.GetChild(0).GetChild(_index).GetChild(1).GetComponent<Text>().text = _usercode;

        CluePanelCanvas.GetChild(0).gameObject.SetActive(true);
    }

    public void ChangeCodeClueUIText(string _usercode, int _index)
    {
        CluePanelCanvas.GetChild(1).GetChild(0).GetComponent<Text>().text = _usercode;
        ClueUIButton.GetChild(1).GetChild(_index).GetChild(0).GetComponent<Text>().text = _usercode;

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
}
