using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionAnim : MonoBehaviour
{
    public void PlaySfxWhoosh()
    {
        AudioManager.Current.PlaySFX(AudioManager.Current.sfxTransition, transform.position, false);
    }
}