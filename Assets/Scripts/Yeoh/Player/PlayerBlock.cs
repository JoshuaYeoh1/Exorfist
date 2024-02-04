using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlock : MonoBehaviour
{
    Player player;
    PlayerMovement move;
    public Animator anim;

    public float blockCooldown=.5f, parryWindowTime=.2f, blockMoveSpeedMult=.3f;
    
    public bool isParrying, isBlocking;

    void Awake()
    {
        player=GetComponent<Player>();
        move=GetComponent<PlayerMovement>();
    }

    void Update()
    {
        CheckInput();

        anim.SetBool("isBlocking", isBlocking);
    }

    void CheckInput()
    {
        if(Input.GetKeyDown(KeyCode.Q)) OnBtnDown();
        if(Input.GetKeyUp(KeyCode.Q)) OnBtnUp();
    }

    bool pressingBtn;

    public void OnBtnDown()
    {
        pressingBtn=true;

        if(player.canBlock) Parry();
    }

    public void OnBtnUp()
    {
        pressingBtn=false;

        if(isBlocking) Unblock();
    }

    bool canBlock=true;

    void Parry()
    {
        if(canBlock)
        {
            canBlock=false;

            StartCoroutine(Parrying());

            RandParryAnim();

            move.moveSpeed *= blockMoveSpeedMult;

            player.stateMachine.TransitionToState(PlayerStateMachine.PlayerStates.Parry);

            anim.CrossFade("cancel", .1f, 2, 0); // cancel attack
        }
    }

    int parryAnim;

    void RandParryAnim()
    {
        string[] anims = {"parry1", "parry2"};

        if(parryAnim==0) parryAnim=1;
        else if(parryAnim==1) parryAnim=0;

        anim.CrossFade(anims[parryAnim], .1f, 3, 0);
    }

    IEnumerator Parrying()
    {
        isParrying=true;
        yield return new WaitForSeconds(parryWindowTime);
        isParrying=false;

        if(pressingBtn) Block();

        else Unblock();
    }

    void Block()
    {
        isBlocking=true;

        player.stateMachine.TransitionToState(PlayerStateMachine.PlayerStates.Block);
    }

    void Unblock()
    {
        isBlocking=false;

        move.moveSpeed = move.defMoveSpeed;

        if(!canBlock) StartCoroutine(BlockCoolingDown());

        player.stateMachine.TransitionToState(PlayerStateMachine.PlayerStates.Idle);
    }

    IEnumerator BlockCoolingDown()
    {
        yield return new WaitForSeconds(blockCooldown);
        canBlock=true;
    }
}
