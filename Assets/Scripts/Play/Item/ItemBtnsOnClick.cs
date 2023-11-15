using System.Collections;
using System.Collections.Generic;
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

    // Detox_n ÀÇ n
    public int DetoxIndex;

    public void ChangeBtnText(string _text)
    {
        BtnText.text = _text;
    }

    public void OnClickBtn()
    {
        switch (BtnType)
        {
            case ItemBtnType.DETOX_USE:
                NetworkManager.Instance.SyncDetox(DetoxIndex);
                break;
        }
    }
}
