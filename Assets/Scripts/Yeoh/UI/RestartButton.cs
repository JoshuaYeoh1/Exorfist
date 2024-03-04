using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartButton : MonoBehaviour
{
    public void ReloadScene()
    {
        if(!Singleton.instance.IsSceneMainMenu())
        Singleton.instance.TransitionTo(SceneManager.GetActiveScene().name);
    }
}
