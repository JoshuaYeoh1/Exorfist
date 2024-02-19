using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHurt : MonoBehaviour
{
    HPManager hp;
    OffsetMeshColor color;
    Rigidbody rb;

    bool iframe;
    public float iframeTime=.1f;

    void Awake()
    {
        hp=GetComponent<HPManager>();
        color=GetComponent<OffsetMeshColor>();
        rb=GetComponent<Rigidbody>();
    }

    public void Hit(float dmg, float kbForce, Vector3 contactPoint, float speedDebuffMult=.3f, float stunTime=.5f)
    {
        if(!iframe)
        {
            DoIFraming(iframeTime);

            Knockback(kbForce, contactPoint);

            color.FlashColor(.1f, true);

            hp.Hit(dmg);

            if(hp.hp>0) // if still alive
            {
                //stun.Stun(speedDebuffMult, stunTime);

                Singleton.instance.SpawnPopUpText(contactPoint, dmg.ToString(), Color.white);
            }
            else Die();
        }
    }

    public void DoIFraming(float t)
    {
        StartCoroutine(iframing(t));
    }
    IEnumerator iframing(float t)
    {
        iframe=true;
        yield return new WaitForSeconds(t);
        iframe=false;
    }

    public void Knockback(float force, Vector3 contactPoint)
    {
        if(force>0)
        {
            Vector3 kbVector = rb.transform.position - contactPoint;
            kbVector.y = 0;

            rb.velocity = new Vector3(0, rb.velocity.y, 0);
            rb.AddForce(kbVector.normalized * force, ForceMode.Impulse);
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }

    public void SpawnRagdoll()
    {

    }
}
