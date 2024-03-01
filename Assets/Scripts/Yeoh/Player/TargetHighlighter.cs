using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetHighlighter : MonoBehaviour
{
    Player player;
    MaterialManager matManager;
    TopMostVertexFinder topFinder;

    public Material outlineMaterial;
    public GameObject indicatorPrefab;

    void Awake()
    {
        player=GetComponent<Player>();
        matManager=GetComponent<MaterialManager>();
        topFinder=GetComponent<TopMostVertexFinder>();
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
    }

    GameObject indicator;
    TransformConstraint indicatorTC;

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
            topY = topFinder.GetTopMostVertex(target).y - target.transform.position.y;
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
}
