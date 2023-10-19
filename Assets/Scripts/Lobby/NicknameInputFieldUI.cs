using TMPro;
using UnityEngine;

public class NicknameInputFieldUI : MonoBehaviour
{
    public TMP_InputField NicknameField;

    private void OnEnable()
    {
        NicknameField.text = GameManager.Instance.PlayerName;
    }

    public void SaveNickname()
    {
        if (string.IsNullOrEmpty(NicknameField.text)) return;

        // [TODO] nickname�� validate(2~9��)
        PlayerPrefs.SetString(StaticVars.PREFS_NICKNAE, NicknameField.text);
        GameManager.Instance.PlayerName = NicknameField.text;
    }
}
