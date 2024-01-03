using TMPro;
using UnityEngine;

public class CodeInputFieldUI : MonoBehaviour
{
    public TMP_InputField CodeField;
    public TMP_InputField PasswordField;

    public void SearchRoom()
    {
        if (string.IsNullOrEmpty(CodeField.text)) return;
        if (true)
        {
            NetworkManager.Instance.JoinRoom(CodeField.text);
        }
        else
        {
            transform.GetChild(1).gameObject.SetActive(true);
        }
    }

    public void CheckPassword()
    {
        
    }
}
