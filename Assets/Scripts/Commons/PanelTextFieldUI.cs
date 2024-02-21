using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PanelTextFieldUI : MonoBehaviour
{
    public BtnSoundOnClick BtnSoundScript;
    public TMP_InputField inputField;
    public Button SaveBtn;

    public void SetTextAndOpen(string _text)
    {
        inputField.text = _text;

        SaveBtn.onClick.RemoveAllListeners();
        SaveBtn.onClick.AddListener(() => BtnSoundScript.ButtonClickAudio());

        gameObject.SetActive(true);
    }

    public string ReturnTextAndClose()
    {
        if (string.IsNullOrWhiteSpace(inputField.text))
        {
            AlertManager.Instance.WarnAlert("입력창이 비어 반영이 불가능합니다.");
            return string.Empty;
        }

        gameObject.SetActive(false);
        return inputField.text;
    }
}
