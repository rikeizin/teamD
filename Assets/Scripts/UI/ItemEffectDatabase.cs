using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class ItemEffectDatabase : MonoBehaviour
{
    [SerializeField]
    private QuickSlotController theQuickSlotController;

    public void IsActivatedquickSlot(int _num)
    {
        theQuickSlotController.IsActivatedQuickSlot(_num);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
