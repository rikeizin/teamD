using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class OptionUI : MonoBehaviour {

    [SerializeField]
    private GameObject settings = null;
    private OptionManager option;

    private void Start()
    {
        option = settings.GetComponent<OptionManager>();
    }

    public void OnEscape(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Scene scene = SceneManager.GetActiveScene();
            if (settings.activeSelf == false)
            {
                //if(scene.name != "TitleScene")
                //{
                    
                //}
                settings.SetActive(true);
            }
            else
            {
                option.OnClickBack();
            }
        }       
    }
}
