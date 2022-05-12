using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace STAGE_MANAGEMENT
{
    public class BGMExeManager : MonoBehaviour
    {
        AudioSource audioByCAM = null;

        AudioClip[] innBGMs = null;
        AudioClip[] normalStageBGMs = null;
        AudioClip[] bossStageBGMs = null;

        
        private void Awake()
        {
            innBGMs = Resources.LoadAll<AudioClip>("Sounds/BGM/InnBGM");
            normalStageBGMs = Resources.LoadAll<AudioClip>("Sounds/BGM/NomarlBGM");
            bossStageBGMs = Resources.LoadAll<AudioClip>("Sounds/BGM/BossBGM");
        }


        public void AddAudioToCAM()
        {
            audioByCAM = Camera.main.gameObject.AddComponent<AudioSource>();
            if (audioByCAM.isPlaying)
            {
                audioByCAM.Stop();
            }
        }

        public void PlayInnBGM()
        {
            Debug.Log("여관 BGM OnPlay");
            audioByCAM.Stop();
            audioByCAM.clip = innBGMs[Random.Range(0, innBGMs.Length)];
            audioByCAM.Play();
        }

        public void PlayNormalStageBGM()
        {
            Debug.Log("일반 BGM OnPlay");
            audioByCAM.Stop();
            audioByCAM.clip = normalStageBGMs[Random.Range(0, normalStageBGMs.Length)];
            audioByCAM.Play();
        }

        public void PlayBossStageBGM()
        {
            Debug.Log("보스 BGM OnPlay");
            audioByCAM.Stop();
            audioByCAM.clip = bossStageBGMs[Random.Range(0, bossStageBGMs.Length)];
            audioByCAM.Play();
        }
    }
}

