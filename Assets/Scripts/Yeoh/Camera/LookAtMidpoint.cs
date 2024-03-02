using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtMidpoint : MonoBehaviour
{
    public Player player;
    Transform enemyTr;
    TransformConstraint constraint;

    Vector3 midpoint;
    public Vector3 midpointOffset;

    public float panTime=1, middle=.5f;

    void Awake()
    {
        constraint=GetComponent<TransformConstraint>();
    }

    void FixedUpdate()
    {
        FindMidpoint();
        SmoothTowards(ref constraint.positionOffset, midpoint-player.transform.position, panTime);
    }

    void FindMidpoint()
    {
        if(player.target) enemyTr = player.target.transform;
        else enemyTr = player.transform;

        Vector3 midPos = Vector3.Lerp(player.transform.position, enemyTr.position, middle);

        midpoint = midPos+midpointOffset;
    }

    Vector3 velocity;

    void SmoothTowards(ref Vector3 selfPos, Vector3 targetPos, float moveTime)
    {
        selfPos = Vector3.SmoothDamp(selfPos, targetPos, ref velocity, moveTime);
    }
}
