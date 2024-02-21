using UnityEngine;
using UnityEngine.UI;

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
        transform.GetComponent<Image>().color = Color.gray;

        switch (BtnType)
        {
            case RoomOptionBtnType.PUBLIC:
                CreateRoomManager.RoomPublic = true;
                transform.parent.GetChild(2).GetComponent<Image>().color = Color.white;
                break;
            case RoomOptionBtnType.PRIVATE:
                CreateRoomManager.RoomPublic = false;
                transform.parent.GetChild(1).GetComponent<Image>().color = Color.white;
                break;
        }
    }
}
