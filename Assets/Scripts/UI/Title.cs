using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    public void StartButton()
    {
        Debug.Log("Start");
        SceneManager.LoadScene("Stage_Inn");
    }

    public void LoadButton()
    {
        Debug.Log("Load");
    }

    public void QuitButton()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
