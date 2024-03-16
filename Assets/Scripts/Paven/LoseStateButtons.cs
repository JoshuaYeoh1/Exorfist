using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoseStateButtons : MonoBehaviour
{
    public void Respawn()
    {
        ScenesManager.Current.ReloadScene();
    }

    public void QuitGame()
    {
        ScenesManager.Current.Quit();
    }
}
