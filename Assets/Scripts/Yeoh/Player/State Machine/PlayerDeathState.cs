using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeathState : BaseState<PlayerStateMachine.PlayerStates>
{
    PlayerStateMachine stateMachine;

    public PlayerDeathState(PlayerStateMachine stateMachine) : base(PlayerStateMachine.PlayerStates.Death)
    {
        this.stateMachine = stateMachine;
    }

    public override void EnterState()
    {
        Debug.Log("Player state: " + stateMachine.GetCurrentState().StateKey);

        stateMachine.player.canMove=false;
        stateMachine.player.canTurn=false;
        stateMachine.player.canAttack=false;
        stateMachine.player.canBlock=false;
        stateMachine.player.canCast=false;
        stateMachine.player.canHurt=false;
        stateMachine.player.canStun=false;

        GameEventSystem.current.playerDeath();

        RandDeathAnim();
        Singleton.instance.SpawnPopUpText(stateMachine.player.popUpTextPos.position, "DEAD!", Color.red);
        //feedback.dieAnim(); // screen red
    }

    public override void UpdateState()
    {

    }

    public override void FixedUpdateState()
    {
        
    }

    public override void ExitState()
    {
        stateMachine.player.isAlive=true;
    }

    public override PlayerStateMachine.PlayerStates GetNextState() // Implement the logic to determine the next state from the this state
    {
        return StateKey;
    }

    void RandDeathAnim()
    {
        int i = Random.Range(1, 2);
        stateMachine.player.anim.CrossFade("death"+i, .1f, 2, 0);
    }
}
