using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlHider : MonoBehaviour
{
    public Player player;

    public GameObject[] attackBtns;
    public GameObject blockBtn, abilityBtn;
    public AbilityButtonToggle abilityButtonToggle;

    void Update()
    {
        CheckAttackButton();
        CheckBlockButton();
        CheckAbilityButton();
    }

    void CheckAttackButton()
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
    }

    void CheckBlockButton()
    {
        if(player.canBlock) ToggleActive(blockBtn, true);

        else ToggleActive(blockBtn, false);
    }

    void CheckAbilityButton()
    {
        if(player.canCast) ToggleActive(abilityBtn, true);

        else HideAbilityButton();
    }

    void ToggleActive(GameObject obj, bool toggle=true)
    {
        if(obj.activeSelf!=toggle) obj.SetActive(toggle);
    }

    void HideAbilityButton()
    {
        if(abilityBtn.activeSelf)
        {
            abilityButtonToggle.HideAbilities();

            abilityBtn.SetActive(false);
        }
    }
}
