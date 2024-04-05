using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserHurtbox : MonoBehaviour
{
    public ParticleSystem ps;
    public List<LineRenderer> lineRenderers = new List<LineRenderer>();

    Dictionary<LineRenderer, Vector2> defaultWidths = new Dictionary<LineRenderer, Vector2>();

    void Awake()
    {
        foreach (LineRenderer lr in lineRenderers)
        {
            defaultWidths[lr] = new Vector2(lr.startWidth, lr.endWidth);
        }
    }

    void Start()
    {
        StartLaser();
    }

    public float tweenTime=.25f;

    void StartLaser()
    {
        foreach(LineRenderer lr in lineRenderers)
        {
            lr.startWidth = 0f;
            lr.endWidth = 0f;

            TweenLineWidth(lr, defaultWidths[lr].x, defaultWidths[lr].y, tweenTime);
        }
    }

    public void StopLaser()
    {
        ps.Stop();

        foreach(LineRenderer lr in lineRenderers)
        {
            TweenLineWidth(lr, 0, 0, tweenTime);
        }

        Destroy(gameObject, tweenTime+.1f);
    }

    void TweenLineWidth(LineRenderer lr, float startTo, float endTo, float time)
    {
        LeanTween.cancel(lr.gameObject);

        LeanTween.value(lr.startWidth, startTo, time)
            .setEaseInOutSine()
            .setOnUpdate( (float value)=>{lr.startWidth=value;} );

        LeanTween.value(lr.endWidth, endTo, time)
            .setEaseInOutSine()
            .setOnUpdate( (float value)=>{lr.endWidth=value;} );
    }
}
