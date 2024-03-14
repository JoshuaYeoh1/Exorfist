using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    [Header("Player AudioClips")]
    [SerializeField] private AudioClip[] SFXClipsPlayer;

    [Header("Enemy AudioClips")]
    [SerializeField] private AudioClip[] SFXClipsEnemies;

    private AudioClip[] SFXClipsEnvironment;

    private void Start()
    {
        GameEventSystem.Current.ParryEvent += OnParryEvent;
    }

    private void OnParryEvent(GameObject victim, GameObject attacker, HurtInfo info)
    {
        if(victim != null && attacker != null)
        {
            switch (victim.tag)
            {
                case "Player":
                    //AudioManager.Current?.PlaySFX(SFXClipsPlayer)
                    break;
                default: break;

            }
        }
    }
}
