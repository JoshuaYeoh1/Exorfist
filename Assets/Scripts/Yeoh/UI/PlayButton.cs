using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayButton : MonoBehaviour
{
    public void LoadNextScene()
    {
        ScenesManager.Current.LoadNextScene();
    }

    public void SfxUIPlay()
    {
        AudioManager.Current.PlaySFX(SFXManager.Current.sfxUIPlay, transform.position, false, false);
    }
}
