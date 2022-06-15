using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{


    public string[] talkData;
    public Text talkText;
    public GameObject[] rune;
    public GameObject[] weapons;


    Player enterPlayer;

    public void Start()
    {
        enterPlayer = GameObject.FindObjectOfType<Player>();
    }

    public void ShopLoading()
    {
        //weapons = Resources.LoadAll<GameObject>("Prefab/Weapons");
        rune = Resources.LoadAll<GameObject>("Prefab/Rune");
    }

    public void Buy()
    {
        ShopLoading();

        int price = 100;
        if (price > enterPlayer.currentGold)
        {
            StopCoroutine(Talk());
            StartCoroutine(Talk());
            return;
        }

        enterPlayer.currentGold -= price;

        int per = Random.Range(0, 9);
        if (per < 9)
        {
            int itemPer = Random.Range(0, 9);
            Instantiate(rune[itemPer], transform.position + Vector3.right * 4.5f, transform.rotation);
        }
        //else
        //{
        //    int itemPer = Random.Range(0, 4);
        //    Instantiate(weapons[itemPer], transform.position + Vector3.right * 4.5f, transform.rotation);
        //}

    }

    IEnumerator Talk()
    {
        talkText.text = talkData[1];
        yield return new WaitForSeconds(2f);
        talkText.text = talkData[0];
    }
}