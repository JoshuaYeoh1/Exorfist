using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    List<GameObject> children = new List<GameObject>();

    Dictionary<GameObject, CanvasGroup> childrenCanvasGroupsDict = new Dictionary<GameObject, CanvasGroup>();

    void Awake()
    {
        GetChildren();
        GetChildrenCanvaGroups();
    }

    void OnEnable()
    {
        GameEventSystem.Current.ShowMenuEvent += OnShowMenu;
        GameEventSystem.Current.HideMenuEvent += OnHideMenu;
    }
    void OnDisable()
    {
        GameEventSystem.Current.ShowMenuEvent -= OnShowMenu;
        GameEventSystem.Current.HideMenuEvent -= OnHideMenu;
    }

    void GetChildren()
    {
        foreach(Transform child in transform)
        {
            children.Add(child.gameObject);
        }
    }

    void GetChildrenCanvaGroups()
    {
        foreach(GameObject child in children)
        {
            childrenCanvasGroupsDict.Add(child, child.GetComponent<CanvasGroup>());
        }
    }

    public GameObject upgradeMenu;
    public GameObject pauseMenu;

    void OnShowMenu(string menuName)
    {
        switch(menuName)
        {
            case "UpgradeMenu":
            {
                ShowMenu(upgradeMenu);
                VFXManager.Current.TweenTime(0, .5f);
            } break;

            case "PauseMenu":
            {
                ShowMenu(pauseMenu);
                VFXManager.Current.TweenTime(0, .5f);
            } break;
        }
    }

    void OnHideMenu(string menuName)
    {
        switch(menuName)
        {
            case "UpgradeMenu":
            {
                HideMenu(upgradeMenu);
                VFXManager.Current.TweenTime(1, .5f);
            } break;

            case "PauseMenu":
            {
                HideMenu(pauseMenu);
                VFXManager.Current.TweenTime(1, .5f);
            } break;
        }
    }

    void ShowMenu(GameObject menuObj)
    {
        menuObj.SetActive(true);

        menuObj.GetComponent<TweenAnimSequence>()?.Play();

        SetAlpha(menuObj, 0);
        Fade(menuObj, 1, .5f);

        FadeAllBut(menuObj, 0, .5f);
    }

    void HideMenu(GameObject menuObj)
    {
        menuObj.SetActive(false);

        FadeAllBut(menuObj, 1, .5f);
    }

    public void SetAlpha(GameObject child, float to)
    {
        childrenCanvasGroupsDict[child].alpha=to;
    }

    Dictionary<GameObject, int> tweenIdDict = new Dictionary<GameObject, int>();

    public void Fade(GameObject target, float to, float time)
    {
        if(time>0)
        {
            if(tweenIdDict.ContainsKey(target))
            {
                LeanTween.cancel(tweenIdDict[target]);
            }

            tweenIdDict[target] = LeanTween.value(childrenCanvasGroupsDict[target].alpha, to, time)
                .setEaseInOutSine()
                .setIgnoreTimeScale(true)
                .setOnUpdate( (float value)=>{childrenCanvasGroupsDict[target].alpha=value;} )
                .id;
        }
        else
        {
            childrenCanvasGroupsDict[target].alpha=to;
        }
    }

    public void FadeAll(float alpha, float time)
    {
        foreach(GameObject child in children)
        {
            Fade(child, alpha, time);
        }
    }

    public void FadeAllBut(GameObject excluded, float alpha, float time)
    {
        foreach(GameObject child in children)
        {
            if(child!=excluded)
            {
                Fade(child, alpha, time);
            }
        }
    }    
}
