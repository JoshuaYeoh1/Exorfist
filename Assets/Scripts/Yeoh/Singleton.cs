using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using TMPro;

public class Singleton : MonoBehaviour
{
    public static Singleton instance;

    void Awake()
    {
        if(!instance)
        {
            instance=this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);

        Invoke("UnlockFPS", .1f);
        LoadVolume();
        AwakeTransition();
    }

    void Update()
    {
        UpdateReloadButton();
        //UpdateShuffleMusic();
        //UpdateShuffleAmbient();
    }

    void UnlockFPS()
    {
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 0;
    }


    /////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    
    [Header("Audio Manager")]
    public AudioSource SFXObject;
    public AudioSource musicSource, ambSource;
    public AudioClip[] mus, amb;

    void UpdateShuffleMusic()
    {
        if(!musicSource.isPlaying) PlayMusic();
    }
    void UpdateShuffleAmbient()
    {
        if(!ambSource.isPlaying) PlayAmbient();
    }

    void PlayMusic()
    {
        musicSource.Stop();
        FadeAudio(musicSource, true, .1f, 1);

        // if(SceneManager.GetActiveScene().buildIndex==0)
        // musicSource.clip = musMainMenu[Random.Range(0, musMainMenu.Length)];
        // else if(LevelCompleted)
        // musicSource.clip = musWin[Random.Range(0, musWin.Length)];
        // else if(SceneManager.GetActiveScene().buildIndex==1)
        // musicSource.clip = musLevel1[Random.Range(0, musLevel1.Length)];
        // else

        //musicSource.clip = mus[Random.Range(0, mus.Length)]; // default music
        //musicSource.Play();
    }

    void PlayAmbient()
    {
        ambSource.Stop();
        FadeAudio(ambSource, true, .1f, 1);

        ambSource.clip = amb[Random.Range(0, amb.Length)];
        ambSource.Play();
    }

    void ToggleAmb(bool toggle=true)
    {
        if(toggle)
        {
            ambSource.clip = amb[Random.Range(0, amb.Length)];
            ambSource.Play();
            //randAmbRt=StartCoroutine(RandAmb());
        }
        else
        {
            ambSource.Stop();
            //if(randAmbRt!=null) StopCoroutine(randAmbRt);
        }
    }

    public void ChangeMusic(float changeFadeTime=2)
    {
        if(changingMusicRt!=null) StopCoroutine(changingMusicRt);
        changingMusicRt = StartCoroutine(ChangingMusic(changeFadeTime));
    }
    Coroutine changingMusicRt;
    IEnumerator ChangingMusic(float changeFadeTime)
    {
        FadeAudio(musicSource, true, changeFadeTime, 0);

        yield return new WaitForSecondsRealtime(changeFadeTime);

        PlayMusic();
    }

    public void FadeAudio(AudioSource source, bool fadeIn, float fadeTime, float toVolume)
    {
        if(fadingAudioRt!=null) StopCoroutine(fadingAudioRt);
        fadingAudioRt = StartCoroutine(FadingAudio(source, fadeIn, fadeTime, toVolume));
    }
    Coroutine fadingAudioRt;
    IEnumerator FadingAudio(AudioSource source, bool fadeIn, float fadeTime, float toVolume)
    {
        if(!fadeIn)
        {
            float lengthOfSource = source.clip.samples/source.clip.frequency;
            yield return new WaitForSecondsRealtime(lengthOfSource-fadeTime);
        }

        float time=0, startVolume=source.volume;

        while(time<fadeTime)
        {
            time+=Time.deltaTime;
            source.volume = Mathf.Lerp(startVolume, toVolume, time/fadeTime);
            yield return null;
        }
        yield break;
    }

    //Coroutine randAmbRt;

    // IEnumerator RandAmb()
    // {
    //     while(true)
    //     {
    //         yield return new WaitForSeconds(Random.Range(2f,10f));

    //         for(int i=0;i<Random.Range(1,4);i++)
    //         {   
    //             playSFX(amb2, transform, false, true, Random.Range(.1f,1f), true);
    //         }
    //     }
    // }

