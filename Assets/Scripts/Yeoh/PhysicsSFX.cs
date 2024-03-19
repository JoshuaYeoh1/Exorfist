using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsSFX : MonoBehaviour
{
    public string type;
    bool sndCool;

    void OnCollisionEnter(Collision other)
    {
        if(!sndCool)
        {
            sndCool=true;
            Invoke("sndCooldown",Random.Range(.5f, 1f));
            GameEventSystem.Current.OnFootstep(gameObject, type, transform);
        }
    }

    void sndCooldown()
    {
        sndCool=false;
    }
}
