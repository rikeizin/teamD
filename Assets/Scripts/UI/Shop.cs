using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{


    public GameObject[] itemobj;
    public int[] itemPrice;
    public Transform[] itempos;
    public string[] talkData;
    public Text talkText;

    Player enterPlayer;



    public void ShopLoading()
    {
        var prefab = Resources.Load<GameObject>("Prefabs/Weapons Prepabs");

        for (int i = 0; i < 3; i++)
        {
            var go = Instantiate(prefab);
            var Weapons = Random.Range(0, 5);
            var Rune = Random.Range(0, 9);
        }
    }

    public void Buy(int index)
    {
        int price = itemPrice[3];
        if (price > enterPlayer.gold)
        {
            StopCoroutine(Talk());
            StartCoroutine(Talk());
            return;
        }

        enterPlayer.gold -= price;
        Vector3 ranVec = Vector3.right * Random.Range(-3, 3)
                        + Vector3.forward * Random.Range(-3, 3);
        Instantiate(itemobj[index], itempos[index].position + ranVec, itempos[index].rotation);

    }

    IEnumerator Talk()
    {
        talkText.text = talkData[1];
        yield return new WaitForSeconds(2f);
        talkText.text = talkData[0];

    }
}