using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyCounter : MonoBehaviour
{
    //[SerializeField] private GameObject text;
    public TextMeshProUGUI counterTMP;
    public UIHider hider;

    void Update()
    {
        hider.value = AIDirector.instance.enemies.Count;
        counterTMP.text = AIDirector.instance.enemies.Count.ToString();
        
        // if(AIDirector.instance.enemies.Count == 0)
        // {
        //     text.SetActive(false);
        // }
        // else
        // {
        //     text.SetActive(true);
        // }
    }
}
