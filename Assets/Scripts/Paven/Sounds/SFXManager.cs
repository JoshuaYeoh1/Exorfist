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
        GameEventSystem.Current.LootEvent += OnLootEvent;
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
        GameEventSystem.Current.AbilityEndEvent -= OnAbilityEnd;
        GameEventSystem.Current.LootEvent -= OnLootEvent;
    }

    void OnHit(GameObject attacker, GameObject victim, HurtInfo hurtInfo)
    {
        if(attacker.tag=="Player")
        {
            
        }
        else
        {
            if(hurtInfo.attackerName=="Enemy1")
            {

            }
            if(hurtInfo.attackerName=="Enemy2")
            {

            }
        }
    }

    void OnHurt(GameObject victim, GameObject attacker, HurtInfo hurtInfo)
    {
        AudioManager.Current.PlaySFX(sfxGenericHit, victim.transform.position);

        if(victim.tag=="Player")
        {
            AudioManager.Current.PlaySFX(sfxUIHurt, victim.transform.position, false);
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
                AudioManager.Current.PlaySFX(sfxDummyHit, victim.transform.position);
            }
        }

        if(hurtInfo.attackName=="AOE")
        {
            AudioManager.Current.PlaySFX(sfxFireHit, victim.transform.position);
        }
        if(hurtInfo.attackName=="Enemy2Slam")
        {
            AudioManager.Current.PlaySFX(sfxFireHit, victim.transform.position);
        }
        if(hurtInfo.attackName=="Laser")
        {
            AudioManager.Current.PlaySFX(sfxFireHit, victim.transform.position);
        }

        if(hurtInfo.attackName=="Light")
        {
            AudioManager.Current?.PlaySFX(PunchClips, victim.transform.position);
            AudioManager.Current.PlaySFX(sfxH2hHit, victim.transform.position);
        }
        if(hurtInfo.attackName=="Heavy")
        {
            AudioManager.Current?.PlaySFX(PunchClips, victim.transform.position);
            AudioManager.Current.PlaySFX(sfxH2hHit, victim.transform.position);
            AudioManager.Current.PlaySFX(sfxH2hHeavyHit, victim.transform.position);
        }
        if(hurtInfo.attackName=="Enemy1")
        {
            AudioManager.Current?.PlaySFX(PunchClips, victim.transform.position);
            AudioManager.Current.PlaySFX(sfxH2hHit, victim.transform.position);
        }
        
        if(hurtInfo.attackName=="Pool")
        {
            AudioManager.Current.PlaySFX(sfxH2hHeavyHit, victim.transform.position);
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
            AudioManager.Current.PlaySFX(sfxBlock1, victim.transform.position);
            AudioManager.Current.PlaySFX(sfxBlock2, victim.transform.position);
        }
    }

    void OnParry(GameObject defender, GameObject attacker, HurtInfo hurtInfo)
    {
        if(defender.tag=="Player")
        {
            AudioManager.Current?.PlaySFX(SFXClipsPlayer[0], defender.transform.position);
            AudioManager.Current.PlaySFX(sfxParry1, defender.transform.position);
            AudioManager.Current.PlaySFX(sfxParry2, defender.transform.position);
        }
    }

    void OnBlockBreak(GameObject victim, GameObject attacker, HurtInfo hurtInfo)
    {
        if(victim.tag=="Player")
        {
            AudioManager.Current?.PlaySFX(SFXClipsPlayer[3], victim.transform.position);
            AudioManager.Current.PlaySFX(sfxBlockBreak1, victim.transform.position);
            AudioManager.Current.PlaySFX(sfxBlockBreak2, victim.transform.position);
        }
    }

    void OnDeath(GameObject victim, GameObject killer, HurtInfo hurtinfo)
    {
        if(victim.tag=="Player")
        {
            //AudioManager.Current.PlaySFX(sfxKillHit, victim.transform.position); // EARRAPE
            AudioManager.Current.PlaySFX(sfxUIDeath, victim.transform.position, false, false);
        }
        else
        {
            if(hurtinfo.victimName=="Enemy1")
            {
                AudioManager.Current?.PlaySFX(Enemy1Death, victim.transform.position);
                //AudioManager.Current.PlaySFX(sfxKillHit, victim.transform.position);
            }
            if(hurtinfo.victimName=="Enemy2")
            {
                AudioManager.Current?.PlaySFX(Enemy2Death, victim.transform.position);
                AudioManager.Current.PlaySFX(voiceEnemy2Death, victim.transform.position);
                //AudioManager.Current.PlaySFX(sfxKillHit, victim.transform.position);
            }
            if(hurtinfo.victimName=="Dummy")
            {

            }
        }
    }

    void OnFootstep(GameObject subject, string type, Transform footstepTr)
    {
        if(subject.tag=="Player")
        {
            //AudioManager.Current?.PlaySFX(SFXClipsPlayer[2], subject.transform.position);
            AudioManager.Current.PlaySFX(sfxFstPlayer, footstepTr.position);
        }

        if(type=="Ragdoll")
        {
            AudioManager.Current.PlaySFX(sfxRagdoll, subject.transform.position);
        }
        if(type=="RagdollBig")
        {
            AudioManager.Current.PlaySFX(sfxFstEnemy2, subject.transform.position);
        }
    }

    void OnAbilityCast(GameObject caster, string abilityName)
    {
        if(caster.tag=="Player")
        {
            if(abilityName=="AOE")
            {
                AudioManager.Current?.PlaySFX(AbilityClipsPlayer[1], caster.transform.position);

                AudioManager.Current.PlaySFX(sfxPlayerAoe, caster.transform.position);
                AudioManager.Current.PlaySFX(sfxPlayerAoe2, caster.transform.position);
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

    void OnLootEvent(GameObject looter, LootInfo lootinfo)
    {
        if(lootinfo.lootName=="Chi")
        {
            AudioManager.Current.PlaySFX(SFXClipsPlayer[4], looter.transform.position);
            AudioManager.Current.PlaySFX(sfxChi, looter.transform.position);
        }
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    [Header("Yeoh Ability")]
    public AudioClip[] sfxPlayerAoe;
    public AudioClip[] sfxPlayerAoe2;
    public AudioClip[] sfxCastingLoop;
    public AudioClip[] sfxCharge;
    public AudioClip[] sfxEnemy2Jump;
    public AudioClip[] sfxEnemy2Slam;
    public AudioClip[] sfxHeal1, sfxHeal2;
    public AudioClip[] sfxLaserIn, sfxLaserLoop, sfxLaserOut;

    [Header("Yeoh Foosteps")]
    public AudioClip[] sfxFstPlayer;
    public AudioClip[] sfxFstEnemy1;
    public AudioClip[] sfxFstEnemy2;

    [Header("Yeoh Hit")]
    public AudioClip[] sfxBlock1;
    public AudioClip[] sfxBlock2;
    public AudioClip[] sfxBlockBreak1;
    public AudioClip[] sfxBlockBreak2;
    public AudioClip[] sfxDummyHit;
    public AudioClip[] sfxFireHit;
    public AudioClip[] sfxGenericHit;
    public AudioClip[] sfxH2hHit;
    public AudioClip[] sfxH2hHeavyHit;
    public AudioClip[] sfxKillHit;
    public AudioClip[] sfxParry1;
    public AudioClip[] sfxParry2;
    public AudioClip[] sfxRagdoll;

    [Header("Yeoh Swing")]
    public AudioClip[] sfxSwingBig;
    public AudioClip[] sfxSwingSmall;

    [Header("Yeoh UI")]
    public AudioClip[] sfxUIBack;
    public AudioClip[] sfxUICameraPan;
    public AudioClip[] sfxChi;
    public AudioClip[] sfxUIClear;
    public AudioClip[] sfxUIClick;
    public AudioClip[] sfxUICooldown;
    public AudioClip[] sfxUIDeath;
    public AudioClip[] sfxUIHover;
    public AudioClip[] sfxUIHurt;
    public AudioClip[] sfxUILowHealth;
    public AudioClip[] sfxUIMeditate;
    public AudioClip[] sfxTransition;
    public AudioClip[] sfxUITrigger;
    public AudioClip[] sfxUIUpgradeNo;
    public AudioClip[] sfxUIUpgradeYes;
    public AudioClip[] sfxUIWin;

    [Header("Yeoh Voice")]
    public AudioClip[] voiceEnemy2Death;
    public AudioClip[] voicePlayerAttackEpic;
    public AudioClip[] voicePlayerAttackHigh;
    public AudioClip[] voicePlayerAttackLow;
    public AudioClip[] voicePlayerAttackMid;
    public AudioClip[] voicePlayerBlock;
    public AudioClip[] voicePlayerHurtHigh;
    public AudioClip[] voicePlayerHurtLow;
    public AudioClip[] voicePlayerHurtMid;
}
