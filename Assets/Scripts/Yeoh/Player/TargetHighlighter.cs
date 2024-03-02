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

    Color defaultOutlineColor;

    void Awake()
    {
        player=GetComponent<Player>();
        matManager=GetComponent<MaterialManager>();
        topFinder=GetComponent<TopVertexFinder>();

        defaultOutlineColor = outlineMaterial.color;
    }

    GameObject lastTarget;
    float topY;

    void Update()
    {
        if(lastTarget!=player.target)
        {
            if(lastTarget) ToggleHighlight(lastTarget, false);

            lastTarget=player.target;

            if(lastTarget) ToggleHighlight(lastTarget, true);
        }

        if(indicator && !lastTarget) Destroy(indicator);

        if(indicatorTC) indicatorTC.positionOffset.y = topY + offsetY;

        CheckManualColor();
    }

    GameObject indicator;
    TransformConstraint indicatorTC;
    SpriteRenderer indicatorSR;

    void ToggleHighlight(GameObject target, bool toggle)
    {
        Renderer[] targetRenderers=target.GetComponentsInChildren<Renderer>();
        
        for(int i=0; i<targetRenderers.Length; i++)
        {
            if(toggle) matManager.AddMaterial(targetRenderers[i], outlineMaterial);
            else matManager.RemoveMaterial(targetRenderers[i], outlineMaterial);
        }

        if(toggle)
        {
            indicator=Instantiate(indicatorPrefab, target.transform.position, Quaternion.identity);
            indicator.hideFlags = HideFlags.HideInHierarchy;

            indicatorTC = indicator.GetComponent<TransformConstraint>();
            indicatorTC.constrainTo = target.transform;

            topY = topFinder.GetTopVertex(target).y;

            indicatorSR = indicator.GetComponent<SpriteRenderer>();
        }
        else
        {
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

        if(player.manual.target && lastTarget==player.manual.target)
        {
            newColor = Color.cyan;
        }
        else newColor = defaultOutlineColor;

        if(outlineMaterial.color != newColor) outlineMaterial.color = newColor;
        if(indicatorSR && indicatorSR.color != newColor) indicatorSR.color = newColor;
    }

    void OnDisable()
    {
        outlineMaterial.color = defaultOutlineColor;
    }
}
