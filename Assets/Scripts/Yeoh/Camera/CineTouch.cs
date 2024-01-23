using UnityEngine;
using Cinemachine;

public class Cinetouch : MonoBehaviour
{
    CinemachineFreeLook cineFreeLook;
    public TouchField touchField;

    public float SenstivityX = .2f, SenstivityY = -.2f;

    void Awake()
    {
        cineFreeLook=GetComponent<CinemachineFreeLook>();
    }

    void Update()
    {
        if(touchField && cineFreeLook)
        {
            cineFreeLook.m_XAxis.Value += touchField.TouchDist.x * 200 * SenstivityX * Time.deltaTime;
            
            cineFreeLook.m_YAxis.Value += touchField.TouchDist.y * SenstivityY * Time.deltaTime;
        }
    }
}
