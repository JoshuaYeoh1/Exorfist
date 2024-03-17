using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    void Awake()
    {
        foreach(Transform child in transform)
        {
            canvases.Add(child.gameObject);
        }
        
        RecordCanvasGroups();
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

    public GameObject upgradeMenu;

    void OnShowMenu(string menuName)
    {
        if(menuName=="UpgradeMenu")
        {
            upgradeMenu.SetActive(true);

            AlphaAllCanvasGroupsExcept(upgradeMenu, 0);
        }
    }

    void OnHideMenu(string menuName)
    {
        if(menuName=="UpgradeMenu")
        {
            upgradeMenu.SetActive(false);

            AlphaAllCanvasGroupsExcept(upgradeMenu, 1);
        }
    }
    
    List<GameObject> canvases = new List<GameObject>();

    Dictionary<GameObject, CanvasGroup> canvasGroupDict = new Dictionary<GameObject, CanvasGroup>();

    void RecordCanvasGroups()
    {
        foreach(GameObject canvas in canvases)
        {
            canvasGroupDict[canvas] = canvas.GetComponent<CanvasGroup>();
        }
    }

    public void AlphaAllCanvasGroups(float alpha)
    {
        foreach(GameObject canvas in canvases)
        {
            canvasGroupDict[canvas].alpha=alpha;
        }
    }

    public void AlphaAllCanvasGroupsExcept(GameObject excludedCanvas, float alpha)
    {
        foreach(GameObject canvas in canvases)
        {
            if(canvas!=excludedCanvas)
            {
                canvasGroupDict[canvas].alpha=alpha;
            }
        }
    }

    public void AlphaCanvas(GameObject selectedCanvas, float alpha)
    {
        foreach(GameObject canvas in canvases)
        {
            if(canvas==selectedCanvas)
            {
                canvasGroupDict[canvas].alpha=alpha;
            }
        }
    }
}
