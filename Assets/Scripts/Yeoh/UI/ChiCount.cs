using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChiCount : MonoBehaviour
{
    UIHider hider;

    public TextMeshProUGUI countTMP;

    void Awake()
    {
        hider=GetComponent<UIHider>();
    }

    void Update()
    {
        int chi = UpgradeManager.Current.chi;

        countTMP.text = $"{chi}";

        hider.value=chi;
    }
}
