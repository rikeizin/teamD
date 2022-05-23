using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void ButtonDelegate();
public class PopupManager : DontDestroy<PopupManager>
{
    [SerializeField]
    GameObject m_popupOkCancelPrefab;
    [SerializeField]
    GameObject m_popupOkPrefab;
    
    List<GameObject> m_popupList = new List<GameObject>();
    public void OpenPopupOkCancel(string subject, string body, ButtonDelegate okBtnDel = null, ButtonDelegate cancelBtnDel = null, string okBtnStr = "Ok", string cancelBtnStr = "Cancel")
    {
        var obj = Instantiate(m_popupOkCancelPrefab) as GameObject;
        obj.transform.SetParent(transform);
        obj.transform.localPosition = Vector3.zero;

        
        var popup = obj.GetComponentInChildren<PopupOkCancel>();
        popup.SetUI(subject, body, okBtnDel, cancelBtnDel, okBtnStr, cancelBtnStr);                
        
        m_popupList.Add(obj);
    }

    public void OpenPopupOk(string subject, string body, ButtonDelegate okBtnDel = null, string okBtnStr = "Ok")
    {
        var obj = Instantiate(m_popupOkPrefab) as GameObject;
        obj.transform.SetParent(transform);
        obj.transform.localPosition = Vector3.zero;

        var popup = obj.GetComponent<PopupOk>();
        popup.SetUI(subject, body, okBtnDel, okBtnStr);

        m_popupList.Add(obj);
    }
    public bool IsOpenPopup()
    {
        return m_popupList.Count == 0 ? false : true;
    }
    public void ClosePopup()
    {
        if(m_popupList.Count>0)
        {
            Destroy(m_popupList[m_popupList.Count - 1]);
            m_popupList.Remove(m_popupList[m_popupList.Count - 1]);
        }
    }
    public void ClearPopup()
    {
        for(int i=0;i<m_popupList.Count;i++)
        {
            Destroy(m_popupList[i]);
        }
        m_popupList.Clear();
    }
    protected override void OnStart()
    {
        m_popupOkCancelPrefab = Resources.Load("Prefab/UI/Popup/Popup_OKCancel") as GameObject;
        m_popupOkPrefab = Resources.Load("Prefab/UI/Popup/Popup_Ok") as GameObject;
    }
}
