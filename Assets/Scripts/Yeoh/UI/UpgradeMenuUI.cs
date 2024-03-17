using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeMenuUI : MonoBehaviour
{
    public void CloseUpgradeMenu()
    {
        GameEventSystem.Current.OnHideMenu("UpgradeMenu");
    }
}
