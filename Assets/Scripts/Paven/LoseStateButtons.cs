using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoseStateButtons : MonoBehaviour
{
    public void SingletonRestart()
    {
        Singleton.instance.ReloadScene();
    }

    public void QuitGame()
    {
        //Application.Quit();
        Singleton.instance.TransitionOut(0, true);
    }
}
