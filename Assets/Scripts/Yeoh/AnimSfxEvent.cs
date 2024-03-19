using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimSfxEvent : MonoBehaviour
{
    public void SfxEnemy2Jump()
    {
        AudioManager.Current.PlaySFX(SFXManager.Current.sfxEnemy2Jump, transform.position);
        AudioManager.Current.PlaySFX(SFXManager.Current.sfxCharge, transform.position);
    }

    public void SfxFstPlayer()
    {
        AudioManager.Current.PlaySFX(SFXManager.Current.sfxFstPlayer, transform.position);
    }

    public void SfxFstEnemy1()
    {
        AudioManager.Current.PlaySFX(SFXManager.Current.sfxFstEnemy1, transform.position);
    }

    public void SfxFstEnemy2()
    {
        AudioManager.Current.PlaySFX(SFXManager.Current.sfxFstEnemy2, transform.position);
    }
    
    public void SfxSwingBig()
    {
        AudioManager.Current.PlaySFX(SFXManager.Current.sfxSwingBig, transform.position);
    }

    public void SfxSwingSmall()
    {
        AudioManager.Current.PlaySFX(SFXManager.Current.sfxSwingSmall, transform.position);
    }

    
}
