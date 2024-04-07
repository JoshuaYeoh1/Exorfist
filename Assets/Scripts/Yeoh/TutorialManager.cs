using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public List<GameObject> pcTutorialPrefabs = new List<GameObject>();
    public List<GameObject> mobileTutorialPrefabs = new List<GameObject>();

    List<GameObject> tutorialPrefabs = new List<GameObject>();

    void Start()
    {
        tutorialPrefabs = Singleton.Current.IsWindows() ? pcTutorialPrefabs : mobileTutorialPrefabs;
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    void OnEnable()
    {
        GameEventSystem.Current.StartTutorialEvent += OnStartTutorial;
    }
    void OnDisable()
    {
        GameEventSystem.Current.StartTutorialEvent -= OnStartTutorial;
    }

    void OnStartTutorial()
    {
        StartCoroutine(StartingTutorial());
    }

    public float tutorialInterval=5;

    IEnumerator StartingTutorial()
    {
        while(Singleton.Current.doTutorial)
        {
            AdvanceTutorial();

            yield return new WaitForSeconds(tutorialInterval);
        }
    }
    
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    int index=0;

    void AdvanceTutorial()
    {
        if(!Singleton.Current.doTutorial) return;

        DestroyTutorial();        

        if(index<tutorialPrefabs.Count)
        {
            SpawnTutorial();

            index++;
        }
        else FinishTutorial();
    }

    void FinishTutorial()
    {
        index=0;
        Singleton.Current.doTutorial = false;
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    GameObject currentTutorial;
    
    void SpawnTutorial()
    {
        currentTutorial = Instantiate(tutorialPrefabs[index], transform.position, Quaternion.identity);
    }

    void DestroyTutorial()
    {
        if(currentTutorial) Destroy(currentTutorial);
    }
}
