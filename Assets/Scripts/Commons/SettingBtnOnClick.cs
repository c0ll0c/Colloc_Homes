using UnityEngine;

public class SettingBtnOnClick : MonoBehaviour
{
    public void OnClickBtn()
    {
        AudioManager.Instance.OpenAudioSettingPanel();
    }
}
