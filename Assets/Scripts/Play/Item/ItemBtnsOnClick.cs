using UnityEngine;
using TMPro;

public enum ItemBtnType
{
    DETOX_USE
}

public class ItemBtnsOnClick : MonoBehaviour
{
    public TMP_Text BtnText;
    public ItemBtnType BtnType;

    public void ChangeBtnText(string _text)
    {
        BtnText.text = _text;
    }

    public void OnClickBtn()
    {
        
    }
}
