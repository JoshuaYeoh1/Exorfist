using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHurt : MonoBehaviour
{
    Player player;
    HPManager hp;
    Rigidbody rb;
    PlayerStun stun;

    public bool iframe;
    public float iframeTime=.5f;

    public GameObject bloodVFXPrefab;

    void Awake()
    {
        player=GetComponent<Player>();
        hp=GetComponent<HPManager>();
        rb=GetComponent<Rigidbody>();
        stun=GetComponent<PlayerStun>();
    }

    public void Hurt(GameObject attacker, float dmg, float kbForce, Vector3 contactPoint, float speedDebuffMult=.3f, float stunTime=.5f)
    {
        if(!iframe && player.isAlive && player.canHurt)
        {
            DoIFraming(iframeTime, .5f, -.5f, -.5f); // flicker red

            Knockback(kbForce, contactPoint);

            GameEventSystem.current.OnHurt(gameObject, attacker, dmg, kbForce, contactPoint, speedDebuffMult, stunTime);

            hp.Hit(dmg);

            if(hp.hp>0) // if still alive
            {
                stun.Stun(speedDebuffMult, stunTime);

                // move to vfx manager later   
                // flash screen red
            }
            else Die(attacker, dmg, kbForce, contactPoint);





            // move to vfx manager later
            VFXManager.current.SpawnPopUpText(contactPoint, dmg.ToString(), Color.red);
            VFXManager.current.CamShake();
            VFXManager.current.HitStop();

            GameObject blood = Instantiate(bloodVFXPrefab, contactPoint, Quaternion.identity);
            blood.hideFlags = HideFlags.HideInHierarchy;

            // stay red screen
            //Singleton.instance.PlaySFX(Singleton.instance.sfxSubwoofer, transform.position, false);
        }
    }

    public void DoIFraming(float t, float r, float g, float b)
    {
        if(iFramingRt!=null) StopCoroutine(iFramingRt);
        iFramingRt = StartCoroutine(IFraming(t, r, g, b));
    }

    Coroutine iFramingRt;
    IEnumerator IFraming(float t, float r, float g, float b)
    {
        iframe=true;
        StartIFrameFlicker(r, g, b);

        yield return new WaitForSeconds(t);

        iframe=false;
        StopIFrameFlicker();
    }

    void StartIFrameFlicker(float r, float g, float b)
    {
        if(iFrameFlickeringRt!=null) StopCoroutine(iFrameFlickeringRt);
        iFrameFlickeringRt = StartCoroutine(IFrameFlickering(r, g, b));
    }

    Coroutine iFrameFlickeringRt;
    IEnumerator IFrameFlickering(float r, float g, float b)
    {
        while(true)
        {
            ModelManager.current.OffsetColor(gameObject, r, g, b);
            yield return new WaitForSecondsRealtime(.05f);
            ModelManager.current.RevertColor(gameObject);
            yield return new WaitForSecondsRealtime(.05f);
        }
    }

    void StopIFrameFlicker()
    {
        if(iFrameFlickeringRt!=null) StopCoroutine(iFrameFlickeringRt);
        ModelManager.current.RevertColor(gameObject);
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

    void Die(GameObject killer, float dmg, float kbForce, Vector3 contactPoint)
    {
        ModelManager.current.RevertColor(gameObject);
        
        player.CancelActions();

        GameEventSystem.current.OnDeath(gameObject, killer, dmg, kbForce, contactPoint);

        player.stateMachine.TransitionToState(PlayerStateMachine.PlayerStates.Death);
    }
}
