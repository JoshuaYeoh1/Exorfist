using UnityEngine;
using Cinemachine;

public class Cinetouch : MonoBehaviour
{
    CinemachineFreeLook cineFreeLook;
    TouchField touchField;
    Player player;

    public float senstivityX=.1f, senstivityY=-.1f;

    public float recenterWaitTime=3;
    float lastTouchedTime;

    void Awake()
    {
        cineFreeLook=GetComponent<CinemachineFreeLook>();
        touchField = GameObject.FindGameObjectWithTag("TouchField").GetComponent<TouchField>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    void Update()
    {
        if(touchField)
        {
            cineFreeLook.m_XAxis.Value += touchField.TouchDist.x * 200 * senstivityX * Time.unscaledDeltaTime;
        
            cineFreeLook.m_YAxis.Value += touchField.TouchDist.y * senstivityY * Time.unscaledDeltaTime;

            CheckRecenter();
        }
    }

    void CheckRecenter()
    {
        if(touchField.Pressed || player.target || player.move.moveInput!=Vector3.zero)
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
