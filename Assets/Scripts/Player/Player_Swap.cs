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
    public int CurrentGold = 0;
    public AudioSource p_audio;
    public AudioClip p_gold;
    public AudioClip p_Item;

    [HideInInspector]
    PlayerController playerController = null;
    [HideInInspector]
    GameObject nearobject;
    [HideInInspector]
    public Animator animator = null;
    [HideInInspector]
    public GameObject equipWeapons;
    [HideInInspector]
    public Player player = null;
    [HideInInspector]
    public int equipWeaponIndex = -1;
    [HideInInspector]
    public int runeIndex = 0;

    private int Gold;
    private int Arrow;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        player = GetComponent<Player>();
        playerController = GetComponent<PlayerController>();
        p_audio = GetComponent<AudioSource>();
    }

    public void Swap(InputAction.CallbackContext context)
    {

        float input = context.ReadValue<float>();
        if (context.started)
        {
            if (!playerController.IsAttackAnimating())
            {
                if (equipWeapons != null)
                    equipWeapons.SetActive(false);
                switch (input)
                {
                    case 1:
                        equipWeaponIndex = 0;
                        equipWeapons = Weapons[equipWeaponIndex];
                        animator.SetInteger("EquipState", 1);
                        if (hasWeapons[equipWeaponIndex + 1])
                            equipWeapons = Weapons[equipWeaponIndex + 1];
                        equipWeapons.SetActive(true);
                        break;
                    case 2:
                        equipWeaponIndex = 1;
                        equipWeapons = Weapons[equipWeaponIndex + 1];
                        animator.SetInteger("EquipState", 11);
                        if (!hasWeapons[equipWeaponIndex + 1])
                            return;
                        equipWeapons.SetActive(true);
                        break;
                    case 3:
                        equipWeaponIndex = 2;
                        equipWeapons = Weapons[equipWeaponIndex + 1];
                        animator.SetInteger("EquipState", 21);
                        if (!hasWeapons[equipWeaponIndex + 1])
                            return;
                        equipWeapons.SetActive(true);
                        break;
                    case 4:
                        equipWeaponIndex = 3;
                        equipWeapons = Weapons[equipWeaponIndex + 1];
                        animator.SetInteger("EquipState", 31);
                        if (!hasWeapons[equipWeaponIndex + 1])
                            return;
                        equipWeapons.SetActive(true);
                        break;
                }
            }
            
        }
    }

    public void GainItem(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (nearobject != null)
            {
                if (nearobject.tag == "Rune")
                {
                    Rune rune = nearobject.GetComponent<Rune>();
                    runeIndex = rune.value;
                    hasRunes[runeIndex]++;
                    rune.GainRunes();
                    p_audio.clip = p_Item;
                    p_audio.Play();
                    Destroy(nearobject);
                }

                if (nearobject.tag == "Weapons")
                {
                    Weapons weapons = nearobject.GetComponent<Weapons>();
                    int weaponIndex = weapons.value;
                    hasWeapons[weaponIndex] = true;
                    p_audio.clip = p_Item;
                    p_audio.Play();
                    Destroy(nearobject);
                }
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Gold")
        {
            Gold = Random.Range(1, 5);
            player.currentGold += Gold + player.goldUp;
            p_audio.clip = p_gold;
            p_audio.Play();
            Destroy(other.gameObject);
        }
        if(other.tag == "Arrow")
        {
            Arrow = Random.Range(1, 5);
            player.arrow += Arrow;
            p_audio.clip = p_Item;
            p_audio.Play();
            Destroy(other.gameObject);
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