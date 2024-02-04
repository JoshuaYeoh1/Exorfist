using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [HideInInspector] public PlayerStateMachine stateMachine;
    [HideInInspector] public PlayerMovement move;
    [HideInInspector] public ClosestObjectFinder finder;

    public List<PlayerWeapon> hitboxes;

    public bool isAlive=true, canLook=true, canMove=true, canAttack=true, canBlock=true, canStun=true;

    void Awake()
    {
        stateMachine=GetComponent<PlayerStateMachine>();
        move=GetComponent<PlayerMovement>();
        finder=GetComponent<ClosestObjectFinder>();
    }
}
