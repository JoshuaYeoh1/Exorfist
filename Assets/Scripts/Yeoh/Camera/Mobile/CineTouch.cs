using UnityEngine;
using Cinemachine;

public class Cinetouch : MonoBehaviour
{
    CinemachineFreeLook cineFreeLook;
    
    public TouchField touchField;
    public Player player;

    public float senstivityX=.05f, senstivityY=-.05f;

    public float recenterWaitTime=3;
    float lastTouchedTime;

    void Awake()
    {
        cineFreeLook=GetComponent<CinemachineFreeLook>();
    }

    void OnEnable()
    {
        GameEventSystem.Current.ChangeCamSensEvent += OnChangeCamSens;
    }
    void OnDisable()
    {
        GameEventSystem.Current.ChangeCamSensEvent -= OnChangeCamSens;
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

    void OnChangeCamSens(float value)
    {
        senstivityX = senstivityX<0 ? -value : value;
        senstivityY = senstivityY<0 ? -value : value;
    }

    void CheckRecenter()
    {
        if(touchField.Pressed || player.target || player.move.input!=Vector3.zero)
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
