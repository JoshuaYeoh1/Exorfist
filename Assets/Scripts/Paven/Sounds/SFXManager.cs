using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEditor.ShaderGraph.Drawing.Inspector.PropertyDrawers;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    [Header("Player AudioClips")]
    [SerializeField] private AudioClip[] SFXClipsPlayer;

    [Header("Enemy AudioClips")]
    [Header("Enemy1")]
    [SerializeField] private AudioClip[] Enemy1HurtClips;
    [SerializeField] private AudioClip Enemy1Death;

    [Header("Enemy2")]
    [SerializeField] private AudioClip[] Enemy2IdleCips;
    [SerializeField] private AudioClip[] Enemy2HurtClips;
    [SerializeField] private AudioClip Enemy2Death;

    [Header("Punch Impact AudioClips")]
    [SerializeField] private AudioClip[] PunchClips;

    private void Start()
    {
        GameEventSystem.Current.ParryEvent += OnParryEvent;
        GameEventSystem.Current.HitEvent += OnHitEvent;
        GameEventSystem.Current.FootstepEvent += OnFootStepEvent;
        GameEventSystem.Current.DeathEvent += OnDeathEvent;
    }

    private void OnDisable()
    {
        GameEventSystem.Current.ParryEvent -= OnParryEvent;
        GameEventSystem.Current.HitEvent -= OnHitEvent;
        GameEventSystem.Current.FootstepEvent -= OnFootStepEvent;
    }

    private void OnParryEvent(GameObject victim, GameObject attacker, HurtInfo info)
    {
        if(victim != null && attacker != null)
        {
            
            switch (victim.tag)
            {
                case "Player":
                    AudioManager.Current?.PlaySFX(SFXClipsPlayer[0], victim.transform.position);
                    break;
                default: 
                    break;

            }
        }
    }

    private void OnHitEvent(GameObject attacker, GameObject victim, HurtInfo info)
    {
        if (victim != null && attacker != null)
        {
            switch (victim.tag)
            {
                case "Player":
                    if(victim.GetComponent<PlayerBlock>().isBlocking == true || victim.GetComponent<PlayerBlock>().isParrying == true)
                    {
                        return;
                    }
                    AudioManager.Current?.PlaySFX(PunchClips, victim.transform.position);
                    break;
                case "Enemy":
                    if(victim.GetComponent<SlamAttackScript>() != null)
                    {
                        AudioManager.Current?.PlaySFX(Enemy2HurtClips, victim.transform.position);
                        AudioManager.Current?.PlaySFX(PunchClips, victim.transform.position);
                    }
                    else
                    {
                        AudioManager.Current?.PlaySFX(Enemy1HurtClips, victim.transform.position);
                        AudioManager.Current?.PlaySFX(PunchClips, victim.transform.position);
                    }
                    break;
                default:
                    Debug.Log("Victim is not a player or an enemy, ayo? Skibidi toilet be like");
                    break;
            }
        }
    }

    private void OnFootStepEvent(GameObject subject, string type, Transform position)
    {
        if (subject != null)
        {
            //AudioManager.Current?.PlaySFX(SFXClipsPlayer[1], subject.transform.position);
        }
    }

    private void OnDeathEvent(GameObject victim, GameObject killer, HurtInfo hurtinfo)
    {
        if (victim != null && killer != null)
        {
            switch (victim.tag)
            {
                case "Player":
                    
                    break;
                case "Enemy":
                    if (victim.GetComponent<SlamAttackScript>() != null)
                    {
                        AudioManager.Current?.PlaySFX(Enemy2Death, victim.transform.position);
                    }
                    else
                    {
                        AudioManager.Current?.PlaySFX(Enemy1Death, victim.transform.position);
                    }
                    break;
                default:
                    Debug.Log("Victim is not a player or an enemy, ayo? Skibidi toilet be like");
                    break;
            }
        }
    }
    //private void OnBlockEvent

}
