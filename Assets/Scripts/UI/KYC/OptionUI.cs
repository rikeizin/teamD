using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class OptionUI : MonoBehaviour {

    [SerializeField]
    private GameObject settings = null;

    public void OnEscape(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if(settings.activeSelf == false)
            {
                settings.SetActive(true);
                Cursor.lockState = CursorLockMode.Confined;
                Time.timeScale = 0;
            }
            else
            {
                settings.SetActive(false);
                Cursor.lockState = CursorLockMode.None;
                Time.timeScale = 1;
            }
        }       
    }
}
