using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum RoomOptionBtnType
{
    PUBLIC,
    PRIVATE,
    FOUR,
    FIVE,
    SIX,
}


public class RoomOptionBtnsOnClick : MonoBehaviour
{
    public RoomnameInputFieldUI CreateRoomManager;
    public RoomOptionBtnType BtnType;

    public void OnClickBtn()
    {
        transform.GetComponent<Image>().color = Color.white;

        switch (BtnType)
        {
            case RoomOptionBtnType.PUBLIC:
                CreateRoomManager.RoomPublic = true;
                transform.parent.GetChild(3).GetComponent<Image>().color = Color.gray;
                transform.parent.GetChild(4).GetChild(2).GetComponent<TMP_InputField>().interactable = false;
                transform.parent.GetChild(4).GetChild(2).GetComponent<TMP_InputField>().text = "";
                break;
            case RoomOptionBtnType.PRIVATE:
                CreateRoomManager.RoomPublic = false;
                transform.parent.GetChild(2).GetComponent<Image>().color = Color.gray;
                transform.parent.GetChild(4).GetChild(2).GetComponent<TMP_InputField>().interactable = true;
                break;
            case RoomOptionBtnType.FOUR:
                CreateRoomManager.PlayerNum = 4;
                transform.parent.GetChild(3).GetComponent<Image>().color = Color.gray;
                transform.parent.GetChild(4).GetComponent<Image>().color = Color.gray;
                break;
            case RoomOptionBtnType.FIVE:
                CreateRoomManager.PlayerNum = 5;
                transform.parent.GetChild(2).GetComponent<Image>().color = Color.gray;
                transform.parent.GetChild(4).GetComponent<Image>().color = Color.gray;
                break;
            case RoomOptionBtnType.SIX:
                CreateRoomManager.PlayerNum = 6;
                transform.parent.GetChild(2).GetComponent<Image>().color = Color.gray;
                transform.parent.GetChild(3).GetComponent<Image>().color = Color.gray;
                break;
        }
    }
}
