using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour
{
    public GameObject gibbedVersion;

    public void spawnGibs()
    {
        if(gibbedVersion) Instantiate(gibbedVersion, transform.position, transform.rotation);
    }
}
