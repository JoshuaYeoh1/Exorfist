using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailParticles : MonoBehaviour
{
    public GameObject trailPrefab;

    public List<Transform> trailPoints = new List<Transform>();
    List<GameObject> myTrails = new List<GameObject>();

    public float scaleMult=1;
    public bool enableOnAwake=true;

    void Awake()
    {
        if(enableOnAwake) EnableTrail();
    }

    bool trailIsOn;

    public void EnableTrail(bool toggle=true)
    {
        if(toggle && !trailIsOn)
        {
            SpawnTrails();

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

    void SpawnTrails()
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
        if(enableOnAwake) EnableTrail(true);
    }
    void OnDisable()
    {
        EnableTrail(false);
    }
}
