using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TilteUI : MonoBehaviour
{
    private GameObject m_settingPanel;

    private void Awake()
    {
        m_settingPanel = GameObject.Find("SettingPanel").gameObject;
        m_settingPanel.SetActive(false);
    }

    public void OnClickNew()
    {
        Debug.Log("NewGame");
        //SceneManager.LoadScene("");
    }

    public void OnClickLoad()
    {
        Debug.Log("LoadGame");
        //SceneManager.LoadScene("");
    }

    public void OnClickSetting()
    {
        Debug.Log("Setting");
        m_settingPanel.SetActive(true);
    }

    public void OnClickSettingBack()
    {
        Debug.Log("SettingBack");
        m_settingPanel.SetActive(false);
    }

    public void OnClickQuit()
    {
        Debug.Log("Quit");
        //Application.Quit();
    }
}
