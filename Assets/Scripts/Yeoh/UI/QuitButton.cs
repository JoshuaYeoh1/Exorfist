using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitButton : MonoBehaviour
{
    public void QuitGame()
    {
        ScenesManager.Current.Quit();
    }
}
