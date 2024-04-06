using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1 : MonoBehaviour
{
    void Start()
    {
        UpgradeManager.Current.ResetAll();
    }
}
