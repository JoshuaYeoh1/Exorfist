using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Current;

    void Awake()
    {
        if(!Current) Current=this;
    }

    /////////////////////////////////////////////////////////////////////////////////////////////////

    [Header("Music")]
    public bool musicEnabled=true;

    public List<AudioSource> musicLayers = new List<AudioSource>();
    public AudioSource currentLayer;

    public AudioClip[] mainMenuMusics;
    public AudioClip[] idleMusics;
    public AudioClip[] combatMusics;

    Dictionary<AudioSource, AudioClip[]> layerClipsDict = new Dictionary<AudioSource, AudioClip[]>();

    void Start()
    {
        RecordDefVolumes();

        MuteAndEmptyAllLayers();
        SetInitialLayersAndClips();
    }

    public void MuteAndEmptyAllLayers()
    {
        foreach(AudioSource source in musicLayers)
        {
            source.loop=false;

            layerClipsDict.Clear();

            source.volume=0;
        }
    }

    public void SetInitialLayersAndClips()
    {
        ChangeClips(0, idleMusics);
        ChangeClips(1, combatMusics);

        ChangeLayer(0,0,0,0);
    }
    
    /////////////////////////////////////////////////////////////////////////////////////////////////

    void Update()
    {
        if(musicEnabled) AutoReplayAndShuffleAllLayers();
    }

    void AutoReplayAndShuffleAllLayers()
    {
        foreach(AudioSource source in musicLayers)
        {
            if(!source.isPlaying) Play(source);
        }
    }
    
    /////////////////////////////////////////////////////////////////////////////////////////////////

    public void ChangeMusic(AudioSource source, AudioClip[] clips, float fadeOutTime=3)
    {
        if(changingMusicRt!=null) StopCoroutine(changingMusicRt);
        changingMusicRt = StartCoroutine(ChangingMusic(source, clips, fadeOutTime));
    }
    public void ChangeMusic(int layerIndex, AudioClip[] clips, float fadeOutTime=3)
    {
        ChangeMusic(musicLayers[layerIndex], clips, fadeOutTime);
    }
    
    Coroutine changingMusicRt;
    IEnumerator ChangingMusic(AudioSource source, AudioClip[] clips, float outTime)
    {
        AudioManager.Current.TweenVolume(source, 0, outTime);

        if(outTime>0) yield return new WaitForSecondsRealtime(outTime);

        ChangeClips(source, clips);

        Play(source);
    }

    public void ChangeClips(AudioSource source, AudioClip[] clips)
    {
        if(layerClipsDict.ContainsKey(source))
        {
            layerClipsDict.Remove(source);
        }

        if(HasClips(clips))
        {
            layerClipsDict.Add(source, clips);
        }
    }
    public void ChangeClips(int layerIndex, AudioClip[] clips)
    {
        ChangeClips(musicLayers[layerIndex], clips);
    }

    void Play(AudioSource source)
    {
        if(!layerClipsDict.ContainsKey(source)) return;

        AudioClip[] clips = layerClipsDict[source];

        int randomClip = Random.Range(0, layerClipsDict[source].Length);

        source.clip = clips[randomClip];

        source.Play();
    }

    public void ChangeLayer(int layerIndex, float outTime=3, float waitTime=1, float inTime=3)
    {
        if(crossfadingLayerRt!=null) StopCoroutine(crossfadingLayerRt);
        crossfadingLayerRt = StartCoroutine(CrossfadingLayer(layerIndex, outTime, waitTime, inTime));
    }

    Coroutine crossfadingLayerRt;
    IEnumerator CrossfadingLayer(int layerIndex, float outTime, float waitTime, float inTime)
    {
        if(currentLayer) AudioManager.Current.TweenVolume(currentLayer, 0, outTime);

        currentLayer = musicLayers[layerIndex];

        if(waitTime>0) yield return new WaitForSecondsRealtime(waitTime);

        AudioManager.Current.TweenVolume(currentLayer, defVolumeDict[currentLayer], inTime);
    }

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    public bool HasClips(AudioClip[] clips)
    {
        return clips!=null && clips.Length>0;
    }
    public bool HasClips(List<AudioClip> clips)
    {
        return HasClips(clips.ToArray());
    }

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////

    Dictionary<AudioSource, float> defVolumeDict = new Dictionary<AudioSource, float>();
    
    void RecordDefVolumes()
    {
        foreach(AudioSource layer in musicLayers)
        {
            defVolumeDict[layer] = layer.volume;
        }
    }

    void ResetLayerVolume(AudioSource layer)
    {
        layer.volume = defVolumeDict[layer];
    }

}
