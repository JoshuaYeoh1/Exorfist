using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonRestartScript : MonoBehaviour
{
    public void SingletonRestart()
    {
        Singleton.instance.ReloadScene();
    }
}
