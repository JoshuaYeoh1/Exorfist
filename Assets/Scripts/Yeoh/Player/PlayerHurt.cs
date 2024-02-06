using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHurt : MonoBehaviour
{
    Player player;
    HPManager hp;
    OffsetMeshColor color;
    Rigidbody rb;
    PlayerStun stun;

    bool iframe;
    public float iframeTime=.5f;

    void Awake()
    {
        player=GetComponent<Player>();
        hp=GetComponent<HPManager>();
        color=GetComponent<OffsetMeshColor>();
        rb=GetComponent<Rigidbody>();
        stun=GetComponent<PlayerStun>();
    }

    public void Hit(float dmg, float kbForce, Vector3 contactPoint, float speedDebuffMult=.3f, float stunTime=.5f)
    {
        if(!iframe && player.isAlive)
        {
            if(dmg>0)
            {
                hp.Hit(dmg);

                color.FlashColor(.1f, true);

                if(hp.hp>0) // if still alive
                {
                    StartCoroutine(iframing());

                    stun.Stun(speedDebuffMult, stunTime);
                    
                    Singleton.instance.SpawnPopUpText(contactPoint, dmg.ToString(), Color.red);

                    Singleton.instance.HitStop();

                    // flash screen red
                }
                else Die();
            }
            
            Knockback(kbForce, contactPoint);

            //Singleton.instance.FadeTimeTo(float to, float time, float delay=0);

            //Singleton.instance.playSFX(Singleton.instance.sfxSubwoofer, transform.position, false);
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

            Singleton.instance.CamShake();
        }
    }

    void Die()
    {
        iframe=true;

        player.stateMachine.TransitionToState(PlayerStateMachine.PlayerStates.Death);

        RandDeathAnim();

        Singleton.instance.SpawnPopUpText(player.popUpTextPos.position, "DEAD!", Color.red);

        //feedback.dieAnim(); // screen red
    }

    void RandDeathAnim()
    {
        int i = Random.Range(1, 2);
        player.anim.CrossFade("death"+i, .1f, 2, 0);
    }

    public void SpawnRagdoll()
    {

    }

    void Update() // testing
    {
        if(Input.GetKeyDown(KeyCode.Delete)) Hit(1, 1, transform.position, .3f, 1);
    }
}
