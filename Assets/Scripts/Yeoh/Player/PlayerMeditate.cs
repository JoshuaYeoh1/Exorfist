using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMeditate : MonoBehaviour
{
    Player player;
    Rigidbody rb;
    HPManager hp;

    bool isMeditating;
    public float regenSpeed=3;

    void Awake()
    {
        player=GetComponent<Player>();
        rb=GetComponent<Rigidbody>();
        hp=GetComponent<HPManager>();
    }

    void OnEnable()
    {
        GameEventSystem.Current.MeditateEnterEvent += OnMeditateEnter;
        GameEventSystem.Current.MeditateExitEvent += OnMeditateExit;
        GameEventSystem.Current.HideMenuEvent += OnHideMenu;
    }
    void OnDisable()
    {
        GameEventSystem.Current.MeditateEnterEvent -= OnMeditateEnter;
        GameEventSystem.Current.MeditateExitEvent -= OnMeditateExit;
        GameEventSystem.Current.HideMenuEvent -= OnHideMenu;
    }
    
    public void Meditate(Transform sitSpot)
    {
        if(!player.canMeditate) return;

        isMeditating=true;

        StartCoroutine(Meditating(sitSpot));
    }

    IEnumerator Meditating(Transform sitSpot)
    {
        player.sm.TransitionToState(PlayerStateMachine.PlayerStates.Pause);

        rb.isKinematic=true;

        LeanTween.move(gameObject, sitSpot.position, 1).setEaseInOutSine();
        LeanTween.rotateY(gameObject, sitSpot.eulerAngles.y, 1).setEaseInOutSine();

        //change camera

        yield return new WaitForSeconds(1);

        player.anim.CrossFade("sit", .5f, 2, 0);
    }

    void OnMeditateEnter(GameObject monk) // after animation finish
    {
        if(monk!=gameObject) return;

        Invoke("ShowUpgradeMenu", 1);

        hp.regenHp = regenSpeed;

        player.ResetAbilityCooldowns();
    }

    void ShowUpgradeMenu()
    {
        if(GameEventSystem.Current)
        {
            GameEventSystem.Current.OnShowMenu("UpgradeMenu");
        }
        else Unmeditate();
    }

    void OnHideMenu(string menuName)
    {
        if(menuName!="UpgradeMenu") return;

        Unmeditate();
    }

    void Unmeditate()
    {
        if(isMeditating)
        {
            isMeditating=false;

            player.anim.CrossFade("stand", .5f, 2, 0);

            CameraManager.Current.ChangeCameraToDefault();

            hp.regenHp = hp.defaultRegenHp;
        }
    }

    void OnMeditateExit(GameObject monk) // after animation finish
    {
        if(monk!=gameObject) return;

        player.sm.TransitionToState(PlayerStateMachine.PlayerStates.Idle);

        rb.isKinematic=false;
    }
}
