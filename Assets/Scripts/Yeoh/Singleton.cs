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
        
        AwakeTransition();
        LoadVolume();
        //AwakeAudio();
    }

    void UnlockFPS()
    {
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 0;
    }

    void Update()
    {
        UpdateFixedDeltaTime();
        UpdateReloadButton();
        //UpdateShuffleMusic();
        //UpdateShuffleAmbient();
    }

    void UpdateFixedDeltaTime() // to fix physics stuttering
    {
        if(Time.timeScale==1)
        {
            if(Time.fixedDeltaTime!=.02f)
            Time.fixedDeltaTime=.02f; // default value
        }
        else // if slow mo
        {
            if(Time.fixedDeltaTime!=.02f*Time.timeScale)
            Time.fixedDeltaTime = .02f*Time.timeScale;
        }
    }

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public void CamShake(float time=.2f, float amp=1.5f, float freq=2)
    {
        GameObject.FindGameObjectWithTag("Cinemachine").GetComponent<CameraCinemachine>().Shake(time, amp, freq);

        Vibrator.Vibrate();
    }

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////

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

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////

    [HideInInspector] public bool canHitStop=true;

    public void HitStop(float fadeIn=.05f, float wait=.01f, float fadeOut=.25f)
    {
        if(canHitStop)
        {
            if(hitStoppingRt!=null) StopCoroutine(hitStoppingRt);
            hitStoppingRt = StartCoroutine(HitStopping(fadeIn, wait, fadeOut));
        }
    }
    Coroutine hitStoppingRt;
    IEnumerator HitStopping(float fadeIn, float wait, float fadeOut)
    {
        TweenTime(0, fadeIn);
        yield return new WaitForSecondsRealtime(fadeIn + wait);
        TweenTime(1, fadeOut);
    }

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public GameObject popUpTextPrefab;

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

    public enum Scenes // must be in the same order as in the build settings, and case sensitive
    {
        MainMenu,
        Level01,
        Level02,
        Level03,
    }

    public void AwakeTransition()
    {
        transitionCanvas=transitionAnimator.transform.parent.gameObject;
        transitionCanvas.SetActive(true);

        transitionCanvasGroup=transitionCanvas.GetComponent<CanvasGroup>();

        TransitionIn(Random.Range(0, transitionTypes)); // choose a random transition
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
            // fadeAudio(ambSource, true, transitionAnimator.GetCurrentAnimatorStateInfo(0).length, 0);

            yield return new WaitForSecondsRealtime(transitionAnimator.GetCurrentAnimatorStateInfo(0).length);

            Debug.Log("Quit");
            Application.Quit();
        }
    }

    void EnableTransition()
    {
        isTransitioning=true;
        //controlsEnabled=false;
        transitionCanvas.SetActive(true);
        transitionCanvasGroup.interactable=true;
        transitionCanvasGroup.blocksRaycasts=true;
    }

    void DisableTransition()
    {
        transitionCanvasGroup.interactable=false;
        transitionCanvasGroup.blocksRaycasts=false;
        //controlsEnabled=true;
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

    public void TransitionTo(string sceneName, bool anim=true)
    {
        if(!isTransitioning)
        {
            CancelTransition();
            StartCoroutine(TransitioningTo(sceneName, anim));
        }
    }
    IEnumerator TransitioningTo(string sceneName, bool anim)
    {
        if(anim)
        {
            TransitionOut(Random.Range(0,transitionTypes));
            yield return new WaitForSecondsRealtime(.1f);
            yield return new WaitForSecondsRealtime(transitionAnimator.GetCurrentAnimatorStateInfo(0).length);
        }

        SceneManager.LoadScene(sceneName);

        if(anim) TransitionIn(Random.Range(0,transitionTypes));

        yield return new WaitForSecondsRealtime(.1f);

        // ChangeMusic();

        // ToggleAmb(false);
        // if(!IsSceneMainMenu()) ToggleAmb(true);
    }    

    void UpdateReloadButton()
    {
        if(Input.GetKeyDown(KeyCode.R)) ReloadScene();
    }

    public void ReloadScene()
    {
        if(!IsSceneMainMenu())
        TransitionTo(SceneManager.GetActiveScene().name);
    }

    public bool IsSceneMainMenu()
    {
        return SceneManager.GetActiveScene().name == Scenes.MainMenu.ToString();
    }

    public void LoadMainMenu()
    {
        if(!IsSceneMainMenu())
        TransitionTo(Scenes.MainMenu.ToString());
    }

    public void LoadNextScene()
    {
        int nextSceneBuildIndex = SceneManager.GetActiveScene().buildIndex + 1;

        if(nextSceneBuildIndex < SceneManager.sceneCountInBuildSettings)
        {
            string nextSceneName = SceneManager.GetSceneByBuildIndex(nextSceneBuildIndex).name;

            TransitionTo(nextSceneName);
        }
    }

    public void LoadPreviousScene()
    {
        int nextSceneBuildIndex = SceneManager.GetActiveScene().buildIndex - 1;

        if(nextSceneBuildIndex>=0)
        {
            string nextSceneName = SceneManager.GetSceneByBuildIndex(nextSceneBuildIndex).name;

            TransitionTo(nextSceneName);
        }
    }

    // public void RandomScene()
    // {
    //     if(!isTransitioning) TransitionTo(Random.Range(0,6));
    // }


    /////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    
    [Header("Audio Manager")]
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

    public AudioSource musicSource, ambSource;

    public AudioClip[] musicClipsDefault;
    public AudioClip[] ambientClipsDefault;

    AudioClip[] currentMusicClips, currentAmbientClips; 

    void AwakeAudio()
    {
        if(currentMusicClips.Length<=0) currentMusicClips = musicClipsDefault; // current music is empty

        if(currentAmbientClips.Length<=0) currentAmbientClips = ambientClipsDefault; // current ambient is empty
    }

    void UpdateShuffleMusic()
    {
        if(!musicSource.isPlaying) PlayMusic();
    }

    void PlayMusic()
    {
        musicSource.Stop();
        FadeAudio(musicSource, 1, .1f); // make sure that volume is on

        musicSource.clip = currentMusicClips[Random.Range(0, currentMusicClips.Length)];
        musicSource.Play();
    }

    public void ChangeMusic(AudioClip[] clip, float fadeOutTime=2)
    {
        if(changingMusicRt!=null) StopCoroutine(changingMusicRt);
        changingMusicRt = StartCoroutine(ChangingMusic(clip, fadeOutTime));
    }
    Coroutine changingMusicRt;
    IEnumerator ChangingMusic(AudioClip[] clip, float fadeOutTime)
    {
        FadeAudio(musicSource, 0, fadeOutTime);

        yield return new WaitForSecondsRealtime(fadeOutTime);

        currentMusicClips = clip;
        PlayMusic();
    }

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////

    void UpdateShuffleAmbient()
    {
        if(!ambSource.isPlaying) PlayAmbient();
    }

    void PlayAmbient()
    {
        ambSource.Stop();
        FadeAudio(ambSource, 1, .1f); // make sure that volume is on

        ambSource.clip = currentAmbientClips[Random.Range(0, currentAmbientClips.Length)];
        ambSource.Play();
    }

    void ToggleAmb(bool toggle=true)
    {
        if(toggle)
        {
            ambSource.clip = currentAmbientClips[Random.Range(0, currentAmbientClips.Length)];
            ambSource.Play();
            //randAmbRt=StartCoroutine(RandAmb());
        }
        else
        {
            ambSource.Stop();
            //if(randAmbRt!=null) StopCoroutine(randAmbRt);
        }
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

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public void FadeAudio(AudioSource source, float to, float time)
    {
        if(fadingAudioRt!=null) StopCoroutine(fadingAudioRt);
        fadingAudioRt = StartCoroutine(FadingAudio(source, to, time));
    }
    Coroutine fadingAudioRt;
    IEnumerator FadingAudio(AudioSource source, float to, float time)
    {
        float _time=0;

        while(_time<time)
        {
            _time+=Time.deltaTime;
            source.volume = Mathf.Lerp(source.volume, to, _time/time);
            yield return null;
        }
        yield break;
    }

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////

    [Header("SFX")]
    public AudioSource SFXObject;
    public AudioClip[] sfxTransition;
    //public AudioClip[] sfxExplode, sfxPlayerShoot, sfxAmmoPickup, sfxSubwoofer, sfxEnemyWalk;
    // public AudioClip[] sfxHitmarker, sfxPropSpawn;
    // public AudioClip[] sfxEnemySpawn, sfxEnemyHit, sfxEnemySwing, sfxEnemyPunch, sfxEnemyWing;
    // public AudioClip[] sfxEnemyVoiceAttack, sfxEnemyVoiceDie, sfxEnemyVoiceHurt, sfxEnemyVoiceIdle;
    // public AudioClip[] sfxUiLose, sfxUiClick;

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
}