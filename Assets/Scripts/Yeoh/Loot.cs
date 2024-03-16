using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loot : MonoBehaviour
{
    Rigidbody rb;

    public string lootName="Chi";
    public int quantity=1;

    public float lootDelay=1;
    public bool destroyOnLoot=false;

    void Awake()
    {
        rb=GetComponent<Rigidbody>();
    }

    bool canLoot;

    void OnEnable()
    {
        if(lootDelay>0) StartCoroutine(LootDelaying());
    }

    IEnumerator LootDelaying()
    {
        canLoot=false;
        yield return new WaitForSeconds(lootDelay);
        canLoot=true;
    }

    void OnTriggerStay(Collider other)
    {
        if(!canLoot) return;

        if(!other.isTrigger)
        {
            Rigidbody otherRb = other.attachedRigidbody;

            if(otherRb) Pickup(other, otherRb);
        }
    }

    [HideInInspector] public Vector3 contactPoint;

    void Pickup(Collider other, Rigidbody otherRb)
    {
        GameObject looter = otherRb.gameObject;

        contactPoint = other.ClosestPointOnBounds(transform.position);

        LootInfo lootInfo = new LootInfo();

        lootInfo.lootName=lootName;
        lootInfo.quantity=quantity;
        lootInfo.contactPoint = contactPoint;

        GameEventSystem.Current.OnLoot(looter, lootInfo);

        if(destroyOnLoot)
        {
            Destroy(gameObject);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    public void Push(Vector3 force)
    {
        Vector3 randForce = new Vector3
        (
            Random.Range(-force.x, force.x),
            Random.Range(force.y*.25f, force.y*.5f),
            Random.Range(-force.z, force.z)
        );

        rb.AddForce(randForce, ForceMode.Impulse);
    }
}
