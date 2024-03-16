using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public static SFXManager Current;

    void Awake()
    {
        if(!Current) Current=this;

        DictSetup();
    }
    
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////

    //Dictionary definitions and their constructors
    Dictionary<string, AudioClip> EnemyClips = new Dictionary<string, AudioClip>();
    Dictionary<string, AudioClip> PlayerClips = new Dictionary<string, AudioClip>();
    Dictionary<string, AudioClip> EnvironmentClips = new Dictionary<string, AudioClip>();
    
    //Data values for audio clip
    public List<AudioClip> PlayerClipsDV;
    public List<AudioClip> EnemyClipsDV;
    public List<AudioClip> EnvironmentClipsDV;

    void DictSetup()
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
    
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////

    [Header("Player AudioClips")]
    public AudioClip[] SFXClipsPlayer;
    public AudioClip[] AbilityClipsPlayer;

    [Header("Enemy AudioClips")]
    [Header("Enemy1")]
    public AudioClip[] Enemy1HurtClips;
    public AudioClip Enemy1Death;

    [Header("Enemy2")]
    //public AudioClip[] Enemy2IdleCips;
    public AudioClip[] Enemy2HurtClips;
    public AudioClip Enemy2Death;
    
    [Header("Punch Impact AudioClips")]
    public AudioClip[] PunchClips;

    void OnEnable()
    {
        GameEventSystem.Current.HitEvent += OnHit;
        GameEventSystem.Current.HurtEvent += OnHurt;
        GameEventSystem.Current.StunEvent += OnStun;
        GameEventSystem.Current.BlockEvent += OnBlock;
        GameEventSystem.Current.ParryEvent += OnParry;
        GameEventSystem.Current.BlockBreakEvent += OnBlockBreak;
        GameEventSystem.Current.DeathEvent += OnDeath;
        GameEventSystem.Current.FootstepEvent += OnFootstep;
        GameEventSystem.Current.EnemySoundEvent += OnSoundEnemy;
        GameEventSystem.Current.PlayerSoundEvent += OnSoundPlayer;
        GameEventSystem.Current.AbilityCastEvent += OnAbilityCast;
        GameEventSystem.Current.AbilityEndEvent += OnAbilityEnd;
    }
    void OnDisable()
    {
        GameEventSystem.Current.HitEvent -= OnHit;
        GameEventSystem.Current.HurtEvent -= OnHurt;
        GameEventSystem.Current.StunEvent -= OnStun;
        GameEventSystem.Current.BlockEvent -= OnBlock;
        GameEventSystem.Current.ParryEvent -= OnParry;
        GameEventSystem.Current.BlockBreakEvent -= OnBlockBreak;
        GameEventSystem.Current.DeathEvent -= OnDeath;
        GameEventSystem.Current.FootstepEvent -= OnFootstep;
        GameEventSystem.Current.EnemySoundEvent -= OnSoundEnemy;
        GameEventSystem.Current.PlayerSoundEvent -= OnSoundPlayer;
        GameEventSystem.Current.AbilityCastEvent -= OnAbilityCast;
    }

    void OnHit(GameObject attacker, GameObject victim, HurtInfo hurtInfo)
    {
        AudioManager.Current?.PlaySFX(PunchClips, victim.transform.position);

        if(victim.tag=="Player")
        {

        }
        else
        {
            if(hurtInfo.victimName=="Enemy1")
            {

            }
            if(hurtInfo.victimName=="Enemy2")
            {

            }
            if(hurtInfo.victimName=="Dummy")
            {

            }
        }
    }

    void OnHurt(GameObject victim, GameObject attacker, HurtInfo hurtInfo)
    {
        if(victim.tag=="Player")
        {

        }
        else
        {
            if(hurtInfo.victimName=="Enemy1")
            {
                AudioManager.Current?.PlaySFX(Enemy1HurtClips, victim.transform.position);
            }
            if(hurtInfo.victimName=="Enemy2")
            {
                AudioManager.Current?.PlaySFX(Enemy2HurtClips, victim.transform.position);
            }
            if(hurtInfo.victimName=="Dummy")
            {

            }
        }
    }

    void OnStun(GameObject victim, GameObject attacker, HurtInfo hurtInfo)
    {
        if(victim.tag=="Player")
        {

        }
        else
        {
            if(hurtInfo.victimName=="Enemy1")
            {
                
            }
            if(hurtInfo.victimName=="Enemy2")
            {
                
            }
        }
    }

    void OnBlock(GameObject victim, GameObject attacker, HurtInfo hurtInfo)
    {
        if(victim.tag=="Player")
        {
            AudioManager.Current?.PlaySFX(SFXClipsPlayer[1], victim.transform.position);
        }
    }

    void OnParry(GameObject victim, GameObject attacker, HurtInfo hurtInfo)
    {
        if(victim.tag=="Player")
        {
            AudioManager.Current?.PlaySFX(SFXClipsPlayer[0], victim.transform.position);
        }
    }

    void OnBlockBreak(GameObject victim, GameObject attacker, HurtInfo hurtInfo)
    {
        if(victim.tag=="Player")
        {
            AudioManager.Current?.PlaySFX(SFXClipsPlayer[3], victim.transform.position);
        }
    }

    void OnDeath(GameObject victim, GameObject killer, HurtInfo hurtinfo)
    {
        if(victim.tag=="Player")
        {

        }
        else
        {
            if(hurtinfo.victimName=="Enemy1")
            {
                AudioManager.Current?.PlaySFX(Enemy1Death, victim.transform.position);
            }
            if(hurtinfo.victimName=="Enemy2")
            {
                AudioManager.Current?.PlaySFX(Enemy2Death, victim.transform.position);
            }
            if(hurtinfo.victimName=="Dummy")
            {

            }
        }
    }

    void OnFootstep(GameObject subject, string type, Transform position)
    {
        if(subject.tag=="Player")
        {
            AudioManager.Current?.PlaySFX(SFXClipsPlayer[2], subject.transform.position);
        }
    }

    void OnAbilityCast(GameObject caster, string abilityName)
    {
        if(caster.tag=="Player")
        {
            if(abilityName=="AOE")
            {
                AudioManager.Current?.PlaySFX(AbilityClipsPlayer[1], caster.transform.position);
            }
            if(abilityName=="Laser")
            {
                //AudioManager.Current?.PlaySFX(PunchClips, caster.transform.position);
            }
            if(abilityName=="Heal")
            {
                AudioManager.Current?.PlaySFX(AbilityClipsPlayer[2], caster.transform.position);
            }
        }
        else
        {
            if(abilityName=="Enemy2Slam")
            {

            }
        }
    }

    void OnAbilityEnd(GameObject caster, string abilityName)
    {
        if(caster.tag=="Player")
        {
            if(abilityName=="AOE")
            {

            }
            if(abilityName=="Laser")
            {

            }
            if(abilityName=="Heal")
            {

            }
        }
        else
        {
            if(abilityName=="Enemy2Slam")
            {

            }
        }
    }

    void OnSoundEnemy(Transform transform, string searchKey)
    {
        AudioClip clip = EnemyClips[searchKey];

        if(clip!=null)
        {
            AudioManager.Current?.PlaySFX(clip, transform.position);
        }
        else
        {
            Debug.Log("There is no sound in the EnemySound dictionary defined as " + searchKey);
        }
    }

    void OnSoundPlayer(Transform transform, string searchKey)
    {
        AudioClip clip = PlayerClips[searchKey];

        if(clip!=null)
        {
            AudioManager.Current?.PlaySFX(clip, transform.position);
        }
        else
        {
            Debug.Log("There is no sound in the PlayerClip dictionary defined as " + searchKey);
        }
    }

    void OnSoundEventEnvironment(Transform transform, string searchKey)
    {
        AudioClip clip = EnvironmentClips[searchKey];

        if(clip!=null)
        {
            AudioManager.Current?.PlaySFX(clip, transform.position);
        }
        else
        {
            Debug.Log("There is no sound in the EnvironmentClip dictionary defined as " + searchKey);
        }
    }
}
