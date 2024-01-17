using TMPro;
using UnityEngine;

public class CreateRoomOptionsUI: MonoBehaviour
{
    public TMP_InputField RoomnameField;
    public bool RoomPublic;

    private void Start()
    {
        RoomPublic = true;
    }

    public void CreateRoom()
    {
        if (string.IsNullOrEmpty(RoomnameField.text)) return;

        NetworkManager.Instance.CreateRoom(RoomnameField.text, !RoomPublic);
    }
}
