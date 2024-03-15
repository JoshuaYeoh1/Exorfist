using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2 : MonoBehaviour
{
    public void SlamWindupEvent()
    {
        GameEventSystem.Current.OnAbilityCasting(gameObject, "Enemy2Slam");
    }

    public void SlamEvent()
    {
        GameEventSystem.Current.OnAbilityCast(gameObject, "Enemy2Slam");
        GameEventSystem.Current.EnemySoundEventPlay(gameObject.transform, "SlamImpact");
        GameEventSystem.Current.EnemySoundEventPlay(gameObject.transform, "Enemy2 SlamImpactVoice");
    }

    public void PlayLeapSound()
    {
        GameEventSystem.Current.EnemySoundEventPlay(gameObject.transform, "Enemy2 SlamLeapVoice");
    }
}
