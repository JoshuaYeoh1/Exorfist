using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.VFX;
using UnityEngine.SceneManagement;

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
        GameEventSystem.Current.AbilitySlowMoEvent += OnAbilitySlowMo;
        GameEventSystem.Current.AbilityCastEvent += OnAbilityCast;
        GameEventSystem.Current.AbilityCastingEvent += OnAbilityCasting;
        GameEventSystem.Current.FootstepEvent += OnFootstep;
        GameEventSystem.Current.LootEvent += OnLoot;

        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }
    void OnDisable()
    {
        GameEventSystem.Current.HurtEvent -= OnHurt;
        GameEventSystem.Current.BlockEvent -= OnBlock;
        GameEventSystem.Current.ParryEvent -= OnParry;
        GameEventSystem.Current.BlockBreakEvent -= OnBlockBreak;
        GameEventSystem.Current.DeathEvent -= OnDeath;
        GameEventSystem.Current.AbilitySlowMoEvent -= OnAbilitySlowMo;
        GameEventSystem.Current.AbilityCastEvent -= OnAbilityCast;
        GameEventSystem.Current.AbilityCastingEvent -= OnAbilityCasting;
        GameEventSystem.Current.FootstepEvent -= OnFootstep;
        GameEventSystem.Current.LootEvent -= OnLoot;

        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }
    
    void OnHurt(GameObject victim, GameObject attacker, HurtInfo hurtInfo)
    {
        Color vfxColor = Color.white;

        if(victim.tag=="Player")
        {
            if(hurtInfo.doShockwave) SpawnShockwave(hurtInfo.contactPoint, Color.white);

            if(hurtInfo.doHitstop) HitStop();

            CameraManager.Current.Shake();

            if(hurtInfo.doImpact) SpawnImpact(hurtInfo.contactPoint);

            vfxColor = Color.red;
        }

        else if(attacker.tag=="Player")
        {
            if(hurtInfo.doHitstop) HitStop();

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
            SpawnPopUpText(ModelManager.Current.GetColliderTop(victim), "DEAD!", Color.grey, Vector3.one*2);
        }
    }
    
    void OnAbilitySlowMo(bool toggle)
    {
        canHitStop=!toggle;
    }

    void OnAbilityCasting(GameObject caster, string abilityName)
    {
        if(caster.tag=="Player")
        {
        }
        else
        {
            if(abilityName=="Enemy2Slam")
            {
                //SpawnEnemy2Trail(caster, caster.transform.position); //ugly
            }
        }
    }

    void OnAbilityCast(GameObject caster, string abilityName)
    {
        if(caster.tag=="Player")
        {
            if(abilityName=="AOE")
            {
                CameraManager.Current.Shake(.5f, 3);

                HitStop(.01f, .02f);

                SpawnShockwave(ModelManager.Current.GetColliderCenter(caster), Color.yellow);

                SpawnGroundExplosion(caster.transform.position, UpgradeManager.Current.GetAoeRange()*.2f);
            }
            else if(abilityName=="Laser")
            {
                HitStop(.01f, .02f);

                SpawnShockwave(ModelManager.Current.GetColliderCenter(caster), Color.yellow);
            }
            else if(abilityName=="Heal")
            {
                SpawnPopUpText(ModelManager.Current.GetColliderTop(caster), "HEAL!", Color.yellow, Vector3.one*2);

                SpawnShockwave(ModelManager.Current.GetColliderCenter(caster), Color.yellow);

                SpawnHeal(caster);
                SpawnShine(caster);
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

    void OnSceneUnloaded(Scene scene)
    {
        Time.timeScale=1;
    }

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////

    int tweenTimeLt=0;
    public void TweenTime(float to, float time=.01f)
    {
        if(to<0) to=0;

        LeanTween.cancel(tweenTimeLt);

        if(time>0)
        {
            tweenTimeLt = LeanTween.value(Time.timeScale, to, time)
                .setEaseInOutSine()
                .setIgnoreTimeScale(true)
                .setOnUpdate( (float value)=>{Time.timeScale=value;} )
                .id;
        }
        else Time.timeScale = to;
    }

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////

    [HideInInspector] public bool canHitStop=true;

    public void HitStop(float fadeIn=.01f, float wait=.01f, float fadeOut=.25f)
    {
        if(canHitStop)
        {
            if(hitStoppingRt!=null) StopCoroutine(hitStoppingRt);
            hitStoppingRt = StartCoroutine(HitStopping(fadeIn, wait, fadeOut));
        }
    }
    Coroutine hitStoppingRt;
    IEnumerator HitStopping(float fadeIn, float wait, float fadeOut)
    {
        TweenTime(0, fadeIn);

        if(fadeIn>0) yield return new WaitForSecondsRealtime(fadeIn);
        if(wait>0) yield return new WaitForSecondsRealtime(wait);

        TweenTime(1, fadeOut);
    }

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////

    GameObject Spawn(string poolName, Vector3 position, Quaternion rotation=default)
    {
        return ObjectPooler.Current.SpawnFromPool(poolName, position, rotation);
    }

    void HideObject(GameObject obj, float wait=0, float shrink=0, bool removeConstraint=false)
    {
        StartCoroutine(HidingObject(obj, wait, shrink, removeConstraint));
    }
    IEnumerator HidingObject(GameObject obj, float wait, float shrink, bool removeConstraint)
    {
        if(wait>0) yield return new WaitForSeconds(wait);

        if(!obj) yield break; // you shall not pass

        if(shrink>0)
        {
            LeanTween.scale(obj, Vector3.zero, shrink).setEaseInOutSine();
            yield return new WaitForSeconds(shrink);
        }

        if(removeConstraint)
        {
            TransformConstraint tc = obj.GetComponent<TransformConstraint>();
            tc.constrainTo=null;
        }

        obj.SetActive(false);
    }
    
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public void SpawnPopUpText(Vector3 pos, string text, Color color, Vector3 pushForce)
    {
        PopUpAnim popUp = Spawn("PopUpText", pos).GetComponent<PopUpAnim>();

        popUp.Push(pushForce);

        TextMeshProUGUI[] tmps = popUp.GetComponentsInChildren<TextMeshProUGUI>();

        foreach(TextMeshProUGUI tmp in tmps)
        {
            tmp.text = text;
            tmp.color = color;
        }
    }

    public void SpawnHitmarker(Vector3 pos, Color color, float time=.1f)
    {
        SpriteRenderer sr = Spawn("Hitmarker", pos).GetComponent<SpriteRenderer>();

        sr.color = color;

        HideObject(sr.gameObject, time, .2f);
    }

    public void SpawnFlash(Vector3 pos, Color color)
    {
        SpriteRenderer sr = Spawn("Flash", pos).GetComponent<SpriteRenderer>();

        sr.color = color;

        sr.GetComponent<Animator>().Play("flash", 0);

        HideObject(sr.gameObject, .4f);
    }

    public void SpawnShockwave(Vector3 pos, Color color)
    {
        ParticleSystem shock = Spawn("Shockwave", pos).GetComponent<ParticleSystem>();

        ParticleSystem.MainModule main = shock.main;
        main.startColor = color;

        shock.Play();
    }

    public void SpawnGroundExplosion(Vector3 pos, float scaleMult=1)
    {
        ParticleSystem explode = Spawn("GroundExplosion", pos).GetComponent<ParticleSystem>();

        explode.transform.localScale*=scaleMult;

        ExpandAnim(explode.gameObject);

        VisualEffect[] childVFXs = explode.GetComponentsInChildren<VisualEffect>();

        foreach(VisualEffect vfx in childVFXs)
        {
            vfx.Play();
        }

        explode.Play();
    }

    public void SpawnBlood(Vector3 pos)
    {
        VisualEffect blood = Spawn("Blood", pos).GetComponent<VisualEffect>();

        blood.Play();

        HideObject(blood.gameObject, 1);
    }

    public void SpawnPlayerFootprint(Transform footstepTr)
    {
        GameObject footprint = Spawn("PlayerFootprint", footstepTr.position, footstepTr.rotation);

        VisualEffect[] childVFXs = footprint.GetComponentsInChildren<VisualEffect>();

        foreach(VisualEffect vfx in childVFXs)
        {
            vfx.Play();
        }

        HideObject(footprint, 3);
    }

    public void SpawnHeal(GameObject caster)
    {
        VisualEffect heal = Spawn("Heal", caster.transform.position).GetComponent<VisualEffect>();

        heal.Play();

        heal.GetComponent<TransformConstraint>().constrainTo = caster.transform;

        HideObject(heal.gameObject, UpgradeManager.Current.healDuration, 0, true);
    }

    public void SpawnShine(GameObject caster)
    {
        TransformConstraint shineTC = Spawn("Shine", caster.transform.position).GetComponent<TransformConstraint>();

        VisualEffect[] childVFXs = shineTC.GetComponentsInChildren<VisualEffect>();

        foreach(VisualEffect vfx in childVFXs)
        {
            vfx.Play();
        }

        shineTC.constrainTo = caster.transform;

        HideObject(shineTC.gameObject, 1, 0, true);
    }
    
    public void SpawnImpact(Vector3 pos)
    {
        VisualEffect impact = Spawn("Impact", pos).GetComponent<VisualEffect>();

        impact.Play();

        HideObject(impact.gameObject, 1);
    }
    
    public void SpawnSparks(Vector3 pos)
    {
        VisualEffect sparks = Spawn("Sparks", pos).GetComponent<VisualEffect>();

        sparks.Play();

        HideObject(sparks.gameObject, 1);
    }

    public void SpawnEnemy2Slam(Vector3 pos)
    {
        ParticleSystem explode = Spawn("Enemy2Slam", pos).GetComponent<ParticleSystem>();

        VisualEffect[] childVFXs = explode.GetComponentsInChildren<VisualEffect>();

        foreach(VisualEffect vfx in childVFXs)
        {
            vfx.Play();
        }

        explode.Play();
    }

    public void SpawnEnemy2Trail(GameObject caster, Vector3 pos)
    {
        GameObject trail = Spawn("Enemy2Trail", pos);

        trail.transform.parent = caster.transform;

        trail.transform.localPosition = new Vector3(0, 1.5f, 0);

        VisualEffect[] childVFXs = trail.GetComponentsInChildren<VisualEffect>();

        foreach(VisualEffect vfx in childVFXs)
        {
            vfx.Play();
        }

        HideObject(trail.gameObject, 1);
    }

    public void SpawnChi(Vector3 pos, Vector3 pushForce)
    {
        Loot chi = Spawn("Chi", pos).GetComponent<Loot>();

        ExpandAnim(chi.gameObject, .75f);

        chi.Push(pushForce);

        VisualEffect[] childVFXs = chi.GetComponentsInChildren<VisualEffect>();

        foreach(VisualEffect vfx in childVFXs)
        {
            vfx.Play();
        }
    }
    
    void Update()
    {
        Testing();
    }
    
    void Testing()
    {
        if(Input.GetKeyDown(KeyCode.Keypad0)) CameraManager.Current.Shake();
        if(Input.GetKeyDown(KeyCode.Keypad1)) HitStop();
        if(Input.GetKeyDown(KeyCode.Keypad2)) SpawnHitmarker(PlayerTop(), Color.white);
        if(Input.GetKeyDown(KeyCode.Keypad3)) SpawnFlash(PlayerTop(), Color.white);
        if(Input.GetKeyDown(KeyCode.Keypad4)) SpawnShockwave(PlayerTop(), Color.white);
        if(Input.GetKeyDown(KeyCode.Keypad5)) SpawnGroundExplosion(FindPlayer().transform.position);
        if(Input.GetKeyDown(KeyCode.Keypad6)) {SpawnHeal(FindPlayer()); SpawnShine(FindPlayer());}
        if(Input.GetKeyDown(KeyCode.Keypad7)) SpawnImpact(PlayerTop());
        if(Input.GetKeyDown(KeyCode.Keypad8)) SpawnSparks(PlayerTop());
        if(Input.GetKeyDown(KeyCode.Keypad9)) SpawnPopUpText(PlayerTop(), "ABOI", Color.cyan, Vector3.one*2);
        if(Input.GetKeyDown(KeyCode.KeypadDivide)) SpawnChi(ModelManager.Current.GetColliderCenter(FindPlayer()), Vector3.one*5);
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
