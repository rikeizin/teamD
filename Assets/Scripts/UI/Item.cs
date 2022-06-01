using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "New Item/item")]
// filename 이 에셋을 생성하게 되면 기본적으로 지어질 이름
// menuName 유니티 에셋-우클-Create- 메뉴에 보일 이름
public class Item : ScriptableObject
{
    public enum ItemType
    {
        Equipment,  //장비
        Rune,       //룬
    }

    public string itemName; // 아이템의이름
    public ItemType itemType; //아이템유형
    public Sprite itemImage; // 아이템의 이미지(인벤토리에 보일이미지)
    public GameObject itemPrefab; //아이템의 프리팹(아이템생성시 프리팹으로찍어냄)
}
