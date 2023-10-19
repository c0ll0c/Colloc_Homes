using UnityEngine;

public class BtnSoundOnClick : MonoBehaviour
{
    public void ButtonClickAudio()
    {
        AudioManager.Instance.PlayEffect(EffectAudioType.BTN_CLICK);
    }
}
