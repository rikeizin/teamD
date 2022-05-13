using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    //public GameObject hitEffect = null;
    Queue<IBattle> hitTarget = new Queue<IBattle>(16);
    Player player = null;

    private void Awake()
    {
        player = FindObjectOfType<Player>();
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log($"target2 : {other.gameObject.name}");
        if (player.IsAttack && other.CompareTag("Enemy")
            && other.gameObject != player.gameObject)
        {
            Debug.Log($"target : {other.gameObject.transform.name}");
            other.GetComponent<MonsterController>().Hit();
            IBattle battle = other.GetComponentInParent<IBattle>();
            if (battle != null)
            {
                Vector3 hitPoint = this.transform.position - this.transform.up * 0.8f;
                //Vector3 effectPoint = other.ClosestPoint(hitPoint);
                //Instantiate(hitEffect, effectPoint, Quaternion.identity);
                //GameObject obj;
                //ParticleSystem.MainModule mainModule = obj.GetComponent<ParticleSystem>().main;
                //mainModule.stopAction = ParticleSystemStopAction.Destroy;
                hitTarget.Enqueue(battle);
            }
        }
    }

    public IBattle GetHitTarget()
    {
        return hitTarget.Dequeue();
    }

    public int HitTargetCount()
    {
        return hitTarget.Count;
    }
}
