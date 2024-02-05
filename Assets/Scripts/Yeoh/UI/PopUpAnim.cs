using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpAnim : MonoBehaviour
{
    public float animIn=.5f, animWait=.5f, animOut=.5f;

    Vector3 defScale;
    
    void Start()
    {
        defScale = transform.localScale;
        StartCoroutine(Animating());
    }

    IEnumerator Animating()
    {
        transform.localScale = Vector3.zero;

        LeanTween.scale(gameObject, defScale, animIn).setEaseOutElastic().setIgnoreTimeScale(true);

        yield return new WaitForSeconds(animIn + animWait);

        LeanTween.scale(gameObject, Vector3.zero, animOut).setEaseInOutSine().setIgnoreTimeScale(true);

        yield return new WaitForSeconds(animOut);

        Destroy(gameObject);
    }
}
