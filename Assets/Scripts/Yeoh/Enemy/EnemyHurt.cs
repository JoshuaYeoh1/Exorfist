using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHurt : MonoBehaviour
{
    HPManager hp;
    ChangeMeshColor color;

    bool iframe;
    public float iframeTime=.1f;

    void Awake()
    {
        hp=GetComponent<HPManager>();
        color=GetComponent<ChangeMeshColor>();
    }

    public void Hit(float dmg)
    {
        if(!iframe)
        {
            hp.Hit(dmg);

            if(hp.hp>0)
            {
                StartCoroutine(iframing());

                color.FlashColor(.1f);
            }
            else Die();
        }
    }

    IEnumerator iframing()
    {
        iframe=true;
        yield return new WaitForSeconds(iframeTime);
        iframe=false;
    }

    void Die()
    {
        iframe=true;
        // spawn ragdoll
        Destroy(gameObject);
    }
}
