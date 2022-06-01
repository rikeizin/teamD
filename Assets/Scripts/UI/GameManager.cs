using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int stage;
    public float playtime;
    public bool isBattle;

    public GameObject gameUI;
    public Text stageText;
    public Text playtimeText;

    void Update()
    {
        if (isBattle)
            playtime += Time.deltaTime;
    }

    void LateUpdate()
    {
        stageText.text = " Stage " + stage;
        int hour = (int)(playtime / 3600);
        int min = (int)((playtime - hour * 3600) / 60);
        int second = (int)(playtime % 60);
        playtimeText.text = string.Format("{0:00}", hour) + ":" + string.Format("{0:00}", min) + ":" + string.Format("{0:00}", second);
    }
}
