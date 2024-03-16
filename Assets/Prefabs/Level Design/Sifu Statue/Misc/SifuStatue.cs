using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class SifuStatue : MonoBehaviour
{
    GameObject player;
    Player p;

    void Update()
    {
        CheckHighlight();
    }

    [HideInInspector] public bool inRange;

    void OnTriggerEnter(Collider other)
    {
        ReliableOnTriggerExit.NotifyTriggerEnter(other, gameObject, OnTriggerExit);

        Rigidbody otherRb = other.attachedRigidbody;

        if(otherRb && otherRb.gameObject.tag=="Player")
        {
            if(!inRange)
            {
                inRange=true;
                
                player=otherRb.gameObject;

                p=player.GetComponent<Player>();
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        ReliableOnTriggerExit.NotifyTriggerExit(other, gameObject);

        Rigidbody otherRb = other.attachedRigidbody;

        if(otherRb && otherRb.gameObject.tag=="Player")
        {
            if(inRange)
            {
                inRange=false;
            }
        }
    }
    
    public GameObject statueModel;
    public GameObject indicator;
    public Material outlineMaterial;
    bool highlighted;

    void CheckHighlight()
    {
        if(inRange && p.canMeditate)
        {
            Highlight();
        }
        else if(!inRange || !p.canMeditate)
        {
            Unhighlight();
        }
    }

    void Highlight()
    {
        if(highlighted) return;

        highlighted=true;

        indicator.SetActive(true);

        ModelManager.Current.AddMaterial(statueModel, outlineMaterial);
    }

    void Unhighlight()
    {
        if(!highlighted) return;

        highlighted=false;

        indicator.SetActive(false);

        ModelManager.Current.RemoveMaterial(statueModel, outlineMaterial);
    }

    public Transform sitSpot;
    public CinemachineVirtualCamera statueCamera;
    
    public void SitPlayer()
    {
        PlayerMeditate pm = player.GetComponent<PlayerMeditate>();

        pm.Meditate(sitSpot);

        CameraManager.Current.ChangeCamera(statueCamera);

        p.respawnPoint.position = sitSpot.position;
        p.respawnPoint.rotation = Quaternion.Euler(0, sitSpot.eulerAngles.y, 0);

        Debug.Log("Respawn Point Set!");
    }
}
