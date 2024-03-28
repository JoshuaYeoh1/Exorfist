using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VignetteHealth : MonoBehaviour
{
    Image vignette;

    public HPManager hp;
    public Color vignetteColor;

    void Awake()
    {
        vignette=GetComponent<Image>();
    }
    
    void Update()
    {
        vignetteColor.a = (hp.hpMax!=0)? 1-hp.hp/hp.hpMax : 1;

        vignette.color = vignetteColor;
    }
}
