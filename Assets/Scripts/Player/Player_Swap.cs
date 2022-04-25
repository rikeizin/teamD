using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Swap : MonoBehaviour
{
    public GameObject[] Runes;
    public int[] hasRunes;
    public GameObject[] Weapons;
    public bool[] hasWeapons;

    GameObject nearobject;
    GameObject equipWeapons;
    int weaponIndex = -1;
    int equipWeaponIndex = -1;

    public void Swap(InputAction.CallbackContext context)
    {

        float input = context.ReadValue<float>();
        if (context.started)
        {
            if(equipWeapons != null)
                equipWeapons.SetActive(false);
            switch (input)
            {
                case 1:
                        weaponIndex = 0;
                        equipWeapons = Weapons[weaponIndex];
                    if (hasWeapons[weaponIndex + 1])
                        equipWeapons = Weapons[weaponIndex + 1];
                    equipWeapons.SetActive(true);
                    break;
                case 2:
                        weaponIndex = 1;
                        equipWeapons = Weapons[weaponIndex + 1];
                    if (!hasWeapons[weaponIndex+1])
                        return;
                    equipWeapons.SetActive(true);
                    break;
                case 3:
                        weaponIndex = 2;
                        equipWeapons = Weapons[weaponIndex + 1];
                    if (!hasWeapons[weaponIndex+1])
                        return;
                    equipWeapons.SetActive(true);
                    break;
                case 4:
                        weaponIndex = 3;
                        equipWeapons = Weapons[weaponIndex + 1];
                    if (!hasWeapons[weaponIndex+1])
                        return;
                    equipWeapons.SetActive(true);
                    break;
            }
        }
    }

    public void interation(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (nearobject != null)
            {
                if(nearobject.tag == "Rune")
                {
                    Rune rune = nearobject.GetComponent<Rune>();
                    int runeIndex = rune.value;
                    hasRunes[runeIndex]++;
                
                    Destroy(nearobject);
                }

                if (nearobject.tag == "Weapons")
                {
                    Weapons weapons = nearobject.GetComponent<Weapons>();
                    int weaponIndex = weapons.value;
                    hasWeapons[weaponIndex] = true;

                    Destroy(nearobject);
                }
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Rune" || other.tag == "Weapons")
        {
            nearobject = other.gameObject;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Rune" || other.tag == "Weapons")
        {
            nearobject = null;
        }
    }
}