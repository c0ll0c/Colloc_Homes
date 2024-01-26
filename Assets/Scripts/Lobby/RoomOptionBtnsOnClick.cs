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
                transform.parent.GetChild(2).GetComponent<Image>().color = Color.gray;
                break;
            case RoomOptionBtnType.PRIVATE:
                CreateRoomManager.RoomPublic = false;
                transform.parent.GetChild(1).GetComponent<Image>().color = Color.gray;
                break;
        }
    }
}
