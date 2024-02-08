using UnityEngine;
using Cinemachine;

public class Cinetouch : MonoBehaviour
{
    CinemachineFreeLook cineFreeLook;
    TouchField touchField;

    public float SenstivityX = .2f, SenstivityY = -.2f;

    public float recenterWaitTime=3;
    float lastTouchedTime;

    void Awake()
    {
        cineFreeLook=GetComponent<CinemachineFreeLook>();

        touchField = GameObject.FindGameObjectWithTag("TouchField").GetComponent<TouchField>();
    }

    void Update()
    {
        if(touchField)
        {
            CheckPressing();

            cineFreeLook.m_XAxis.Value += touchField.TouchDist.x * 200 * SenstivityX * Time.deltaTime;
        
            cineFreeLook.m_YAxis.Value += touchField.TouchDist.y * SenstivityY * Time.deltaTime;
        }
        else Debug.Log("woi no touch field brother");
    }

    void CheckPressing()
    {
        if(touchField.Pressed)
        {
            if(cineFreeLook.m_RecenterToTargetHeading.m_enabled)
            cineFreeLook.m_RecenterToTargetHeading.m_enabled=false;

            lastTouchedTime=Time.time;
        }
        else
        {
            if(Time.time > lastTouchedTime+recenterWaitTime)
            {
                if(!cineFreeLook.m_RecenterToTargetHeading.m_enabled)
                cineFreeLook.m_RecenterToTargetHeading.m_enabled=true;
            }
        }
    }
}
