using UnityEngine;

public enum EffectAudioType
{
    DETOX,
    PORTAL,
    PLANE,
    VACCINE,
    BTN_CLICK,
    BTN_POP,
    WIN,
    LOSE
}

// 전체적인 음향 관리
public class AudioManager: MonoSingleton<AudioManager>
{
    public AudioSource BgmPlayer;
    public AudioSource EffectPlayer;

    public AudioClip GameBGM;
    public AudioClip LobbyBGM;

    public AudioClip DetoxAudio;
    public AudioClip PortalAudio;
    public AudioClip PlaneAudio;
    public AudioClip VaccineAudio;

    public AudioClip ButtonPopAudio;
    public AudioClip ButtonClickAudio;

    public AudioClip WinAudio;
    public AudioClip LoseAudio;

    private void Start()
    {
        BgmPlayer.clip = LobbyBGM;
        BgmPlayer.Play();
    }

    public void ChangeBGM(GameState _state)
    {
        if (_state == GameState.READY) BgmPlayer.clip = GameBGM;
        else BgmPlayer.clip = LobbyBGM;

        if (!BgmPlayer.isPlaying) BgmPlayer.Play();
    }

    // [MIGHTDO] Effect Play Queue?
    public void PlayEffect(EffectAudioType _effectType)
    {
        switch (_effectType)
        {
            case EffectAudioType.DETOX:
                EffectPlayer.clip = DetoxAudio; break;
            case EffectAudioType.PORTAL:
                EffectPlayer.clip = PortalAudio; break;
            case EffectAudioType.PLANE:
                EffectPlayer.clip = PlaneAudio; break;
            case EffectAudioType.VACCINE:
                EffectPlayer.clip = VaccineAudio; break;
            case EffectAudioType.BTN_CLICK:
                EffectPlayer.clip = ButtonClickAudio; break;
            case EffectAudioType.BTN_POP:
                EffectPlayer.clip = ButtonPopAudio; break;
            case EffectAudioType.WIN:
                EffectPlayer.clip = WinAudio; break;
            case EffectAudioType.LOSE:
                EffectPlayer.clip = LoseAudio; break;
        }

        EffectPlayer.Play();
    }
}
