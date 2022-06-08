using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using STAGE_MANAGEMENT;

public class OptionManager : MonoBehaviour
{
    // 오디오 믹스관리 by 김예찬
    [SerializeField]
    private AudioMixer m_AudioMixer; // 이펙트 음향 믹스 그룹


    // 음향 UI by 최지은
    public Slider m_bgmSlider;
    public Text m_bgmPercent;
    public Toggle m_bgmToggle;
    public AudioSource m_bgmAudio;
    public Slider m_sfxSlider;
    public Text m_sfxPercent;
    public Toggle m_sfxToggle;
    //public AudioSource m_sfxAudio;
    //public AudioClip[] m_sfxClip;

    private float m_bgmVolume;
    private float m_sfxVolume;

    public Player player = null;

    private void Awake()
    {
        
    }
    private void Start()
    {
        player = StageManager.Inst.Player.GetComponent<Player>();
    }

    public void Update()
    {
        m_bgmVolume = m_bgmSlider.value;
        m_bgmAudio.volume = m_bgmVolume;

        // 효과음 볼륨믹서
        m_sfxVolume = m_sfxSlider.value;
        m_AudioMixer.SetFloat("Effect", Mathf.Lerp(-80.0f, 0.0f, m_sfxVolume)); //범위 -80 ~ 0 데시벨 기준 볼륨 셋팅
    }

    public void Move_Music(float value)
    {
        m_bgmPercent.text = (Mathf.RoundToInt((float)m_bgmSlider.value * 100) + "%");
    }

    public void Move_Effect(float value)
    {
        m_sfxPercent.text = (Mathf.RoundToInt((float)m_sfxSlider.value * 100) + "%");
    }

    public void MusicToggle()
    {
        if(m_bgmToggle.isOn == true)
        {
            m_bgmAudio.mute = true;
        }
        else if(m_bgmToggle.isOn == false)
        {
            m_bgmAudio.mute = false;
        }
    }

    public void EffectToggle()
    {
        if (m_sfxToggle.isOn == true)
        {
            m_AudioMixer.SetFloat("MuteForEffect", Mathf.Lerp(-80.0f, 0.0f, 0));
        }
        else if (m_sfxToggle.isOn == false)
        {
            m_AudioMixer.SetFloat("MuteForEffect", Mathf.Lerp(-80.0f, 0.0f, 1));
        }
    }

    public void OnClickBack()
    {
        if(SceneManager.GetActiveScene().name == "TitleScene")
        {
            gameObject.SetActive(false);
        }
        else
        {
            player.OnStop();
            gameObject.SetActive(false);
        }
    }
}
