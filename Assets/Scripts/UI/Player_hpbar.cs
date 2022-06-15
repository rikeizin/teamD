using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Player_hpbar : MonoBehaviour
{
    public Slider hpBar;
    public float maxHp = 100f;
    public float currentHp = 100f;
    Player player;

    void Start()
    {
        player = GameObject.FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        hpBar.value = Mathf.Lerp(hpBar.value, player.hp / player.maxHp, Time.deltaTime * 5f);
    }
}
