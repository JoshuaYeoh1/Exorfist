using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton : MonoBehaviour
{
    public static Singleton Current;

    public int chi;

    void Awake()
    {
        if(!Current)
        {
            Current=this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
        
        Invoke("UnlockFPS", .1f);
    }

    void UnlockFPS()
    {
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 0;
    }

    void Update()
    {
        //UpdateFixedDeltaTime();
    }

    void UpdateFixedDeltaTime() // to fix physics stuttering // CAUSES RAGDOLLS TO FLY UP WHEN TWEENING TIME FAST
    {
        if(Time.timeScale==1)
        {
            if(Time.fixedDeltaTime!=.02f)
            Time.fixedDeltaTime=.02f; // default value
        }
        else // if timescale changed
        {
            if(Time.fixedDeltaTime!=.02f*Time.timeScale)
            Time.fixedDeltaTime = .02f*Time.timeScale;
        }
    }
}