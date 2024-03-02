using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TargetHighlighter : MonoBehaviour
{
    Player player;
    MaterialManager matManager;
    TopVertexFinder topFinder;

    public Material outlineMaterial;
    public GameObject indicatorPrefab;

    void Awake()
    {
        player=GetComponent<Player>();
        matManager=GetComponent<MaterialManager>();
        topFinder=GetComponent<TopVertexFinder>();
    }

    GameObject target;
    float topY;

    void Update()
    {
        CheckSwitchTarget();

        if(indicator && !target) Destroy(indicator);

        if(indicatorTC) indicatorTC.positionOffset.y = topY + offsetY;

        CheckManualColor();
    }

    void CheckSwitchTarget()
    {
        if(target!=player.target)
        {
            if(target) ToggleHighlight(target, false);

            target=player.target;

            if(target) ToggleHighlight(target, true);
        }
    }

    GameObject indicator;
    TransformConstraint indicatorTC;
    SpriteRenderer indicatorSR;

    void ToggleHighlight(GameObject target, bool toggle)
    {
        if(toggle)
        {
            matManager.AddMaterial(target, outlineMaterial);

            indicator=Instantiate(indicatorPrefab, target.transform.position, Quaternion.identity);
            indicator.hideFlags = HideFlags.HideInHierarchy;

            indicatorTC = indicator.GetComponent<TransformConstraint>();
            indicatorTC.constrainTo = target.transform;

            topY = topFinder.GetTopVertex(target).y - target.transform.position.y;

            indicatorSR = indicator.GetComponent<SpriteRenderer>();
        }
        else
        {
            matManager.RemoveMaterial(target, outlineMaterial);

            if(indicator) Destroy(indicator);
        }
    }

    float offsetY;

    void PlayOffsetAnim()
    {
        LeanTween.value(.1f, .25f, .5f)
            .setEaseInOutSine()
            .setIgnoreTimeScale(true)
            .setLoopPingPong()
            .setOnUpdate( (float value)=>{offsetY=value;} );
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
            foreach(Material outlineMat in matManager.GetMaterials(target, outlineMaterial))
            {
                if(outlineMat.color != newColor) outlineMat.color = newColor;
            }
        }

        if(indicatorSR && indicatorSR.color != newColor) indicatorSR.color = newColor;
    }
}
