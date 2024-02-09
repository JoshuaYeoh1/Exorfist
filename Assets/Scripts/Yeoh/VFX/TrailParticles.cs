using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailParticles : MonoBehaviour
{
    public GameObject trailPrefab;
    public float scaleMult=1;
    public List<Transform> trailPoints = new List<Transform>();
    List<GameObject> myTrails = new List<GameObject>();
    bool trailIsOn;

    void Awake()
    {
        enableTrail();
    }

    public void enableTrail(bool toggle=true)
    {
        if(toggle && !trailIsOn)
        {
            spawnTrails();

            trailIsOn=true;
        }
        else if(!toggle && trailIsOn)
        {
            trailIsOn=false;

            if(myTrails.Count>0)
            {
                foreach(GameObject trail in myTrails)
                {
                    ParticleSystem ps = trail.GetComponent<ParticleSystem>();
                    if(ps) ps.Stop();
                }

                myTrails.Clear();
            }
        }
    }

    void spawnTrails()
    {
        for(int i=0;i<trailPoints.Count;i++)
        {
            myTrails.Add(Instantiate(trailPrefab, trailPoints[i].position, Quaternion.identity));

            if(scaleMult!=1) myTrails[i].transform.localScale = myTrails[i].transform.localScale*scaleMult;
        }

        foreach(GameObject trail in myTrails)
        {
            trail.GetComponent<ParticleSystem>().Play();
        }
    }

    void Update()
    {
        if(trailIsOn)
        {
            for(int i=0;i<trailPoints.Count;i++)
            {
                myTrails[i].transform.position = trailPoints[i].position;
            }
        }
    }

    void OnEnable()
    {
        enableTrail(true);
    }
    void OnDisable()
    {
        enableTrail(false);
    }
}
