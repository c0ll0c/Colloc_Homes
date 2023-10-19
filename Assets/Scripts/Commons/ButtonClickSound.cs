using UnityEngine;

public class ButtonClickSound : MonoBehaviour
{
    public void ButtonClickAudio()
    {
        AudioManager.Instance.PlayEffect(EffectAudioType.BTN_CLICK);
    }
}
