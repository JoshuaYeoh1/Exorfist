using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="Attacks/Melee")]

public class AttackSO : ScriptableObject
{
    public float dash=2, hitboxActiveDuration=.1f;
    public string animName="jab";
    public int hitboxIndex=0;

    public float dmg=1, dmgBlock=1, kbForce=2, speedDebuffMult=.3f, stunTime=.5f;
    public bool hasSweepingEdge=true, unparryable;
    public bool doImpact=true, doShake=true, doHitstop=true, doShockwave=true;
}
