using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using STAGE_MANAGEMENT;

public class ShopController : MonoBehaviour
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

    private void Start()
    {
        findactionText();
        findShop();
    }

    private void findactionText()
    {
        actionText = StageManager.Inst.transform.Find("MainUi 1").transform.Find("ShowText").transform.Find("actionText (1)").GetComponent<Text>();
    }

    private void findShop()
    {
        //go_ShopBase =
    }

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
        if (Physics.Raycast(new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), transform.forward, out hitInfo, range, layerMask))
        {
            if (hitInfo.transform.tag == "Player")
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
        actionText.text = hitInfo.transform + (" 열기 " + "<color=yellow>" + "(E)" + "</color>");
    }

    private void ShopInfoDisappear()
    {
        pickupActivated = false;
        actionText.gameObject.SetActive(false);
    }
}


