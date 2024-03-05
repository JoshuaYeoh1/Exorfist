using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoseStateButtons : MonoBehaviour
{
    public void SingletonRestart()
    {
        ScenesManager.current.ReloadScene();
    }

    public void QuitGame()
    {
        ScenesManager.current.Quit();
    }
}