    public void PlaySFX(AudioClip[] clip, Vector3 pos, bool dynamics=true, bool randPitch=true, float volume=1, bool randPan=false)
    {   
        AudioSource source = Instantiate(SFXObject, pos, Quaternion.identity);
        //source.transform.parent = transform;
        
        source.clip = clip[Random.Range(0,clip.Length)];
        source.volume = volume;
        SFXObject sfxobj = source.GetComponent<SFXObject>();
        sfxobj.randPitch = randPitch;
        sfxobj.dynamics = dynamics;
        if(randPan) source.panStereo = Random.Range(-1f,1f);

        source.Play();

        Destroy(source.gameObject, source.clip.length);
    }

    public void PlayVoice(AudioSource voiceSource, AudioClip[] clip, bool dynamics=true, bool randPitch=true, float volume=1, bool randPan=false)
    {   
        voiceSource.Stop();

        voiceSource.clip = clip[Random.Range(0,clip.Length)];
        voiceSource.volume = volume;
        SFXObject sfxobj = voiceSource.GetComponent<SFXObject>();
        sfxobj.randPitch = randPitch;
        sfxobj.dynamics = dynamics;
        if(randPan) voiceSource.panStereo = Random.Range(-1f,1f);

        voiceSource.Play();
    }

    public AudioMixer mixer;
    public const string MASTER_KEY = "masterVolume";
    public const string MUSIC_KEY = "musicVolume";
    public const string SFX_KEY = "sfxVolume";

    void LoadVolume()
    {   
        float masterVolume = PlayerPrefs.GetFloat(MASTER_KEY, 1f);
        float musicVolume = PlayerPrefs.GetFloat(MUSIC_KEY, 1f);
        float sfxVolume = PlayerPrefs.GetFloat(SFX_KEY, 1f);

        mixer.SetFloat(VolumeSettings.MIXER_MASTER, Mathf.Log10(masterVolume)*20);
        mixer.SetFloat(VolumeSettings.MIXER_MUSIC, Mathf.Log10(musicVolume)*20);
        mixer.SetFloat(VolumeSettings.MIXER_SFX, Mathf.Log10(sfxVolume)*20);
    }


    /////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    
    [Header("Game")]
    public bool controlsEnabled=true;
    public GameObject popUpTextPrefab;

    public void CamShake(float time=.2f)
    {
        GameObject.FindGameObjectWithTag("Cinemachine").GetComponent<CameraCinemachine>().Shake(time);
    }
    
    int tweenTimeLt=0;
    public void TweenTime(float to, float time=.01f)
    {
        LeanTween.cancel(tweenTimeLt);
        tweenTimeLt = LeanTween.value(Time.timeScale, to, time).setEaseInOutSine().setIgnoreTimeScale(true).setOnUpdate(UpdateTweenTime).id;
    }
    void UpdateTweenTime(float value)
    {
        Time.timeScale = value;
    }

    public void HitStop(float fadeIn=.05f, float wait=.01f, float fadeOut=.25f)
    {
        if(hitStoppingRt!=null) StopCoroutine(hitStoppingRt);
        hitStoppingRt = StartCoroutine(HitStopping(fadeIn, wait, fadeOut));
    }
    Coroutine hitStoppingRt;
    IEnumerator HitStopping(float fadeIn, float wait, float fadeOut)
    {
        TweenTime(0, fadeIn);

        yield return new WaitForSecondsRealtime(fadeIn + wait);

        TweenTime(1, fadeOut);
    }

    public void SpawnPopUpText(Vector3 pos, string text, Color color, float scaleMult=.35f, float force=2f)
    {
        GameObject popUp = Instantiate(popUpTextPrefab, pos, Quaternion.identity);
        popUp.hideFlags = HideFlags.HideInHierarchy;

        popUp.transform.localScale *= scaleMult;

        Rigidbody rb = popUp.GetComponent<Rigidbody>();
        rb.AddForce(Vector3.up*force, ForceMode.Impulse);

        TextMeshProUGUI[] tmps = popUp.GetComponentsInChildren<TextMeshProUGUI>();

        foreach(TextMeshProUGUI tmp in tmps)
        {
            tmp.text = text;
            tmp.color = color;
        }
    }


    /////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    
    [Header("Scene Manager")]
    public Animator transitionAnimator;
    GameObject transitionCanvas;
    CanvasGroup transitionCanvasGroup;
    public bool isTransitioning;
    int transitionTypes=1;

    public void AwakeTransition()
    {
        transitionCanvas=transitionAnimator.transform.parent.gameObject;
        transitionCanvasGroup=transitionCanvas.GetComponent<CanvasGroup>();

        transitionCanvas.SetActive(true);

        TransitionIn(Random.Range(0, transitionTypes));
    }

