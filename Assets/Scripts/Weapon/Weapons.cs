using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapons : MonoBehaviour
{
    public enum Type {Sword, Wand, Mace, Bow, Arrow};
    public Type type;
    public int value;
    public int turnSpeed;
        
    GameObject weapon;

    private void Awake()
    {
        weapon = GetComponent<GameObject>();
    }
    private void Update()
    {
        Turn();
    }
    private void Turn()
    {
        transform.Rotate(Vector3.up * turnSpeed * Time.deltaTime);
    }
}