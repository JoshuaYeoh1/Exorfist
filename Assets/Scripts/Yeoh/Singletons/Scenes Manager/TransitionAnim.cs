using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionAnim : MonoBehaviour
{
    public void PlaySfxWhoosh()
    {
        AudioManager.current.PlaySFX(AudioManager.current.sfxTransition, transform.position, false);
    }
}
