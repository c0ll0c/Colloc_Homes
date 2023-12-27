using TMPro;
using UnityEngine;

public class RoomnameInputFieldUI : MonoBehaviour
{
    public TMP_InputField RoomnameField;
    public bool RoomPublic;
    public int PlayerNum;

    private void Start()
    {
        RoomPublic = true;
        PlayerNum = 4;
    }

    public void CreateRoom()
    {
        if (string.IsNullOrEmpty(RoomnameField.text)) return;

        NetworkManager.Instance.CreateRoom(RoomnameField.text, RoomPublic, PlayerNum);
    }
}
