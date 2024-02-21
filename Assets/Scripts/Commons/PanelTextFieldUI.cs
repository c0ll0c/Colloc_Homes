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
            AlertManager.Instance.WarnAlert("�Է�â�� ��� �ݿ��� �Ұ����մϴ�.");
            return string.Empty;
        }

        gameObject.SetActive(false);
        return inputField.text;
    }
}
