using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TargetHighlighter : MonoBehaviour
{
    Player player;

    public Material outlineMaterial;
    public GameObject indicatorPrefab;

    void Awake()
    {
        player=GetComponent<Player>();
    }

    GameObject target;
    float localTopY;

    void Update()
    {
        CheckSwitchTarget();
        CheckManualColor();

        if(indicatorTC) indicatorTC.positionOffset.y = localTopY + offsetYAnim; // animated float
    }

    void CheckSwitchTarget()
    {
        if(target!=player.target)
        {
            if(indicator) Destroy(indicator);

            Unhighlight(target);

            target=player.target;

            Highlight(target);
        }

        if((!target || !player.target) && indicator) Destroy(indicator);
    }

    GameObject indicator;
    TransformConstraint indicatorTC;
    SpriteRenderer indicatorSR;

    void Highlight(GameObject _target)
    {
        if(_target)
        {
            ModelManager.Current.AddMaterial(_target, outlineMaterial);

            indicator=Instantiate(indicatorPrefab, _target.transform.position, Quaternion.identity);
            indicator.hideFlags = HideFlags.HideInHierarchy;

            indicatorTC = indicator.GetComponent<TransformConstraint>();
            indicatorTC.constrainTo = _target.transform;

            localTopY = ModelManager.Current.GetColliderTop(_target).y - _target.transform.position.y;

            indicatorSR = indicator.GetComponent<SpriteRenderer>();
        }
    }

    void Unhighlight(GameObject _target)
    {
        if(_target)
        {
            ModelManager.Current.RemoveMaterial(_target, outlineMaterial);
        }
    }

    float offsetYAnim;

    void PlayOffsetAnim()
    {
        LeanTween.value(.1f, .25f, .5f)
            .setEaseInOutSine()
            .setIgnoreTimeScale(true)
            .setLoopPingPong()
            .setOnUpdate( (float value)=>{offsetYAnim=value;} );
    }

    void OnEnable()
    {
        PlayOffsetAnim();
    }

    void CheckManualColor()
    {
        Color newColor;

        if(player.manual.target && target==player.manual.target)
        {
            newColor = Color.cyan;
        }
        else newColor = outlineMaterial.color;

        if(target)
        {
            foreach(Material outlineMat in ModelManager.Current.GetMaterials(target, outlineMaterial))
            {
                if(outlineMat.color != newColor) outlineMat.color = newColor;
            }
        }

        if(indicatorSR && indicatorSR.color != newColor) indicatorSR.color = newColor;
    }
}
