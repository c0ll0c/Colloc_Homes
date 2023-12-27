using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum RoomOptionBtnType
{
    PUBLIC,
    PRIVATE,
}


public class RoomOptionBtnsOnClick : MonoBehaviour
{
    public CreateRoomOptionsUI CreateRoomManager;
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
        }
    }
}
