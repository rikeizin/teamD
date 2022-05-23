using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TilteUI : MonoBehaviour
{
    public GameObject m_optionPanel;

    private void Awake()
    {
        m_optionPanel.SetActive(false);
    }

    public void OnClickStart()
    {
        Debug.Log("StartGame");
        SceneManager.LoadScene("Stage_Inn");
    }

    public void OnClickSetting()
    {
        m_optionPanel.SetActive(true);
    }

    public void OnClickQuit()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
