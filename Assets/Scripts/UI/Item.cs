using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "New Item/item")]
// filename �� ������ �����ϰ� �Ǹ� �⺻������ ������ �̸�
// menuName ����Ƽ ����-��Ŭ-Create- �޴��� ���� �̸�
public class Item : ScriptableObject
{
    public enum ItemType
    {
        Equipment,  //���
        Rune,       //��
    }

    public string itemName; // ���������̸�
    public ItemType itemType; //����������
    public Sprite itemImage; // �������� �̹���(�κ��丮�� �����̹���)
    public GameObject itemPrefab; //�������� ������(�����ۻ����� ������������)
}
