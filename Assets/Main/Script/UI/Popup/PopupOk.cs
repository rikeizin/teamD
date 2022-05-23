using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupOk : MonoBehaviour
{
    [SerializeField]
    Text m_subjectText;
    [SerializeField]
    Text m_bodyText;
    [SerializeField]
    Text m_okBtnText;
    

    ButtonDelegate m_okBtnDel;
    public void SetUI(string subject, string body, ButtonDelegate okBtnDel = null, string okBtnStr = "OK")
    {
        m_subjectText.text = subject;
        m_bodyText.text = body;
        m_okBtnText.text = okBtnStr;
        m_okBtnDel = okBtnDel;
    }
    public void OnPressOK()
    {
        if(m_okBtnDel != null)
        {
            m_okBtnDel();
        }
        else
        {
            PopupManager.Instance.ClosePopup();
        }
    }
}
