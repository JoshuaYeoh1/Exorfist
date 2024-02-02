using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public List<PlayerWeapon> hitboxes;

    public bool canLook=true, canMove=true;
    public bool isAttacking;
}
