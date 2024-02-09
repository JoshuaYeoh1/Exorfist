using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputBuffer : MonoBehaviour
{
    PlayerCombat combat;
    PlayerBlock block;

    void Awake()
    {
        combat=GetComponent<PlayerCombat>();
        block=GetComponent<PlayerBlock>();
    }

    public float inputBufferTime=.3f;
    
    [HideInInspector] public float lastPressedLightAttack=-1, lastPressedHeavyAttack=-1, lastPressedBlock=-1;

    public void OnBtnDownLightAttack()
    {
        lastPressedLightAttack = Time.time;
    }

    public void OnBtnDownHeavyAttack()
    {
        lastPressedHeavyAttack = Time.time;
    }

    public void OnBtnDownBlock()
    {
        lastPressedBlock = Time.time;

        block.pressingBtn=true;
    }
    public void OnBtnUpBlock()
    {
        block.OnBtnUp();

        block.pressingBtn=false;
    }

    void Update()
    {
        if(Time.time-lastPressedLightAttack < inputBufferTime)
        {
            combat.OnBtnDown("light");
        }

        if(Time.time-lastPressedHeavyAttack < inputBufferTime)
        {
            combat.OnBtnDown("heavy");
        }

        if(Time.time-lastPressedBlock < inputBufferTime)
        {
            block.OnBtnDown();
        }

        KeyboardInput(); // temp for testing on PC
    }

    void KeyboardInput()
    {
        if(Input.GetKeyDown(KeyCode.Space)) OnBtnDownLightAttack();

        if(Input.GetKeyDown(KeyCode.LeftAlt)) OnBtnDownHeavyAttack();

        if(Input.GetKeyDown(KeyCode.Q)) OnBtnDownBlock();
        if(Input.GetKeyUp(KeyCode.Q)) OnBtnUpBlock();
    }
}
