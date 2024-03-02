using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputBuffer : MonoBehaviour
{
    PlayerCombat combat;
    PlayerBlock block;
    PlayerAOE aoe;
    PlayerLaser laser;
    PlayerHeal heal;

    public float inputBufferTime=.3f;
    
    [HideInInspector] public float lastPressedLightAttack=-1, lastPressedHeavyAttack=-1, lastPressedBlock=-1;
    [HideInInspector] public float lastPressedAOE=-1, lastPressedLaser=-1, lastPressedHeal=-1;

    void Awake()
    {
        combat=GetComponent<PlayerCombat>();
        block=GetComponent<PlayerBlock>();
        aoe=GetComponent<PlayerAOE>();
        laser=GetComponent<PlayerLaser>();
        heal=GetComponent<PlayerHeal>();
    }

    void Update()
    {
        KeyboardInput(); // temp for testing on PC
        CheckInputBuffer();
    }

    public void LightAttack()
    {
        lastPressedLightAttack = Time.time;
    }
    public void HeavyAttack()
    {
        lastPressedHeavyAttack = Time.time;
    }
    public void BlockDown()
    {
        lastPressedBlock = Time.time;

        block.pressingBtn=true;
    }
    public void BlockUp()
    {
        if(block.isBlocking) block.Unblock();

        block.pressingBtn=false;
    }

    public void AOE()
    {
        lastPressedAOE = Time.time;
    }
    public void Laser()
    {
        lastPressedLaser = Time.time;
    }
    public void Heal()
    {
        lastPressedHeal = Time.time;
    }

    void CheckInputBuffer()
    {
        if(Time.time-lastPressedLightAttack < inputBufferTime)
        {
            combat.Attack("light");
        }
        if(Time.time-lastPressedHeavyAttack < inputBufferTime)
        {
            combat.Attack("heavy");
        }
        if(Time.time-lastPressedBlock < inputBufferTime)
        {
            block.Parry();
        }

        if(Time.time-lastPressedAOE < inputBufferTime)
        {
            aoe.StartCast();
        }
        if(Time.time-lastPressedLaser < inputBufferTime)
        {
            laser.StartCast();
        }
        if(Time.time-lastPressedHeal < inputBufferTime)
        {
            heal.StartCast();
        }
    }

    void KeyboardInput()
    {
        if(Input.GetKeyDown(KeyCode.Space)) LightAttack();
        if(Input.GetKeyDown(KeyCode.LeftAlt)) HeavyAttack();
        if(Input.GetKeyDown(KeyCode.Q)) BlockDown();
        if(Input.GetKeyUp(KeyCode.Q)) BlockUp();

        if(Input.GetKeyUp(KeyCode.Z)) AOE();
        if(Input.GetKeyUp(KeyCode.X)) Laser();
        if(Input.GetKeyUp(KeyCode.C)) Heal();
    }
    
}
