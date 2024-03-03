using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="Attacks/Melee")]

public class AttackSO : ScriptableObject
{
    //public AnimatorOverrideController animOV;

    public float dash=2, damage=1, knockback=2, hitboxActiveDuration=.2f;

    public string animName="jab";

    public int hitboxIndex=0;

    public bool hasSweepingEdge;
}
