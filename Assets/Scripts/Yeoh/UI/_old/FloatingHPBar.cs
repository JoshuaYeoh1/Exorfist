// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class FloatingHPBar : MonoBehaviour
// {
//     HPManager hp;

//     public GameObject hpBarPrefab;
//     public Transform hpBarPos;

//     [HideInInspector] public HPBar floatingHpBar;

//     void Start()
//     {
//         hp=GetComponent<HPManager>();

//         floatingHpBar = Instantiate(hpBarPrefab,hpBarPos.position,Quaternion.identity).GetComponent<HPBar>();
//         floatingHpBar.gameObject.hideFlags = HideFlags.HideInHierarchy;

//         hp.hpBarFill = floatingHpBar.hpBarFill;
//         //hp.hider = floatingHpBar.hider;
//     }

//     void Update()
//     {
//         if(floatingHpBar) floatingHpBar.transform.position = hpBarPos.position;
//     }

//     void OnDestroy()
//     {
//         if(floatingHpBar)
//         Destroy(floatingHpBar.gameObject);
//     }
// }
