using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHider : MonoBehaviour
{
    public float value, valueMax;
    
    public bool hideWhenFull=true, hideWhenEmpty;
    public InOutAnim targetUI;
    public float animTime=.5f;
    
    bool canShow=true, canHide;

    void Update()
    {
        CheckUIVisibility();
    }

    void CheckUIVisibility()
    {
        if(hideWhenFull && value>=valueMax)
        {
            HideUI();
        }

        if(hideWhenEmpty && value<=0)
        {
            HideUI();
        }

        if(value>0 && value<valueMax)
        {
            ShowUI();
        }
    }

    void HideUI()
    {
        if(canHide)
        {
            canHide=false;
            targetUI.animOut(animTime);
            Invoke("ToggleShow", animTime);  
        }
    }
    void ShowUI()
    {
        if(canShow)
        {   
            canShow=false;
            targetUI.animIn(animTime);
            Invoke("ToggleHide", animTime);
        }
    }

    void ToggleHide()
    {
        canHide=!canHide;
    }
    void ToggleShow()
    {
        canShow=!canShow;
    }
}
