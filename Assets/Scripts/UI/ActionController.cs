using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionController : MonoBehaviour
{
    [SerializeField]
    private float range; //아이템 습득이가능한 최대거리

    private bool pickupActivated = false; //  아이템습득 가능할시 true

    private RaycastHit hitInfo; // 충돌체 정보 저장

    [SerializeField]
    private LayerMask layerMask; // 특정 레이어를가진 오브젝트에 대해서만 습득가능

    [SerializeField]
    private Text actionText; // 행동을 보여줄 텍스트

    [SerializeField]
    private Inventory theInventory;

    public static bool ShopActivated = false;

    void Update()
    {   
        CheckItem();
        TryAction();
    }

    private void TryAction()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            
            CheckItem();
            CanPickUp();
           
        }
    }

    private void CheckItem()
    {
        if (Physics.Raycast(new Vector3(transform.position.x ,transform.position.y+1 ,transform.position.z), transform.forward, out hitInfo, range, layerMask))
        {
            Debug.Log(hitInfo.transform.gameObject.name);
            if (hitInfo.transform.tag == "Rune")
            {
                ItemInfoAppear();
            }
        }
        else
            ItemInfoDisappear();
    }

    private void ItemInfoAppear()
    {
        pickupActivated = true;
        actionText.gameObject.SetActive(true);
        actionText.text = hitInfo.transform.GetComponent<ItemPickUp>().item.itemName + " 획득 " + "<color=yellow>" + "(E)" + "</color>";
    }

    private void ItemInfoDisappear()
    {
        pickupActivated = false;
        actionText.gameObject.SetActive(false);
    }

    private void CanPickUp()
    {
        if(pickupActivated)
        {
            if(hitInfo.transform != null)
            {
                Debug.Log(hitInfo.transform.GetComponent<ItemPickUp>().item.itemName + " 획득 했습니다.");
                theInventory.AcquireItem(hitInfo.transform.GetComponent<ItemPickUp>().item);
                Destroy(hitInfo.transform.gameObject);
                ItemInfoDisappear();
            }
        }
    }
}
