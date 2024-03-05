using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager current;

    public AudioMixer mixer;
    public const string MASTER_KEY = "masterVolume";
    public const string MUSIC_KEY = "musicVolume";
    public const string SFX_KEY = "sfxVolume";

    public AudioSource musicSource, ambSource;

    public AudioClip[] defaultMusics;
    public AudioClip[] defaultAmbients;

    AudioClip[] currentMusics;
    AudioClip[] currentAmbients; 

    void Awake()
    {
        if(!current) current=this;
        else Destroy(gameObject);

        LoadVolume();
        //AwakeAudio();
    }

    void Update()
    {
        //UpdateShuffleMusic();
        //UpdateShuffleAmbient();
    }

    void LoadVolume()
    {   
        float masterVolume = PlayerPrefs.GetFloat(MASTER_KEY, 1f);
        float musicVolume = PlayerPrefs.GetFloat(MUSIC_KEY, 1f);
        float sfxVolume = PlayerPrefs.GetFloat(SFX_KEY, 1f);

        mixer.SetFloat(VolumeSettings.MIXER_MASTER, Mathf.Log10(masterVolume)*20);
        mixer.SetFloat(VolumeSettings.MIXER_MUSIC, Mathf.Log10(musicVolume)*20);
        mixer.SetFloat(VolumeSettings.MIXER_SFX, Mathf.Log10(sfxVolume)*20);
    }

    void AwakeAudio()
    {
        if(currentMusics.Length==0) currentMusics = defaultMusics; // if current music is empty

        if(currentAmbients.Length==0) currentAmbients = defaultAmbients; // if current ambient is empty
    }

    void UpdateShuffleMusic()
    {
        if(!musicSource.isPlaying) PlayMusic();
    }

    void PlayMusic()
    {
        musicSource.Stop();
        FadeAudio(musicSource, 1, .1f); // make sure that volume is on

        musicSource.clip = currentMusics[Random.Range(0, currentMusics.Length)];
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

        currentMusics = clip;
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

        ambSource.clip = currentAmbients[Random.Range(0, currentAmbients.Length)];
        ambSource.Play();
    }

    void ToggleAmb(bool toggle=true)
    {
        if(toggle)
        {
            ambSource.clip = currentAmbients[Random.Range(0, currentAmbients.Length)];
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

    public AudioClip[] sfxTransition;
    //public AudioClip[] sfxExplode, sfxPlayerShoot, sfxAmmoPickup, sfxSubwoofer, sfxEnemyWalk;
    // public AudioClip[] sfxHitmarker, sfxPropSpawn;
    // public AudioClip[] sfxEnemySpawn, sfxEnemyHit, sfxEnemySwing, sfxEnemyPunch, sfxEnemyWing;
    // public AudioClip[] sfxEnemyVoiceAttack, sfxEnemyVoiceDie, sfxEnemyVoiceHurt, sfxEnemyVoiceIdle;
    // public AudioClip[] sfxUiLose, sfxUiClick;
}
