using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WiggleLight : MonoBehaviour
{
    Light mylight;

    [Header("Wiggle")]
    public bool wiggle=true;
    float seed, defaultValue, wiggledValue;
    public float frequency=3, magnitude=4, offset=4, duration;

    [Header("Preview")]
    public float min;
    public float current, max;

    void Awake()
    {
        mylight=GetComponent<Light>();
        defaultValue=mylight.intensity;
        min=offset;
        //seed=Random.value;
        seed=Random.Range(-9999f,9999f);
    }

    void Update()
    {
        if(wiggle)
        {
            wiggledValue = (Mathf.PerlinNoise(seed,Time.time*frequency)*2-1)*magnitude+offset;
            mylight.intensity = wiggledValue;
        }

        current=wiggledValue;
        if(current>max) max=current;
        if(current<min) min=current;
    }

    public void shake()
    {
        if(shakeRt!=null) StopCoroutine(shakeRt);
        shakeRt = StartCoroutine(shakeEnum());
    }

    Coroutine shakeRt;
    IEnumerator shakeEnum()
    {
        mylight.intensity = defaultValue;
        wiggle=true;
        yield return new WaitForSeconds(duration);
        wiggle=false;
        mylight.intensity = defaultValue;
    }
}
