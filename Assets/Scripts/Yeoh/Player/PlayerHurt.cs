using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHurt : MonoBehaviour
{
    Player player;
    HPManager hp;
    OffsetMeshColor color;
    public Animator anim;

    bool iframe;
    public float iframeTime=.5f;

    void Awake()
    {
        player=GetComponent<Player>();
        hp=GetComponent<HPManager>();
        color=GetComponent<OffsetMeshColor>();
    }

    void Update() // testing
    {
        if(Input.GetKeyDown(KeyCode.Delete)) Hit(1);
    }

    public void Hit(float dmg)
    {
        if(!iframe)
        {
            hp.Hit(dmg);

            color.FlashColor(.1f);

            if(hp.hp>0)
            {
                StartCoroutine(iframing());

                //if(hp.hp>dmg) feedback.hurtAnim(); // flash screen red
            }
            else Die();

            //Singleton.instance.playSFX(Singleton.instance.sfxSubwoofer, transform.position, false);
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
        gameObject.layer=0;

        player.stateMachine.TransitionToState(PlayerStateMachine.PlayerStates.Death);

        RandDeathAnim();

        //feedback.dieAnim(); // screen red
    }

    void RandDeathAnim()
    {
        string[] anims = {"death"};

        int i = Random.Range(0, anims.Length);

        anim.CrossFade(anims[i], .1f, 2, 0);
    }

    public void SpawnRagdoll()
    {

    }
}
