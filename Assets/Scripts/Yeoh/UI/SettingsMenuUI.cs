using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenuUI : MonoBehaviour
{
    public Slider camSensSlider;
    public Slider maxFPSSlider;
    public Toggle vsyncToggle;
    public Toggle hapticsToggle;
    
    void Awake()
    {
        camSensSlider.onValueChanged.AddListener(ChangeCamSens);
        maxFPSSlider.onValueChanged.AddListener(ChangeMaxFPS);
        vsyncToggle.onValueChanged.AddListener(ToggleVSync);
        hapticsToggle.onValueChanged.AddListener(ToggleHaptics);
    }

    void Start() // if player prefs key not found, use the second param as default
    {
        camSensSlider.value = PlayerPrefs.GetFloat(SettingsManager.CamSensKey, SettingsManager.Current.camSens);
        maxFPSSlider.value = PlayerPrefs.GetInt(SettingsManager.MaxFPSKey, SettingsManager.Current.maxFPS);
        vsyncToggle.isOn = PlayerPrefs.GetInt(SettingsManager.VSyncKey, SettingsManager.Current.vSync)==1 ? true : false;
        hapticsToggle.isOn = PlayerPrefs.GetInt(SettingsManager.HapticsKey, SettingsManager.Current.haptics)==1 ? true : false;
    }

    public void ChangeCamSens(float value)
    {
        GameEventSystem.Current.OnChangeCamSens(value);
    }

    public void ChangeMaxFPS(float value)
    {
        Application.targetFrameRate = Mathf.RoundToInt(value);
    }

    public void ToggleVSync(bool toggle)
    {
        QualitySettings.vSyncCount = toggle ? 1 : 0;
    }

    public void ToggleHaptics(bool toggle)
    {
        GameEventSystem.Current.OnToggleHaptics(toggle);
    }

    void OnDisable()
    {
        PlayerPrefs.SetFloat(SettingsManager.CamSensKey, camSensSlider.value);
        PlayerPrefs.SetInt(SettingsManager.MaxFPSKey, Mathf.RoundToInt(maxFPSSlider.value));
        PlayerPrefs.SetInt(SettingsManager.VSyncKey, vsyncToggle.isOn ? 1 : 0);
        PlayerPrefs.SetInt(SettingsManager.HapticsKey, hapticsToggle.isOn ? 1 : 0);
    }
}
