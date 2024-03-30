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

    public GameObject charToonBtn;
    public GameObject charToonOldBtn;
    public GameObject charUrpBtn;
    int charShaderType=1;

    public GameObject envToonBtn;
    public GameObject envToonOldBtn;
    public GameObject envUrpBtn;
    int envShaderType=0;
    
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

        charShaderType = PlayerPrefs.GetInt(SettingsManager.CharShaderTypeKey, (int)SettingsManager.Current.charShaderType);
        envShaderType = PlayerPrefs.GetInt(SettingsManager.EnvShaderTypeKey, (int)SettingsManager.Current.envShaderType);
        ChangeCharShaderType(charShaderType);
        ChangeEnvShaderType(envShaderType);
    }

    public void ChangeCamSens(float value)
    {
        GameEventSystem.Current.OnChangeCamSens(value);

        PlayerPrefs.SetFloat(SettingsManager.CamSensKey, value);
    }

    public void ChangeMaxFPS(float value)
    {
        Application.targetFrameRate = Mathf.RoundToInt(value);

        PlayerPrefs.SetInt(SettingsManager.MaxFPSKey, Mathf.RoundToInt(value));
    }

    public void ToggleVSync(bool toggle)
    {
        QualitySettings.vSyncCount = toggle ? 1 : 0;

        PlayerPrefs.SetInt(SettingsManager.VSyncKey, toggle ? 1 : 0);
    }

    public void ToggleHaptics(bool toggle)
    {
        GameEventSystem.Current.OnToggleHaptics(toggle);

        PlayerPrefs.SetInt(SettingsManager.HapticsKey, toggle ? 1 : 0);
    }

    public void ChangeCharShaderType(int i)
    {
        switch(i)
        {
            case 0:
            {
                charToonBtn.SetActive(true);
                charToonOldBtn.SetActive(false);
                charUrpBtn.SetActive(false);
            } break;

            case 1:
            {
                charToonBtn.SetActive(false);
                charToonOldBtn.SetActive(true);
                charUrpBtn.SetActive(false);
            } break;
            
            case 2:
            {
                charToonBtn.SetActive(false);
                charToonOldBtn.SetActive(false);
                charUrpBtn.SetActive(true);
            } break;
        }

        GameEventSystem.Current.OnChangeCharShaderType((ShaderType)i);

        charShaderType=i;
        
        PlayerPrefs.SetInt(SettingsManager.CharShaderTypeKey, i);
    }

    public void ChangeEnvShaderType(int i)
    {
        switch(i)
        {
            case 0:
            {
                envToonBtn.SetActive(true);
                envToonOldBtn.SetActive(false);
                envUrpBtn.SetActive(false);
            } break;

            case 1:
            {
                envToonBtn.SetActive(false);
                envToonOldBtn.SetActive(true);
                envUrpBtn.SetActive(false);
            } break;
            
            case 2:
            {
                envToonBtn.SetActive(false);
                envToonOldBtn.SetActive(false);
                envUrpBtn.SetActive(true);
            } break;
        }

        GameEventSystem.Current.OnChangeEnvShaderType((ShaderType)i);
        
        envShaderType=i;

        PlayerPrefs.SetInt(SettingsManager.EnvShaderTypeKey, i);
    }

    void OnDisable()
    {
        PlayerPrefs.SetFloat(SettingsManager.CamSensKey, camSensSlider.value);
        PlayerPrefs.SetInt(SettingsManager.MaxFPSKey, Mathf.RoundToInt(maxFPSSlider.value));
        PlayerPrefs.SetInt(SettingsManager.VSyncKey, vsyncToggle.isOn ? 1 : 0);
        PlayerPrefs.SetInt(SettingsManager.HapticsKey, hapticsToggle.isOn ? 1 : 0);

        PlayerPrefs.SetInt(SettingsManager.CharShaderTypeKey, charShaderType);
        PlayerPrefs.SetInt(SettingsManager.EnvShaderTypeKey, envShaderType);
    }
}
