using TMPro;
using UnityEngine;

public class NicknameInputFieldUI : MonoBehaviour
{
    public TMP_InputField NicknameField;
    public TMP_Text tmpNickname;

    private void OnEnable()
    {
        NicknameField.text = GameManager.Instance.PlayerName;
    }

    public void SaveNickname()
    {
        if (string.IsNullOrEmpty(NicknameField.text)) return;
        // [TODO] nickname°ª validate(2~9ÀÚ)
        PlayerPrefs.SetString(StaticVars.PREFS_NICKNAE, NicknameField.text);
        GameManager.Instance.PlayerName = NicknameField.text;
        tmpNickname.text = GameManager.Instance.PlayerName;
    }
}
