using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlHider : MonoBehaviour
{
    Player player;

    public GameObject[] attackBtns;
    public GameObject blockBtn, abilityBtn;
    public AbilityButtonToggle abilityButtonToggle;

    void Awake()
    {
        player=transform.root.GetComponent<Player>();
    }

    void Update()
    {
        if(player.canAttack)
        {
            foreach(GameObject atkBtn in attackBtns)
            {
                ToggleActive(atkBtn, true);
            }
        }
        else
        {
            foreach(GameObject atkBtn in attackBtns)
            {
                ToggleActive(atkBtn, false);
            }
        }

        if(player.canBlock) ToggleActive(blockBtn, true);
        else ToggleActive(blockBtn, false);

        if(player.canCast) ToggleActive(abilityBtn, true);
        else
        {
            if(abilityBtn.activeSelf)
            {
                abilityButtonToggle.HideAbilities();
                abilityBtn.SetActive(false);
            }
        }
    }

    void ToggleActive(GameObject obj, bool toggle=true)
    {
        if(obj.activeSelf!=toggle) obj.SetActive(toggle);
    }
}
