using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMeditate : MonoBehaviour
{
    Player player;
    Rigidbody rb;

    void Awake()
    {
        player=GetComponent<Player>();
        rb=GetComponent<Rigidbody>();
    }

    void OnEnable()
    {
        GameEventSystem.Current.MeditateEnterEvent += OnMeditateEnter;
        GameEventSystem.Current.MeditateExitEvent += OnMeditateExit;
    }
    void OnDisable()
    {
        GameEventSystem.Current.MeditateEnterEvent -= OnMeditateEnter;
        GameEventSystem.Current.MeditateExitEvent -= OnMeditateExit;
    }
    
    public void Meditate(Transform sitSpot)
    {
        if(!player.canMeditate) return;

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

    void OnMeditateEnter(GameObject monk)
    {
        if(monk!=gameObject) return;

        Invoke("ShowUpgradeMenu", 1);
    }

    void ShowUpgradeMenu()
    {
        HideUpgradeMenu();
    }

    void HideUpgradeMenu()
    {
        player.anim.CrossFade("stand", .5f, 2, 0);

        CameraManager.Current.ChangeCameraToDefault();
    }

    void OnMeditateExit(GameObject monk)
    {
        if(monk!=gameObject) return;

        player.sm.TransitionToState(PlayerStateMachine.PlayerStates.Idle);

        rb.isKinematic=false;
    }
}
