using System.Collections.Generic;
using UnityEngine;
using System;

public enum EffectAudioType
{
    DETOX,
    UNDETOX,
    PORTAL,
    PLANE,
    VACCINE,
    BTN_CLICK,
    BTN_POP,
    WIN,
    LOSE,
    ATTACK,
    ATTACKED,
    INFECT,
    DROP,
    COOLTIME,
    ACTIVE
}

[Serializable]
public struct EffectAudio
{
    public EffectAudioType EffectType;
    public AudioClip Audio;
}

// ��ü���� ���� ����
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

        if (_effectType == EffectAudioType.PLANE || _effectType == EffectAudioType.COOLTIME)
            EffectPlayer.loop = true;
        else
            EffectPlayer.loop = false;

        EffectPlayer.Play();
    }

    public void PauseEffect()
    {
        EffectPlayer.Pause();
    }
}
