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
            childrenCanvasGroupsDict[child] = child.GetComponent<CanvasGroup>();
        }
    }

    public GameObject upgradeMenu;

    void OnShowMenu(string menuName)
    {
        if(menuName=="UpgradeMenu")
        {
            upgradeMenu.SetActive(true);

            upgradeMenu.GetComponent<TweenAnimSequence>().Play();

            SetAlpha(upgradeMenu, 0);
            Fade(upgradeMenu, 1, .5f);

            FadeAllBut(upgradeMenu, 0, .5f);
        }
    }

    void OnHideMenu(string menuName)
    {
        if(menuName=="UpgradeMenu")
        {
            upgradeMenu.SetActive(false);

            FadeAllBut(upgradeMenu, 1, .5f);
        }
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
