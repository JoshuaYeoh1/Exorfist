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
        if(other.attachedRigidbody && canLoot)
        {
            GameObject looter = other.attachedRigidbody.gameObject;

            GameEventSystem.Current.OnLoot(looter, lootName, quantity);

            if(destroyOnLoot)
            {
                Destroy(gameObject);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
    }

    public void Push(Vector3 force)
    {
        Vector3 randForce = new Vector3
        (
            Random.Range(-force.x, force.x),
            Random.Range(force.y*.5f, force.y),
            Random.Range(-force.z, force.z)
        );

        rb.AddForce(randForce, ForceMode.Impulse);
    }
}
