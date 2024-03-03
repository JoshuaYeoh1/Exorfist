using UnityEngine;
using Cinemachine;

public class Cinetouch : MonoBehaviour
{
    CinemachineFreeLook cineFreeLook;
    TouchField touchField;
<<<<<<< HEAD
    ClosestObjectFinder finder;
=======
    Player player;
>>>>>>> main

    public float senstivityX=.1f, senstivityY=-.1f;

    public float recenterWaitTime=3;
    float lastTouchedTime;

    void Awake()
    {
        cineFreeLook=GetComponent<CinemachineFreeLook>();
        touchField = GameObject.FindGameObjectWithTag("TouchField").GetComponent<TouchField>();
<<<<<<< HEAD
        finder = GameObject.FindGameObjectWithTag("Player").GetComponent<ClosestObjectFinder>();
=======
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
>>>>>>> main
    }

    void Update()
    {
        if(touchField)
        {
<<<<<<< HEAD
            cineFreeLook.m_XAxis.Value += touchField.TouchDist.x * 200 * SenstivityX * Time.deltaTime;
        
            cineFreeLook.m_YAxis.Value += touchField.TouchDist.y * SenstivityY * Time.deltaTime;
=======
            cineFreeLook.m_XAxis.Value += touchField.TouchDist.x * 200 * senstivityX * Time.unscaledDeltaTime;
        
            cineFreeLook.m_YAxis.Value += touchField.TouchDist.y * senstivityY * Time.unscaledDeltaTime;
>>>>>>> main

            CheckRecenter();
        }
    }

    void CheckRecenter()
    {
<<<<<<< HEAD
        if(touchField.Pressed || finder.target)
=======
        if(touchField.Pressed || player.target)
>>>>>>> main
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
