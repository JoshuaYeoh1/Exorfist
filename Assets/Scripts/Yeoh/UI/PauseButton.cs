using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseButton : MonoBehaviour
{
    public void PauseGame()
    {
        GameEventSystem.Current.OnShowMenu("PauseMenu");
    }
    
    public void ResumeGame()
    {
        GameEventSystem.Current.OnHideMenu("PauseMenu");
    }
}
