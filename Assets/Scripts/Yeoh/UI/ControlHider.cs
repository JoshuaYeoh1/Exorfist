using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlHider : MonoBehaviour
{
    public Player player;

    public GameObject[] attackBtns;
    public GameObject blockBtn, abilityBtn;
    public AbilityButtonToggle abilityButtonToggle;
    public GameObject joystick;

    void Update()
    {
        CheckAttackButton();
        //CheckBlockButton();
        CheckAbilityButton();
        CheckJoystick();
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

    void HideAbilityButton()
    {
        if(abilityBtn.activeSelf)
        {
            abilityButtonToggle.HideAbilities();

            abilityBtn.SetActive(false);
        }
    }

    void CheckJoystick()
    {
        if(!player.isPaused && player.isAlive) ToggleActive(joystick, true);

        else HideJoystick();
    }

    void HideJoystick()
    {
        if(joystick.activeSelf)
        {
            player.move.NoInput();

            joystick.SetActive(false);
        }
    }

    void ToggleActive(GameObject obj, bool toggle=true)
    {
        if(obj.activeSelf!=toggle) obj.SetActive(toggle);
    }

    
}
