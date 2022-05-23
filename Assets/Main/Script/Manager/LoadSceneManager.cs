using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadSceneManager : DontDestroy<LoadSceneManager>
{
    /// <summary>
    /// 요거 CI는 그냥 Title이랑 합치고 지워도 괜찮아용
    /// </summary>
  public enum eSceneState
    {
        None = -1,
        CI,
        Title,
        Lobby,
        Game
    }

    //로딩화면 오브젝트(씬 전환할때만 켜짐)
    [SerializeField]
    GameObject m_loadingObj;

    Slider m_loadingBar;
    //비동기적으로 씬 전환(코루틴)
    AsyncOperation m_loadSceneState;
    //현재 씬 == 타이틀시작
    eSceneState m_state = eSceneState.Title;
    eSceneState m_loadState = eSceneState.None;

    //현재 스테이지 정보 불러오기
    public eSceneState GetState()
    {
        return m_state;
    }

    /// <summary>
    /// 비동기적 씬 변경 함수 ABC 스크립트에 사용하는거 적어뒀음
    /// </summary>
    public void LoadSceneAsync(eSceneState sceneState)
    {
        if (m_loadState != eSceneState.None) return;

        m_loadState = sceneState;
        m_loadSceneState = SceneManager.LoadSceneAsync(sceneState.ToString());
        if(m_loadingObj != null)
        {
            m_loadingObj.SetActive(true);
            m_loadingBar.value = 0f;
        }
        
    }

    protected override void OnStart()
    {
        m_loadingBar = GetComponentInChildren<Slider>();
        m_loadingObj.SetActive(false);
    }

    void Update()
    {
        if (m_loadSceneState != null)
        {
            //로딩이 완료되었는지 체크
            if (m_loadSceneState.isDone)
            {
                m_state = m_loadState;
                m_loadState = eSceneState.None;
                m_loadSceneState = null;
                m_loadingBar.value = 1f;
                m_loadingObj.SetActive(false);
                PopupManager.Instance.ClearPopup();
            }
            else
            {
                //progress <- 로딩 진행 퍼센트 float 반환
                m_loadingBar.value = m_loadSceneState.progress;
            }
        }

        //팝업창
        if (Input.GetKeyDown(KeyCode.Escape))
        {           
            //열려있으면 닫아라
            if (PopupManager.Instance.IsOpenPopup())
            {
                PopupManager.Instance.ClosePopup();
            }
            else
            {            
                switch (m_state)
                {
                    case eSceneState.Title:                        
                        PopupManager.Instance.OpenPopupOkCancel("Notice", "게임을 종료하시겠습니까?", () =>
                        {                            
#if UNITY_EDITOR
                        UnityEditor.EditorApplication.isPlaying = false;
#else
                        Application.Quit();
#endif
                    }, null, "예", "아니요");
                        break;
                    case eSceneState.Lobby:
                        PopupManager.Instance.OpenPopupOkCancel("Notice", "타이틀 화면으로 돌아가시겠습니까?", () =>
                        {
                            LoadSceneAsync(eSceneState.Title);
                        }, null, "예", "아니오");
                        break;
                    case eSceneState.Game:
                        PopupManager.Instance.OpenPopupOkCancel("Notice", "로비 화면으로 돌아가시겠습니까?", () =>
                        {
                            LoadSceneAsync(eSceneState.Lobby);
                        }, null, "예", "아니오");
                        break;
                }
            }
        }
    }
}
