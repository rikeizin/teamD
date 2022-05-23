using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public enum SoundTrack : byte
{
    Inn = 0,
    NormalStage,
    BossStage
}

[RequireComponent(typeof(AudioSource))]
public class BGMExeManager : MonoBehaviour
{
    #region 맴버변수 필드
    private static BGMExeManager instance = null;
    public static BGMExeManager Inst { get => instance; }

    private AudioSource m_AudioSource = null;

    [Header("MIX GROUP SETTING")]
    [SerializeField]
    private AudioMixerGroup bgmMixGroup = null;

    [Header("배경음 온 오프")]
    public bool bgmOnOff = false;
    
    [Header("사운드 트랙")]
    private AudioClip[] playList;
    [SerializeField]
    private uint maxPlayListCount = 10;
    [SerializeField]
    private AudioClip[] InnTrack = null;
    [SerializeField]
    public AudioClip[] normalStageTrack = null;
    [SerializeField]
    public AudioClip[] bossStageTrack = null;

    public bool BGMOnOff
    {
        get => bgmOnOff;
        set
        {
            bgmOnOff = value;
            if (m_AudioSource != null)
            {
                if (bgmOnOff)
                {
                    m_AudioSource.mute = false;
                }
                else
                {
                    m_AudioSource.mute = true;
                }
            }
        }
    }
    #endregion

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            instance.Initalize();
            DontDestroyOnLoad(this.gameObject);
        } 
        else
        {
            if (instance != this)
            {
                Destroy(this.gameObject);
            }
        }
    }

    private void Initalize()
    {
        m_AudioSource = GetComponent<AudioSource>();

        if (InnTrack.Length < 1)
        {
            InnTrack = Resources.LoadAll<AudioClip>("Sounds/BGM/InnBGM");
        }

        if (normalStageTrack.Length < 1)
        {
            normalStageTrack = Resources.LoadAll<AudioClip>("Sounds/BGM/NomarlBGM");
        }

        if (bossStageTrack.Length < 1)
        {
            bossStageTrack = Resources.LoadAll<AudioClip>("Sounds/BGM/BossBGM");
        }

        if (bgmMixGroup == null)
        {
            AudioMixerGroup[] mixGroups = Resources.Load<AudioMixer>("AudioMixer").FindMatchingGroups("Master");
            foreach (var mixGroup in mixGroups)
            {
                if (mixGroup.name == "BGM")
                {
                    bgmMixGroup = mixGroup;

                }
            }
        }
        
        if(m_AudioSource.outputAudioMixerGroup == null)
        {
            m_AudioSource.outputAudioMixerGroup = bgmMixGroup;
        }
    }

    private void OnValidate()
    {
        BGMOnOff = bgmOnOff ? true : false;
    }
    
    /// <summary>
    /// 연속재생
    /// </summary>
    /// <returns></returns>
    IEnumerator ContinuousPlay()
    {
        m_AudioSource.Stop();
        m_AudioSource.clip = playList[Random.Range(0, playList.Length)];
        m_AudioSource.Play();

        if (playList.Length < 1)
        {
            m_AudioSource.loop = true;
        }
        else
        {
            while (true)
            {
                yield return new WaitUntil(() => m_AudioSource.isPlaying == false);
                m_AudioSource.clip = playList[Random.Range(0, playList.Length)];
                m_AudioSource.Play();
            }
        }
    }

    public void StopSoundTrack()
    {
        StopCoroutine(ContinuousPlay());
    }

    /// <summary>
    /// 사운드 트랙 변경
    /// </summary>
    /// <param name="track"></param>
    public void ChangeSoundTrack(SoundTrack track)
    {
        StopSoundTrack();
        switch (track)
        {
            case SoundTrack.Inn:
                playList = InnTrack;
                break;
            case SoundTrack.NormalStage:
                playList = normalStageTrack;
                break;
            case SoundTrack.BossStage:
                playList = bossStageTrack;
                break;
        }
        StartCoroutine(ContinuousPlay());
    }
}

