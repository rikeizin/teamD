using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleColl : MonoBehaviour
{

    GameObject mage;

    private void OnParticleCollision(GameObject other)
    {

        mage = GameObject.Find("Skeleton_Mage");

        mage.GetComponent<Skeleton_Mage>().Attack();
        

    }
}
