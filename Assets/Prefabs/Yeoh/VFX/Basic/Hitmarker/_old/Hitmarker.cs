// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class Hitmarker : MonoBehaviour
// {
//     public GameObject hitmarkerPrefab;

//     public void SpawnHitmarker(Vector3 pos, Color color, float scaleMult=1, float destroyAfter=.1f)
//     {
//         SpriteRenderer sr = Instantiate(hitmarkerPrefab, pos, Quaternion.identity).GetComponent<SpriteRenderer>();
//         sr.gameObject.hideFlags = HideFlags.HideInHierarchy;

//         sr.color = color;

//         sr.transform.localScale *= scaleMult;

//         Destroy(sr.gameObject, destroyAfter);
//     }
// }
