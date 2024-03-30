using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialSwitcher : MonoBehaviour
{
    public enum ObjectType
    {
        Char = 0,
        Env = 1,
    }

    public ObjectType objectType = ObjectType.Env;

    Renderer myRenderer;
    
    public Material toonMat;
    public Material toonOldMat;
    public Material urpMat;

    void Awake()
    {
        myRenderer=GetComponent<SkinnedMeshRenderer>();
        if(!myRenderer) myRenderer=GetComponent<MeshRenderer>();
    }

    void OnEnable()
    {
        GameEventSystem.Current.ChangeCharShaderTypeEvent += OnChangeCharShaderType;
        GameEventSystem.Current.ChangeEnvShaderTypeEvent += OnChangeEnvShaderType;
    }
    void OnDisable()
    {
        GameEventSystem.Current.ChangeCharShaderTypeEvent -= OnChangeCharShaderType;
        GameEventSystem.Current.ChangeEnvShaderTypeEvent -= OnChangeEnvShaderType;
    }
    
    void Start()
    {
        OnChangeCharShaderType(SettingsManager.Current.charShaderType);
        OnChangeEnvShaderType(SettingsManager.Current.envShaderType);
    }

    void OnChangeCharShaderType(ShaderType shaderType)
    {
        if(objectType!=ObjectType.Char) return;

        ChangeMaterial(shaderType);
    }

    void OnChangeEnvShaderType(ShaderType shaderType)
    {
        if(objectType!=ObjectType.Env) return;

        ChangeMaterial(shaderType);
    }

    void ChangeMaterial(ShaderType shaderType)
    {
        switch(shaderType)
        {
            case ShaderType.Toon: myRenderer.material = toonMat; break;
            case ShaderType.ToonOld: myRenderer.material = toonOldMat; break;
            case ShaderType.URP: myRenderer.material = urpMat; break;
        }
    }

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public ShaderType defaultShaderType = ShaderType.Toon;

    [ContextMenu("Change Material Now")]
    public void ChangeMaterialNow()
    {
        Awake();
        ChangeMaterial(defaultShaderType);
    }
}
