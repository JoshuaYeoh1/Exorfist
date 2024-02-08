using UnityEngine;
using Cinemachine;

public class Cinetouch : MonoBehaviour
{
    CinemachineFreeLook cineFreeLook;
    TouchField touchField;

    public float SenstivityX = .2f, SenstivityY = -.2f;

    void Awake()
    {
        cineFreeLook=GetComponent<CinemachineFreeLook>();

        touchField = GameObject.FindGameObjectWithTag("TouchField").GetComponent<TouchField>();
    }

    void Update()
    {
        if(touchField)
        {
            if(cineFreeLook)
            {
                cineFreeLook.m_XAxis.Value += touchField.TouchDist.x * 200 * SenstivityX * Time.deltaTime;
            
                cineFreeLook.m_YAxis.Value += touchField.TouchDist.y * SenstivityY * Time.deltaTime;
            }
        }
        else
        {
            Debug.Log("Bruh where's the touch field?");
        }
    }
}
