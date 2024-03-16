using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public GameObject explosionPrefab;
    public float vfxScaleMult=5, blastRadius=10, blastForce=1000;
    public LayerMask blastLayerMask;

    public void Explode(Vector3 pos, float dmg=0)
    {
        CameraManager.Current.Shake();

        //Singleton.instance.playSFX(Singleton.instance.sfxExplode, pos, false);

        if(explosionPrefab)
        {
            GameObject explo = Instantiate(explosionPrefab, pos, Quaternion.identity);
            if(vfxScaleMult!=1) explo.transform.localScale = explo.transform.localScale*vfxScaleMult;
        }
        
        Collider[] collidersToDmg =  Physics.OverlapSphere(pos, blastRadius, blastLayerMask);

        foreach(Collider other in collidersToDmg)
        {
            if(dmg>0)
            {
                if(other.attachedRigidbody.gameObject.tag=="Player")
                {
                    Player player = other.attachedRigidbody.GetComponent<Player>();

                    //if(player) player.hit(dmg);
                }
                else if(other.attachedRigidbody.gameObject.tag=="Enemy")
                {
                    //Enemy enemy = other.attachedRigidbody.GetComponent<Enemy>();

                    //if(enemy) enemy.die();
                }
            }
        }

        Collider[] collidersToPush =  Physics.OverlapSphere(pos, blastRadius*1.5f, blastLayerMask);

        foreach(Collider other in collidersToPush)
        {
            Rigidbody otherRb = other.attachedRigidbody.GetComponent<Rigidbody>();

            if(otherRb)
            {
                otherRb.velocity=Vector3.zero;
                otherRb.AddExplosionForce(blastForce, pos, blastRadius*1.5f);
            }
        }

        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, .5f, 0, .5f);
        Gizmos.DrawWireSphere(transform.position, blastRadius);
    }
}
