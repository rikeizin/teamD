using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopController: MonoBehaviour
{
    [SerializeField]
    private GameObject go_ShopBase;

    [SerializeField]
    private LayerMask layerMask;

    [SerializeField]
    private Text actionText;

    private RaycastHit hitInfo;

    [SerializeField]
    private float range;

    private bool pickupActivated = false;


    public static bool ShopActivated = false;

    void Update()
    {

        if (CheckShop())
        {
            ShopInfoAppear();
            if (Input.GetKeyDown(KeyCode.E))
            {
                ShopActivated = !ShopActivated;
                if (ShopActivated)
                    OpenShop();
                else
                    CloseShop();
            }
        }
        else
        {
            ShopInfoDisappear();
            CloseShop();
        }
    }

    private bool CheckShop()
    {
        if (Physics.Raycast(transform.position, transform.forward, out hitInfo, range, layerMask))
        {
            if (hitInfo.transform.tag == "Shop")
                return true;
        }
        return false;
    }

    private void OpenShop()
        {
            go_ShopBase.SetActive(true);
        }

    private void CloseShop()
        {
            go_ShopBase.SetActive(false);
        }


    private void ShopInfoAppear()
    {
        pickupActivated = true;
        actionText.gameObject.SetActive(true);
        actionText.text = hitInfo.transform  + " ¿­±â " + "<color=yellow>" + "(E)" + "</color>";
    }

    private void ShopInfoDisappear()
    {
        pickupActivated = false;
        actionText.gameObject.SetActive(false);
    }
}