    Coroutine transitionRt;

    public void TransitionIn(int type)
    {
        CancelTransition();
        transitionRt = StartCoroutine(TransitioningIn(type));
    }
    IEnumerator TransitioningIn(int type)
    {
        EnableTransition();
        transitionAnimator.SetInteger("randin", type);

        yield return new WaitForSecondsRealtime(.1f);
        yield return new WaitForSecondsRealtime(transitionAnimator.GetCurrentAnimatorStateInfo(0).length);

        DisableTransition();
    }

    public void TransitionOut(int type, bool quit=false)
    {
        CancelTransition();
        transitionRt = StartCoroutine(TransitioningOut(type, quit));
    }
    IEnumerator TransitioningOut(int type, bool quit=false)
    {
        EnableTransition();
        transitionAnimator.SetInteger("randout", type);

        if(quit)
        {
            yield return new WaitForSecondsRealtime(.1f);

            // fadeAudio(musicSource, true, transitionAnimator.GetCurrentAnimatorStateInfo(0).length, 0);
            //fadeAudio(ambSource, true, transitionAnimator.GetCurrentAnimatorStateInfo(0).length, 0);

            yield return new WaitForSecondsRealtime(transitionAnimator.GetCurrentAnimatorStateInfo(0).length);

            Debug.Log("Quit");
            Application.Quit();
        }
    }

    public void TransitionTo(int sceneNumber) // please change to using Enum
    {
        CancelTransition();
        StartCoroutine(TransitioningTo(sceneNumber));
    }
    IEnumerator TransitioningTo(int sceneNumber, bool anim=true)
    {
        if(anim)
        {
            TransitionOut(Random.Range(0,transitionTypes));
            yield return new WaitForSecondsRealtime(.1f);
            yield return new WaitForSecondsRealtime(transitionAnimator.GetCurrentAnimatorStateInfo(0).length);
        }

        SceneManager.LoadScene(sceneNumber);

        if(anim) TransitionIn(Random.Range(0,transitionTypes));

        yield return new WaitForSecondsRealtime(.1f);

        // changeMusic();

        // toggleAmb(false);
        // if(SceneManager.GetActiveScene().buildIndex!=0) toggleAmb(true);
    }

    void EnableTransition()
    {
        isTransitioning=true;
        controlsEnabled=false;
        transitionCanvas.SetActive(true);
        transitionCanvasGroup.interactable=true;
        transitionCanvasGroup.blocksRaycasts=true;
    }

    void DisableTransition()
    {
        transitionCanvasGroup.interactable=false;
        transitionCanvasGroup.blocksRaycasts=false;
        controlsEnabled=true;
        isTransitioning=false;
    }

    void CancelTransition()
    {
        transitionCanvasGroup.interactable=false;
        transitionCanvasGroup.blocksRaycasts=false;
        if(transitionRt!=null) StopCoroutine(transitionRt);
        transitionAnimator.SetInteger("randin", -1);
        transitionAnimator.SetInteger("randout", -1);
        transitionCanvas.SetActive(false);
        isTransitioning=false;
    }

    void UpdateReloadButton()
    {
        if(Input.GetKeyDown(KeyCode.R)) ReloadScene();
    }

    public void ReloadScene()
    {
        //if(!transitioning && SceneManager.GetActiveScene().buildIndex!=0)
        TransitionTo(SceneManager.GetActiveScene().buildIndex);
    }

    public void RandomScene()
    {
        if(!isTransitioning) TransitionTo(Random.Range(0,6)); // dont use index numbers
    }


    /////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    
    [Header("SFX")]
    public AudioClip[] sfxTransition;
    //public AudioClip[] sfxExplode, sfxPlayerShoot, sfxAmmoPickup, sfxSubwoofer, sfxEnemyWalk;
    // public AudioClip[] sfxHitmarker, sfxPropSpawn;
    // public AudioClip[] sfxEnemySpawn, sfxEnemyHit, sfxEnemySwing, sfxEnemyPunch, sfxEnemyWing;
    // public AudioClip[] sfxEnemyVoiceAttack, sfxEnemyVoiceDie, sfxEnemyVoiceHurt, sfxEnemyVoiceIdle;
    // public AudioClip[] sfxUiLose, sfxUiClick;
}