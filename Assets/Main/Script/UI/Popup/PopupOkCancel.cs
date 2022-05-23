using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupOkCancel : MonoBehaviour
{
    [SerializeField]
    Text m_subjectText;
    [SerializeField]
    Text m_bodyText;
    [SerializeField]
    Text m_okBtnText;
    [SerializeField]
    Text m_cancelBtnText;


    ButtonDelegate m_okBtnDel;
    ButtonDelegate m_cancelBtnDel;
    public void SetUI(string subject, string body, ButtonDelegate okBtnDel = null, ButtonDelegate cancelBtnDel = null, string okBtnStr = "OK", string cancelBtnStr = "Cancel")
    {        
        m_subjectText.text = subject;
        m_bodyText.text = body;
        m_okBtnText.text = okBtnStr;
        m_cancelBtnText.text = cancelBtnStr;
        m_okBtnDel = okBtnDel;
        m_cancelBtnDel = cancelBtnDel;
    }
    
    public void OnPressOK()
    {
        if (m_okBtnDel != null)
        {
            m_okBtnDel();
        }
        else
        {
            PopupManager.Instance.ClosePopup();
        }
    }
    public void OnPressCancel()
    {
        if (m_cancelBtnDel != null)
        {
            m_cancelBtnDel();
        }
        else
        {
            PopupManager.Instance.ClosePopup();
        }
    }
}
