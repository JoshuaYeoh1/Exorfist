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
            if(dmg>0)
            {
                hp.Hit(dmg);

                color.FlashColor(.1f, true);

                if(hp.hp>0) // if still alive
                {
                    StartCoroutine(iframing());

                    //stun.Stun(speedDebuffMult, stunTime);
                }
                else Die();
            }

            Knockback(kbForce, contactPoint);
        }
    }

    IEnumerator iframing()
    {
        iframe=true;
        yield return new WaitForSeconds(iframeTime);
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
        iframe=true;
        // spawn ragdoll
        Destroy(gameObject);
    }
}
