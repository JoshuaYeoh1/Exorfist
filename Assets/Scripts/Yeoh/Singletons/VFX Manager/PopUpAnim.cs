using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpAnim : MonoBehaviour
{
    Rigidbody rb;

    public float animIn=.5f, animWait=.5f, animOut=.5f;
    public Vector3 pushForce;

    void Awake()
    {
        rb=GetComponent<Rigidbody>();
    }

    Vector3 defScale;
    
    void Start()
    {
        defScale = transform.localScale;

        StartCoroutine(Animating());
    }

    IEnumerator Animating()
    {
        transform.localScale = Vector3.zero;

        Push();

        LeanTween.scale(gameObject, defScale, animIn).setEaseOutElastic().setIgnoreTimeScale(true);

        yield return new WaitForSeconds(animIn + animWait);

        LeanTween.scale(gameObject, Vector3.zero, animOut).setEaseInOutSine().setIgnoreTimeScale(true);

        yield return new WaitForSeconds(animOut);

        Destroy(gameObject);
    }

    void Push()
    {
        pushForce = new Vector3
        (
            Random.Range(pushForce.x, -pushForce.x),
            pushForce.y,
            Random.Range(pushForce.z, -pushForce.z)
        );

        rb.AddForce(pushForce, ForceMode.Impulse);
    }
}
