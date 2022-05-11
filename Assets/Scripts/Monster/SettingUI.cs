using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingUI : MonoBehaviour
{
    public Slider m_masterVolume;
    public Text m_masterPercent;
    public Slider m_musicVolume;
    public Text m_musicPercent;
    public Slider m_effectVolume;
    public Text m_effectPercent;

    public void Move_Master(float value)
    {
        m_masterPercent.GetComponent<Text>().text = ((int)m_masterVolume.value + "%");
    }

    public void Move_Music(float value)
    {
        m_musicPercent.GetComponent<Text>().text = ((int)m_musicVolume.value + "%");
    }

    public void Move_Effect(float value)
    {
        m_effectPercent.GetComponent<Text>().text = ((int)m_effectVolume.value + "%");
    }

}
