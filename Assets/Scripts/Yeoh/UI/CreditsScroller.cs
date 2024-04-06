using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CreditsScroller : MonoBehaviour
{
    public RectTransform rt;

    public Vector2 startPos;
    public Vector2 endPos;
    Vector2 scrollDir;

    public float scrollSpeed=300;
    float defScrollSpeed;
    public float scrollFasterMult=3;

    void Awake()
    {
        defScrollSpeed = scrollSpeed;

        scrollDir = (endPos - startPos).normalized;
    }

    void OnEnable()
    {
        GoToStartPos();

        scrollSpeed = defScrollSpeed;

        OnScrollStart.Invoke();
    }
    void OnDisable()
    {
        GoToStartPos();
    }

    void Update()
    {
        ScrollMove(scrollDir, scrollSpeed);

        CheckFinished();
    }

    void ScrollMove(Vector2 scrollDir, float scrollSpeed)
    {
        rt.anchoredPosition += scrollDir * scrollSpeed * Time.unscaledDeltaTime;
    }

    void CheckFinished()
    {
        float startToHereDistance = Vector3.Distance(startPos, rt.anchoredPosition);
        float startToEndDistance = Vector3.Distance(startPos, endPos);

        if(startToHereDistance > startToEndDistance)
        {
            scrollSpeed=0;

            GoToEndPos();

            OnScrollFinish.Invoke();
        }
    }

    public UnityEvent OnScrollStart;
    public UnityEvent OnScrollFinish;

    public void ScrollFaster(bool toggle)
    {
        if(toggle)
        scrollSpeed = defScrollSpeed * scrollFasterMult;

        else
        scrollSpeed = defScrollSpeed;
    }

    [ContextMenu("Record Start Position")]
    void RecordStartPos()
    {
        startPos = rt.anchoredPosition;
    }

    [ContextMenu("Record End Position")]
    void RecordEndPos()
    {
        endPos = rt.anchoredPosition;
    }
    
    [ContextMenu("Go To Start Position")]
    void GoToStartPos()
    {
        rt.anchoredPosition = startPos;
    }

    [ContextMenu("Go To End Position")]
    void GoToEndPos()
    {
        rt.anchoredPosition = endPos;
    }
    
}
