using TMPro;
using UnityEngine;

public class RoomnameInputFieldUI : MonoBehaviour
{
    public TMP_InputField RoomnameField;

    public void CreateRoom()
    {
        if (string.IsNullOrEmpty(RoomnameField.text)) return;

        NetworkManager.Instance.CreateRoom(RoomnameField.text);
    }
}
