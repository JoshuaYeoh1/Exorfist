using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtMidpoint : MonoBehaviour
{
    Player player;
    Transform enemyTr;
    Vector3 midpointXZ;

    public float panTime=1, middle=.5f;

    void Awake()
    {
        player=transform.root.GetComponent<Player>();
    }

    void FixedUpdate()
    {
        FindMidpoint();
        SmoothTowards(midpointXZ, panTime);
    }

    void FindMidpoint()
    {
        if(player.finder.target) enemyTr = player.finder.target.transform;
        else enemyTr = player.transform;

        Vector3 midpoint = Vector3.Lerp(player.transform.position, enemyTr.position, middle);

        midpointXZ = new Vector3(midpoint.x, transform.position.y, midpoint.z);
    }

    Vector3 velocity;

    void SmoothTowards(Vector3 pos, float moveTime)
    {
        transform.position = Vector3.SmoothDamp(transform.position, pos, ref velocity, moveTime);
    }
}
