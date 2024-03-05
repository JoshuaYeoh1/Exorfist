using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Scenes // must be in the same order as in the build settings, and case sensitive
{
    MainMenu,
    Level01,
    Level02,
    Level03,
    Yeoh3,
    PoCScene,
}

public class ScenesManager : Monostate<ScenesManager>
{
    public Animator transitionAnimator;
    public GameObject transitionCanvas;
    public CanvasGroup transitionCanvasGroup;

    public bool isTransitioning;
    int transitionTypes=1;

    void Awake()
    {
        AwakeTransition();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R)) ReloadScene();
    }

    void AwakeTransition()
    {
        PlayTransition("in", Random.Range(1, transitionTypes+1)); // choose a random transition
    }

    public void PlayTransition(string type, int i, bool quit=false)
    {
        playingTransitionRt = StartCoroutine(PlayingTransition(type, i, quit));
    }
    Coroutine playingTransitionRt;
    IEnumerator PlayingTransition(string type, int i, bool quit=false)
    {
        ShowTransition(true);

        transitionAnimator.Play(type+i, 0);

        if(type=="in")
        {
            yield return new WaitForSecondsRealtime(.1f);
            yield return new WaitForSecondsRealtime(GetTransitionLength());

            ShowTransition(false);
        }

        else if(type=="out")
        {
            yield return new WaitForSecondsRealtime(.1f);
            // FadeAudio(musicSource, true, GetTransitionLength(), 0);
            // FadeAudio(ambSource, true, GetTransitionLength(), 0);
            yield return new WaitForSecondsRealtime(GetTransitionLength());

            if(quit) Quit(false);
        }
    }

    public void TransitionTo(Scenes scene, bool anim=true)
    {
        if(!isTransitioning) playingTransitionRt = StartCoroutine(TransitioningTo(scene, anim));
    }
    IEnumerator TransitioningTo(Scenes scene, bool anim)
    {
        if(anim)
        {
            PlayTransition("out", Random.Range(1, transitionTypes+1));

            yield return new WaitForSecondsRealtime(.1f);
            yield return new WaitForSecondsRealtime(GetTransitionLength());
        }

        SceneManager.LoadScene(scene.ToString());

        if(anim) PlayTransition("in", Random.Range(1, transitionTypes+1));

        // ChangeMusic();

        // ToggleAmb(false);
        // if(!IsSceneMainMenu()) ToggleAmb(true);
    }

    public void ShowTransition(bool toggle)
    {
        if(!toggle) CancelTransition();

        transitionAnimator.gameObject.SetActive(toggle);
        transitionCanvasGroup.interactable=toggle;
        transitionCanvasGroup.blocksRaycasts=toggle;
        isTransitioning=toggle;

        if(toggle) CancelTransition();
    }

    public void CancelTransition()
    {
        if(playingTransitionRt!=null) StopCoroutine(playingTransitionRt);
        transitionAnimator.Play("cancel", 0);
    }

    public void LoadNextScene()
    {
        int nextSceneBuildIndex = SceneManager.GetActiveScene().buildIndex + 1;

        if(nextSceneBuildIndex < SceneManager.sceneCountInBuildSettings)
        {
            string nextSceneName = SceneManager.GetSceneByBuildIndex(nextSceneBuildIndex).name;

            TransitionTo(StringToEnum(nextSceneName));
        }
    }

    public void LoadPreviousScene()
    {
        int nextSceneBuildIndex = SceneManager.GetActiveScene().buildIndex - 1;

        if(nextSceneBuildIndex>=0)
        {
            string nextSceneName = SceneManager.GetSceneByBuildIndex(nextSceneBuildIndex).name;

            TransitionTo(StringToEnum(nextSceneName));
        }
    }

    public void ReloadScene()
    {
        if(!IsSceneMainMenu())
        TransitionTo(StringToEnum(SceneManager.GetActiveScene().name));
    }

    public void LoadMainMenu()
    {
        if(!IsSceneMainMenu())
        TransitionTo(Scenes.MainMenu);
    }

    public bool IsSceneMainMenu()
    {
        return SceneManager.GetActiveScene().name == Scenes.MainMenu.ToString();
    }

    public Scenes StringToEnum(string str)
    {
        return (Scenes)System.Enum.Parse(typeof(Scenes), str);
    }

    public float GetTransitionLength()
    {
        return transitionAnimator.GetCurrentAnimatorStateInfo(0).length;
    }

    public void Quit(bool anim=true)
    {
        if(anim) PlayTransition("out", Random.Range(1, transitionTypes+1), true);

        else
        {
            Debug.Log("Quit");
            Application.Quit();
        }
        
    }

    // public void RandomScene()
    // {
    //     if(!isTransitioning) TransitionTo(Random.Range(0,6));
    // }
}
