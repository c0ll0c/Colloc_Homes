using System.Collections.Generic;
using UnityEngine;
using System;

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

[Serializable]
public struct EffectAudio
{
    public EffectAudioType EffectType;
    public AudioClip Audio;
}

// 전체적인 음향 관리
public class AudioManager: MonoSingleton<AudioManager>
{
    public AudioSource BgmPlayer;
    public AudioSource EffectPlayer;

    public AudioClip GameBGM;
    public AudioClip LobbyBGM;

    public EffectAudio[] AudioClips;
    private readonly Dictionary<EffectAudioType, AudioClip> audios = new();

    private void Start()
    {
        BgmPlayer.clip = LobbyBGM;
        BgmPlayer.Play();

        foreach(EffectAudio effectAudio in AudioClips)
        {
            audios.Add(effectAudio.EffectType, effectAudio.Audio);
        }
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
        EffectPlayer.clip = audios[_effectType];
        EffectPlayer.Play();
    }
}
