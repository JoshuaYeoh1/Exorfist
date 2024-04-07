using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class VFXManager : MonoBehaviour
{
    public static VFXManager Current;

    void Awake()
    {
        if(!Current) Current=this;
    }
    
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////

    void OnEnable()
    {
        GameEventSystem.Current.HurtEvent += OnHurt;
        GameEventSystem.Current.BlockEvent += OnBlock;
        GameEventSystem.Current.ParryEvent += OnParry;
        GameEventSystem.Current.BlockBreakEvent += OnBlockBreak;
        GameEventSystem.Current.DeathEvent += OnDeath;
        GameEventSystem.Current.AbilityCastEvent += OnAbilityCast;
        GameEventSystem.Current.FootstepEvent += OnFootstep;
        GameEventSystem.Current.LootEvent += OnLoot;
    }
    void OnDisable()
    {
        GameEventSystem.Current.HurtEvent -= OnHurt;
        GameEventSystem.Current.BlockEvent -= OnBlock;
        GameEventSystem.Current.ParryEvent -= OnParry;
        GameEventSystem.Current.BlockBreakEvent -= OnBlockBreak;
        GameEventSystem.Current.DeathEvent -= OnDeath;
        GameEventSystem.Current.AbilityCastEvent -= OnAbilityCast;
        GameEventSystem.Current.FootstepEvent -= OnFootstep;
        GameEventSystem.Current.LootEvent -= OnLoot;
    }
    
    void OnHurt(GameObject victim, GameObject attacker, HurtInfo hurtInfo)
    {
        Color vfxColor = Color.white;

        if(victim.tag=="Player")
        {
            if(hurtInfo.doShockwave) SpawnShockwave(hurtInfo.contactPoint, Color.white);

            if(hurtInfo.doHitstop) TimescaleManager.Current.HitStop();

            CameraManager.Current.Shake();

            if(hurtInfo.doImpact) SpawnImpact(hurtInfo.contactPoint);

            vfxColor = Color.red;
        }

        else if(attacker.tag=="Player")
        {
            if(hurtInfo.doHitstop) TimescaleManager.Current.HitStop();

            if(hurtInfo.doShockwave) SpawnShockwave(hurtInfo.contactPoint, Color.white);

            if(hurtInfo.doShake) CameraManager.Current.Shake();

            if(hurtInfo.doImpact) SpawnImpact(hurtInfo.contactPoint);
        }

        SpawnPopUpText(ModelManager.Current.GetColliderTop(victim), hurtInfo.dmg.ToString(), vfxColor, Vector3.one*2);

        SpawnHitmarker(hurtInfo.contactPoint, vfxColor);

        if(hurtInfo.victimName!="Dummy")
        {
            SpawnBlood(hurtInfo.contactPoint);
        }
    }

    void OnBlock(GameObject defender, GameObject attacker, HurtInfo hurtInfo)
    {
        if(defender.tag=="Player")
        {
            SpawnPopUpText(ModelManager.Current.GetColliderTop(defender), hurtInfo.dmg.ToString(), Color.cyan, Vector3.one*2);

            SpawnFlash(hurtInfo.contactPoint, Color.cyan);

            SpawnShockwave(hurtInfo.contactPoint, Color.cyan);

            SpawnSparks(hurtInfo.contactPoint);

            CameraManager.Current.Shake();

            if(hurtInfo.doImpact) SpawnImpact(hurtInfo.contactPoint);
        }
    }

    void OnParry(GameObject defender, GameObject attacker, HurtInfo hurtInfo)
    {
        if(defender.tag=="Player")
        {
            SpawnPopUpText(ModelManager.Current.GetColliderTop(defender), "PARRY!", Color.green, Vector3.one*2);

            SpawnFlash(hurtInfo.contactPoint, Color.green);

            SpawnShockwave(hurtInfo.contactPoint, Color.green);

            SpawnSparks(hurtInfo.contactPoint);

            CameraManager.Current.Shake();

            if(hurtInfo.doImpact) SpawnImpact(hurtInfo.contactPoint);
        }
    }

    void OnBlockBreak(GameObject defender, GameObject attacker, HurtInfo hurtInfo)
    {
        if(defender.tag=="Player")
        {
            SpawnPopUpText(ModelManager.Current.GetColliderTop(defender), "bREAK!", Color.red, Vector3.one*2);

            SpawnFlash(hurtInfo.contactPoint, Color.red);

            SpawnShockwave(hurtInfo.contactPoint, Color.red);

            CameraManager.Current.Shake();

            if(hurtInfo.doImpact) SpawnImpact(hurtInfo.contactPoint);
        }
    }

    void OnDeath(GameObject victim, GameObject killer, HurtInfo hurtInfo)
    {
        if(victim.tag=="Player")
        {
            SpawnPopUpText(ModelManager.Current.GetColliderTop(victim), "DEAD!", Color.red, Vector3.one*2);
        }
        else
        {
            SpawnPopUpText(ModelManager.Current.GetColliderTop(victim), "DEAD!", Color.red, Vector3.one*2);
        }
    }    

    void OnAbilityCast(GameObject caster, string abilityName)
    {
        if(caster.tag=="Player")
        {
            if(abilityName=="AOE")
            {
                CameraManager.Current.Shake(.5f, 3);

                TimescaleManager.Current.HitStop(.01f, .02f);

                SpawnShockwave(ModelManager.Current.GetColliderCenter(caster), Color.yellow);

                SpawnGroundExplosion(caster.transform.position, UpgradeManager.Current.GetAoeRange()*.2f);
            }
            else if(abilityName=="Laser")
            {
                TimescaleManager.Current.HitStop(.01f, .02f);

                SpawnShockwave(ModelManager.Current.GetColliderCenter(caster), Color.yellow);
            }
            else if(abilityName=="Heal")
            {
                SpawnPopUpText(ModelManager.Current.GetColliderTop(caster), "HEAL!", Color.yellow, Vector3.one*2);

                SpawnShockwave(ModelManager.Current.GetColliderCenter(caster), Color.yellow);

                SpawnHeal(caster);
            }

            ModelManager.Current.FlashColor(caster, .5f, .5f, -.5f);
        }
        else
        {
            if(abilityName=="Enemy2Slam")
            {
                CameraManager.Current.Shake(.5f, 3);

                SpawnShockwave(caster.transform.position, Color.white);

                SpawnEnemy2Slam(caster.transform.position);

                ModelManager.Current.FlashColor(caster, .5f, -.5f, -.5f);

                AudioManager.Current.PlaySFX(SFXManager.Current.sfxEnemy2Slam, caster.transform.position);
            }
        }
    }

    void OnFootstep(GameObject subject, string type, Transform footstepTr)
    {
        if(subject.tag=="Player")
        {
            SpawnPlayerFootprint(footstepTr);
        }
    }

    public void OnLoot(GameObject looter, LootInfo lootInfo)
    {
        if(lootInfo.lootName=="Chi")
        {
            SpawnShockwave(lootInfo.contactPoint, Color.white);

            //SpawnPopUpText(ModelManager.Current.GetColliderTop(looter), $"+{lootInfo.quantity}", Color.white, Vector3.one*2); // too cluttered

            ModelManager.Current.FlashColor(looter, 1, 1, 1);

            SpawnImpact(lootInfo.contactPoint);

            UpgradeManager.Current.chi += lootInfo.quantity;
        }
    }
    
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public bool hideInHierarchy=true;

    public GameObject popupPrefab;

    public void SpawnPopUpText(Vector3 pos, string text, Color color, Vector3 pushForce)
    {
        GameObject obj = Instantiate(popupPrefab, pos, Quaternion.identity);
        if(hideInHierarchy) obj.hideFlags = HideFlags.HideInHierarchy;
        
        PopUpAnim popUp = obj.GetComponent<PopUpAnim>();
        popUp.Push(pushForce);

        TextMeshProUGUI[] tmps = popUp.GetComponentsInChildren<TextMeshProUGUI>();

        foreach(TextMeshProUGUI tmp in tmps)
        {
            tmp.text = text;
            tmp.color = color;
        }
    }

    public GameObject hitmarkerPrefab;

    public void SpawnHitmarker(Vector3 pos, Color color, float time=.1f)
    {
        GameObject obj = Instantiate(hitmarkerPrefab, pos, Quaternion.identity);
        if(hideInHierarchy) obj.hideFlags = HideFlags.HideInHierarchy;
        
        ParticleSystem ps = obj.GetComponent<ParticleSystem>();
        ParticleSystem.MainModule main = ps.main;
        main.startColor = color;
    }

    public GameObject flashPrefab;

    public void SpawnFlash(Vector3 pos, Color color)
    {
        GameObject obj = Instantiate(flashPrefab, pos, Quaternion.identity);
        if(hideInHierarchy) obj.hideFlags = HideFlags.HideInHierarchy;

        SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
        sr.color = color;

        sr.GetComponent<Animator>().Play("flash", 0);

        Destroy(obj, .4f);
    }

    public GameObject shockwavePrefab;

    public void SpawnShockwave(Vector3 pos, Color color)
    {
        GameObject obj = Instantiate(shockwavePrefab, pos, Quaternion.identity);
        if(hideInHierarchy) obj.hideFlags = HideFlags.HideInHierarchy;

        ParticleSystem ps = obj.GetComponent<ParticleSystem>();
        ParticleSystem.MainModule main = ps.main;
        main.startColor = color;
    }

    public GameObject aoePrefab;

    public void SpawnGroundExplosion(Vector3 pos, float scaleMult=1)
    {
        GameObject obj = Instantiate(aoePrefab, pos, Quaternion.identity);
        if(hideInHierarchy) obj.hideFlags = HideFlags.HideInHierarchy;

        ParticleSystem ps = obj.GetComponent<ParticleSystem>();
        ps.transform.localScale*=scaleMult;
        ExpandAnim(obj);
    }

    public GameObject enemy2SlamPrefab;

    public void SpawnEnemy2Slam(Vector3 pos)
    {
        GameObject obj = Instantiate(enemy2SlamPrefab, pos, Quaternion.identity);
        if(hideInHierarchy) obj.hideFlags = HideFlags.HideInHierarchy;
    }

    public GameObject bloodPrefab;

    public void SpawnBlood(Vector3 pos)
    {
        GameObject obj = Instantiate(bloodPrefab, pos, Quaternion.identity);
        if(hideInHierarchy) obj.hideFlags = HideFlags.HideInHierarchy;
    }

    public GameObject footprintPrefab;

    public void SpawnPlayerFootprint(Transform footstepTr)
    {
        GameObject obj = Instantiate(footprintPrefab, footstepTr.position, footstepTr.rotation);
        if(hideInHierarchy) obj.hideFlags = HideFlags.HideInHierarchy;
    }

    public GameObject healPrefab;

    public void SpawnHeal(GameObject caster)
    {
        GameObject obj = Instantiate(healPrefab, caster.transform.position, Quaternion.identity);
        if(hideInHierarchy) obj.hideFlags = HideFlags.HideInHierarchy;

        TransformConstraint tc = obj.GetComponent<TransformConstraint>();
        tc.constrainTo = caster.transform;

        StopParticles(obj, UpgradeManager.Current.healDuration);
    }
    
    public GameObject impactPrefab;

    public void SpawnImpact(Vector3 pos)
    {
        GameObject obj = Instantiate(impactPrefab, pos, Quaternion.identity);
        if(hideInHierarchy) obj.hideFlags = HideFlags.HideInHierarchy;
    }
    
    public GameObject sparksPrefab;

    public void SpawnSparks(Vector3 pos)
    {
        GameObject obj = Instantiate(sparksPrefab, pos, Quaternion.identity);
        if(hideInHierarchy) obj.hideFlags = HideFlags.HideInHierarchy;
    }

    public GameObject chiPrefab;

    public void SpawnChi(Vector3 pos, Vector3 pushForce)
    {
        GameObject obj = Instantiate(chiPrefab, pos, Quaternion.identity);
        if(hideInHierarchy) obj.hideFlags = HideFlags.HideInHierarchy;

        ExpandAnim(obj, .75f);

        Loot loot = obj.GetComponent<Loot>();
        loot.Push(pushForce);
    }

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////

    // GameObject Spawn(string poolName, Vector3 position, Quaternion rotation=default)
    // {
    //     return ObjectPooler.Current.SpawnFromPool(poolName, position, rotation);
    // }

    // void HideObject(GameObject obj, float wait=0, float shrink=0, bool destroy=false, bool removeConstraint=false)
    // {
    //     StartCoroutine(HidingObject(obj, wait, shrink, removeConstraint, destroy));
    // }
    // IEnumerator HidingObject(GameObject obj, float wait, float shrink, bool destroy, bool removeConstraint)
    // {
    //     if(wait>0) yield return new WaitForSeconds(wait);

    //     if(!obj) yield break; // you shall not pass

    //     if(shrink>0)
    //     {
    //         LeanTween.scale(obj, Vector3.zero, shrink).setEaseInOutSine();
    //         yield return new WaitForSeconds(shrink);
    //     }

    //     if(removeConstraint)
    //     {
    //         TransformConstraint tc = obj.GetComponent<TransformConstraint>();
    //         if(tc) tc.constrainTo=null;
    //     }

    //     if(destroy) Destroy(obj);
    //     else obj.SetActive(false);
    // }

    void StopParticles(GameObject obj, float wait)
    {
        StartCoroutine(StoppingParticles(obj, wait));
    }
    IEnumerator StoppingParticles(GameObject obj, float wait)
    {
        if(wait>0) yield return new WaitForSeconds(wait);

        if(!obj) yield break; // you shall not pass

        ParticleSystem ps = obj.GetComponent<ParticleSystem>();
        ps.Stop();
    }

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    void Update()
    {
        Testing();
    }
    
    void Testing()
    {
        // if(Input.GetKeyDown(KeyCode.Keypad0)) CameraManager.Current.Shake();
        // if(Input.GetKeyDown(KeyCode.Keypad1)) TimescaleManager.Current.HitStop();
        // if(Input.GetKeyDown(KeyCode.Keypad2)) SpawnHitmarker(PlayerTop(), Color.white);
        // if(Input.GetKeyDown(KeyCode.Keypad3)) SpawnFlash(PlayerTop(), Color.white);
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            SpawnShockwave(PlayerTop(), Color.white);
            SpawnPopUpText(PlayerTop(), "gonk", new Color(1,1,1,.5f), Vector3.one*2);
            AudioManager.Current.PlaySFX(SFXManager.Current.voiceGonk, PlayerTop());
        }
        // if(Input.GetKeyDown(KeyCode.Keypad5)) SpawnGroundExplosion(FindPlayer().transform.position);
        // if(Input.GetKeyDown(KeyCode.Keypad6)) SpawnHeal(FindPlayer());
        // if(Input.GetKeyDown(KeyCode.Keypad7)) SpawnImpact(PlayerTop());
        // if(Input.GetKeyDown(KeyCode.Keypad8)) SpawnSparks(PlayerTop());
        // if(Input.GetKeyDown(KeyCode.Keypad9)) SpawnPopUpText(PlayerTop(), "ABOI", Color.cyan, Vector3.one*2);
        // if(Input.GetKeyDown(KeyCode.KeypadDivide)) SpawnChi(ModelManager.Current.GetColliderCenter(FindPlayer()), Vector3.one*5);
    }

    GameObject FindPlayer()
    {
        return GameObject.FindGameObjectWithTag("Player");
    }

    Vector3 PlayerTop()
    {
        return ModelManager.Current.GetColliderTop(FindPlayer());
    }

    void ExpandAnim(GameObject obj, float time=.15f)
    {
        Vector3 defscale = obj.transform.localScale;

        obj.transform.localScale=Vector3.zero;

        LeanTween.scale(obj, defscale, time).setEaseInOutSine();
    }
}
