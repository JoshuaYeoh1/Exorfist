using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Current;

    void Awake()
    {
        if(!Current) Current=this;
    }
    
    /////////////////////////////////////////////////////////////////////////////////////////////////

    public const string CamSensKey="CamSens"; 
    public const string MaxFPSKey="MaxFPS"; 
    public const string VSyncKey="VSync"; 
    public const string HapticsKey="Haptics";

    [Header("Defaults")]
    public float camSens=0.05f;
    public int maxFPS=30;
    public int vSync=1;
    public int haptics=1;
    
    void Start()
    {
        LoadSettings();
    }

    void LoadSettings()
    {
        camSens = PlayerPrefs.GetFloat(CamSensKey, camSens);
        maxFPS = PlayerPrefs.GetInt(MaxFPSKey, maxFPS);
        vSync = PlayerPrefs.GetInt(VSyncKey, vSync);
        haptics = PlayerPrefs.GetInt(HapticsKey, haptics);

        GameEventSystem.Current.OnChangeCamSens(camSens);
        Application.targetFrameRate = maxFPS;
        QualitySettings.vSyncCount = vSync;
        GameEventSystem.Current.OnToggleHaptics(haptics==1);
    }

}
