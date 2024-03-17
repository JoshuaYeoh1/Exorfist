using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    List<GameObject> canvases = new List<GameObject>();

    Dictionary<GameObject, CanvasGroup> canvasGroupDict = new Dictionary<GameObject, CanvasGroup>();

    void Awake()
    {
        foreach(Transform child in transform)
        {
            canvases.Add(child.gameObject);
        }
        
        RecordCanvasGroups();
    }

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

    public void AlphaAllCanvasGroupsExcept(float alpha, GameObject excludedCanvas)
    {
        foreach(GameObject canvas in canvases)
        {
            if(canvas!=excludedCanvas)
            {
                canvasGroupDict[canvas].alpha=alpha;
            }
        }
    }

    public void AlphaCanvas(float alpha, GameObject selectedCanvas)
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
