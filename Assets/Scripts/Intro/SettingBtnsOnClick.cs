using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum SettingOptionBtnType
{
    ON,
    OFF,
}


public class SettingBtnsOnClick : MonoBehaviour
{
    public SettingOptionBtnType BtnType;

    public void OnClickBtn()
    {
        transform.parent.GetChild(1).GetComponent<Image>().color = Color.gray;
        transform.parent.GetChild(2).GetComponent<Image>().color = Color.gray;
        transform.GetComponent<Image>().color = Color.white;
    }
}
