using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public GameObject[] itemobj;
    public int[] itemPrice;
    public Transform[] itempos;
    public Text talkText;

    Player enterPlayer;

    public void Buy(int index)
    {
        int price = itemPrice[index];
        if(price > enterPlayer.gold)
        {
            return;
        }
    }
}
