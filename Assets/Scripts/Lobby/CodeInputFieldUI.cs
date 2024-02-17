using TMPro;
using UnityEngine;

public class CodeInputFieldUI : MonoBehaviour
{
    public TMP_InputField CodeField;

    public void FindRoom()
    {
        if (string.IsNullOrEmpty(CodeField.text)) return;
        NetworkManager.Instance.JoinRoom(CodeField.text.ToUpper());
    }
}
