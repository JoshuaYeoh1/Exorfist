using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAIAttackTimer : MonoBehaviour
{
    private bool atkCooldown = false;
    [SerializeField] private float atkCooldownTime; //this field is here so that the designers can input how long they want the cooldown time to be (in seconds)
    [SerializeField] private float atkCooldownTimeCurrent;

    // Update is called once per frame
    void Update()
    {
        if (atkCooldown)
        {
            atkCooldownTimeCurrent -= Time.deltaTime;

            if(atkCooldownTimeCurrent <= 0)
            {
                atkCooldown = false;
                //Debug.Log("Attack ready");
            }
        }
    }

    public void StartAtkCooldown()
    {
        atkCooldown = true;
        atkCooldownTimeCurrent = atkCooldownTime;
    }

    public bool GetAtkCooldownBool() { return atkCooldown; }    
}
