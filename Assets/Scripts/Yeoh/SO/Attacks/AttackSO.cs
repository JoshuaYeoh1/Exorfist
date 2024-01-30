using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="Attacks/Light Attack")]

public class AttackSO : ScriptableObject
{
    public AnimatorOverrideController animOV;

    public float dash=2, damage=1, knockback=2;

    public string animName="jab";
    
    public int hitboxIndex=0;
}
