using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockwaveVFX : MonoBehaviour
{
    public GameObject shockwavePrefab;

    public void SpawnShockwave(Vector3 pos, Color color, float scaleMult=1)
    {
        ParticleSystem shock = Instantiate(shockwavePrefab, pos, Quaternion.identity).GetComponent<ParticleSystem>();
        shock.gameObject.hideFlags = HideFlags.HideInHierarchy;

        ParticleSystem.MainModule main = shock.main;
        main.startColor = color;

        shock.transform.localScale *= scaleMult;
    }
}
