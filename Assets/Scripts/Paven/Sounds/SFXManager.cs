using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public static SFXManager Current;

    [Header("Player AudioClips")]
    [SerializeField] private AudioClip[] SFXClipsPlayer;
    [SerializeField] private AudioClip[] AbilityClipsPlayer;

    [Header("Enemy AudioClips")]
    [Header("Enemy1")]
    [SerializeField] private AudioClip[] Enemy1HurtClips;
    [SerializeField] private AudioClip Enemy1Death;

    [Header("Enemy2")]
    //[SerializeField] private AudioClip[] Enemy2IdleCips;
    [SerializeField] private AudioClip[] Enemy2HurtClips;
    [SerializeField] private AudioClip Enemy2Death;
    

    [Header("Punch Impact AudioClips")]
    [SerializeField] private AudioClip[] PunchClips;

    

    //Data values for audio clip
    [SerializeField] private List<AudioClip> PlayerClipsDV;
    [SerializeField] private List<AudioClip> EnemyClipsDV;
    [SerializeField] private List<AudioClip> EnvironmentClipsDV;

    //Dictionary definitions and their constructors
    private Dictionary<string, AudioClip> EnemyClips = new Dictionary<string, AudioClip>();
    private Dictionary<string, AudioClip> PlayerClips = new Dictionary<string, AudioClip>();
    private Dictionary<string, AudioClip> EnvironmentClips = new Dictionary<string, AudioClip>();

    private void Start()
    {
        DictSetup();
        GameEventSystem.Current.ParryEvent += OnParryEvent;
        GameEventSystem.Current.HitEvent += OnHitEvent;
        GameEventSystem.Current.FootstepEvent += OnFootStepEvent;
        GameEventSystem.Current.DeathEvent += OnDeathEvent;
        GameEventSystem.Current.EnemySoundEvent += OnSoundEventEnemy;
        GameEventSystem.Current.PlayerSoundEvent += OnSoundEventPlayer;
        GameEventSystem.Current.BlockBreakEvent += OnBlockBreakEvent;
        GameEventSystem.Current.AbilityCastEvent += OnAbilityCast;
        GameEventSystem.Current.AbilityEndEvent += OnAbilityEnd;
    }

    private void OnDisable()
    {
        GameEventSystem.Current.ParryEvent -= OnParryEvent;
        GameEventSystem.Current.HitEvent -= OnHitEvent;
        GameEventSystem.Current.FootstepEvent -= OnFootStepEvent;
        GameEventSystem.Current.DeathEvent -= OnDeathEvent;
        GameEventSystem.Current.EnemySoundEvent -= OnSoundEventEnemy;
        GameEventSystem.Current.PlayerSoundEvent -= OnSoundEventPlayer;
        GameEventSystem.Current.BlockBreakEvent -= OnBlockBreakEvent;
        GameEventSystem.Current.AbilityCastEvent -= OnAbilityCast;
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

    private void OnAbilityCast(GameObject caster, string name)
    {
        if(caster != null)
        {
            switch (caster.tag)
            {
                case "Player":
                    switch (name)
                    {
                        case "AOE":
                            AudioManager.Current?.PlaySFX(AbilityClipsPlayer[1], caster.transform.position);
                            break;

                        case "Laser":
                            //AudioManager.Current?.PlaySFX(PunchClips, caster.transform.position);
                            break;

                        case "Heal":
                            AudioManager.Current?.PlaySFX(AbilityClipsPlayer[2], caster.transform.position);
                            break;

                        default:
                            Debug.LogWarning("Invalid ability cast name for player. No such ability as " + name);
                            break;
                    }
                    break;

                case "Enemy":
                    break;

                default:
                    break;
            }
        }
    }
    private void OnAbilityEnd(GameObject caster, string name)
    {
        if (caster != null)
        {
            switch (caster.tag)
            {
                case "Player":
                    switch (name)
                    {
                        case "AOE":
                            //AudioManager.Current?.PlaySFX(PunchClips, caster.transform.position);
                            break;

                        case "Laser":
                            //AudioManager.Current?.PlaySFX(PunchClips, caster.transform.position);
                            break;

                        case "Heal":
                            //AudioManager.Current?.PlaySFX(Enemy2Death, caster.transform.position);
                            break;

                        default:
                            Debug.LogWarning("Invalid ability cast name for player. No such ability as " + name);
                            break;
                    }
                    break;

                case "Enemy":
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
                        AudioManager.Current?.PlaySFX(SFXClipsPlayer[1], victim.transform.position);
                        return;
                    }
                    AudioManager.Current?.PlaySFX(PunchClips, victim.transform.position);
                    break;
                case "Enemy":
                    if(victim.GetComponent<EnemyAI>() != null)
                    {
                        switch (victim.GetComponent<EnemyAI>().id)
                        {
                            case 0:
                                AudioManager.Current?.PlaySFX(Enemy1HurtClips, victim.transform.position);
                                AudioManager.Current?.PlaySFX(PunchClips, victim.transform.position);
                                break;
                            case 1:
                                AudioManager.Current?.PlaySFX(Enemy2HurtClips, victim.transform.position);
                                AudioManager.Current?.PlaySFX(PunchClips, victim.transform.position);
                                break;
                            default:
                                AudioManager.Current?.PlaySFX(PunchClips, victim.transform.position);
                                break;
                        }
                    }
                    else
                    {
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
            AudioManager.Current?.PlaySFX(SFXClipsPlayer[2], subject.transform.position);
        }
    }

    private void OnBlockBreakEvent(GameObject victim, GameObject attacker, HurtInfo hurtinfo)
    {
        if (victim != null && attacker != null)
        {
            switch (victim.tag)
            {
                case "Player":
                    AudioManager.Current?.PlaySFX(SFXClipsPlayer[3], victim.transform.position);
                    break;

                case "Enemy":
                    break;

                default:
                    Debug.Log("Victim is not a player or an enemy, ayo? Skibidi toilet be like");
                    break;
            }
        }
    }

    private void OnDeathEvent(GameObject victim, GameObject killer, string victimName, HurtInfo hurtinfo)
    {
        if (victim != null && killer != null)
        {
            switch (victim.tag)
            {
                case "Player":
                    
                    break;
                case "Enemy":
                    if(victim.GetComponent<EnemyAI>() != null)
                    {
                        switch (victim.GetComponent<EnemyAI>().id)
                        {
                            case 0:
                                AudioManager.Current?.PlaySFX(Enemy1Death, victim.transform.position);
                                break;

                            case 1:
                                AudioManager.Current?.PlaySFX(Enemy2Death, victim.transform.position);
                                break;

                            default:
                                break;
                        }
                    }
                    break;
                default:
                    Debug.Log("Victim is not a player or an enemy, ayo? Skibidi toilet be like");
                    break;
            }
        }
    }

    private void OnSoundEventEnemy(Transform transform, string searchKey)
    {
        AudioClip clip = EnemyClips[searchKey];
        if(clip != null)
        {
            AudioManager.Current?.PlaySFX(clip, transform.position);
        }
        else
        {
            Debug.Log("There is no sound in the EnemySound dictionary defined as " + searchKey);
        }
    }

    private void OnSoundEventPlayer(Transform transform, string searchKey)
    {
        AudioClip clip = PlayerClips[searchKey];
        if (clip != null)
        {
            AudioManager.Current?.PlaySFX(clip, transform.position);
        }
        else
        {
            Debug.Log("There is no sound in the PlayerClip dictionary defined as " + searchKey);
        }
    }

    private void OnSoundEventEnvironment(Transform transform, string searchKey)
    {
        AudioClip clip = EnvironmentClips[searchKey];
        if (clip != null)
        {
            AudioManager.Current?.PlaySFX(clip, transform.position);
        }
        else
        {
            Debug.Log("There is no sound in the EnvironmentClip dictionary defined as " + searchKey);
        }
    }

    private void DictSetup()
    {
        
        foreach(AudioClip clip in PlayerClipsDV)
        {
            PlayerClips.Add(clip.name, clip);
        }

        foreach(AudioClip clip in EnemyClipsDV)
        {
            EnemyClips.Add(clip.name, clip);
        }
    }
}
