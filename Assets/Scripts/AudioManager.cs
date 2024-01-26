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
    ACTIVE,
    PAPER,
    ENTER,
    STATE
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
    public AudioSource PlayerFoot;
    public AudioClip GameBGM;
    public AudioClip LobbyBGM;

    public static int EFFECT_AUDIO_SRC_NUM = 3;
    public GameObject EffectSourceObj;
    private AudioSource[] effectPlayers;
    private Queue<EffectAudioType> audioQueue = new();
    private EffectAudioType?[] playingAudios = new EffectAudioType?[EFFECT_AUDIO_SRC_NUM];

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

        effectPlayers = EffectSourceObj.GetComponentsInChildren<AudioSource>();

        for(int i=0; i< EFFECT_AUDIO_SRC_NUM; i++)
        {
            playingAudios[i] = null;
        }
    }

    private void Update()
    {
        if (audioQueue.Count > 0)
        {
            for (int i = 0; i < EFFECT_AUDIO_SRC_NUM; i++)
            {
                if (!effectPlayers[i].isPlaying)
                {
                    EffectAudioType effectToPlay = audioQueue.Dequeue();

                    playingAudios[i] = effectToPlay;
                    effectPlayers[i].clip = audios[effectToPlay];
                    effectPlayers[i].loop = (effectToPlay == EffectAudioType.PLANE || effectToPlay == EffectAudioType.COOLTIME);
                    effectPlayers[i].Play();

                    return;
                }
            }
        }
    }

    public void ChangeBGM(GameState _state)
    {
        if (_state == GameState.READY) BgmPlayer.clip = GameBGM;
        else BgmPlayer.clip = LobbyBGM;

        if (!BgmPlayer.isPlaying) BgmPlayer.Play();
    }

    public void PlayerFootSound()
    {
        if (!PlayerFoot.isPlaying)
        {
            PlayerFoot.Play();
        }
    }

    public void PauseFootSound()
    {
        PlayerFoot.Pause();
    }

    public void PlayEffect(EffectAudioType _effectType)
    {
        audioQueue.Enqueue(_effectType);
    }

    public void PauseEffect(EffectAudioType _effectType)
    {
        for (int i = 0; i < EFFECT_AUDIO_SRC_NUM; i++)
        {
            if (playingAudios[i] == _effectType)
            {
                effectPlayers[i].Pause();
                playingAudios[i] = null;
            }
        }
    }
}
