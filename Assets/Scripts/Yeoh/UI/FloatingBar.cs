using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingBar : MonoBehaviour
{
    public Transform fillPivot;

    int fillingBarLt=0;

    public void FillBar(float val, float time)
    {
        LeanTween.cancel(fillingBarLt);

        if(time>0) fillingBarLt = LeanTween.scaleX(fillPivot.gameObject, val, time).id;

        else fillPivot.transform.localScale = new Vector3(val, fillPivot.transform.localScale.y, fillPivot.transform.localScale.z);
    }
}
