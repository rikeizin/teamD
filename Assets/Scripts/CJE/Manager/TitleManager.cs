using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using STAGE_MANAGEMENT;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    public BGMExeManager m_bgmExeManager;

    private void Start()
    {
        OnStart();
    }

    private void OnStart()
    {
        m_bgmExeManager.AddAudioToCAM();
        m_bgmExeManager.PlayInnBGM();
    }
}
