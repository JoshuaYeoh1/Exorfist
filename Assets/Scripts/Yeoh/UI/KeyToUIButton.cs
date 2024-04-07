using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class KeyToUIButton : MonoBehaviour
{
    Button button;
    EventTrigger eventTrigger;

    void Awake()
    {
        button=GetComponent<Button>();
        eventTrigger=GetComponent<EventTrigger>();
    }

    public List<KeyCode> keys = new List<KeyCode>();

    bool pressed;

    void Update()
    {
        if(Time.timeScale==0) return;
        
        foreach(KeyCode key in keys)
        {
            if(Input.GetKeyDown(key))
            {
                if(pressed) return;

                button.onClick.Invoke();

                ExecuteEvents.Execute(eventTrigger.gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerDownHandler);

                pressed=true;
            }

            if(Input.GetKeyUp(key))
            {
                if(!pressed) return;

                ExecuteEvents.Execute(eventTrigger.gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerUpHandler);

                pressed=false;
            }
        }
    }
    
    void OnDisable()
    {
        pressed=false;
    }
}
