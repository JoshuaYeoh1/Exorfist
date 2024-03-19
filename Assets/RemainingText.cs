using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RemainingText : MonoBehaviour
{
    [SerializeField] private GameObject text;
    [SerializeField] private TextMeshProUGUI num;
    // Update is called once per frame
    void Update()
    {
        if (text.activeSelf == true)
        {
            num.text = AIDirector.instance.enemies.Count.ToString();
        }
        
        if(AIDirector.instance.enemies.Count == 0)
        {
            text.SetActive(false);
        }
        else
        {
            text.SetActive(true);
        }
    }
}
