using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtInfo
{
    public Collider coll { get; set; }
    public GameObject owner { get; set; }
    public float dmg { get; set; }
    public float dmgBlock { get; set; }
    public float kbForce { get; set; }
    public Vector3 contactPoint { get; set; }
    public float speedDebuffMult { get; set; }
    public float stunTime { get; set; }
    public bool hasSweepingEdge { get; set; }
    public bool doImpact { get; set; }
    public bool doShake { get; set; }
    public bool doHitstop { get; set; }
    public bool doShockwave { get; set; }
    public bool unparryable { get; set; }
    
    public HurtInfo()
    {
        coll = null;
        owner = null;
        dmg = 0;
        dmgBlock = 0;
        kbForce = 0;
        contactPoint = Vector3.zero;
        speedDebuffMult = 0;
        stunTime = 0;
        hasSweepingEdge = false;
        doImpact = false;
        doShake = false;
        doHitstop = false;
        doShockwave = false;
        unparryable = false;
    }
}
