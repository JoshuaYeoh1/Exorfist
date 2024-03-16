using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="Attacks/Melee")]

public class AttackSO : ScriptableObject
{
    public string animName="jab";
    public string attackName="Light";
    public int hitboxIndex=0;
    public float hitboxActiveDuration=.1f;

    public float dash=2;
    public float dmg=1;
    public float dmgBlock=1;
    public float kbForce=2;
    public float speedDebuffMult=.3f;
    public float stunTime=.5f;

    public bool hasSweepingEdge=true;
    public bool doImpact=true;
    public bool doShake=true;
    public bool doHitstop=true;
    public bool doShockwave=true;
    public bool unparryable;
}
