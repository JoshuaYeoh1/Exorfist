using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPopUpText : MonoBehaviour
{
    public Transform tf;
    public float force=3.5f;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P)) Singleton.instance.SpawnPopUpText(tf.position, "YOOOO!", Color.red);
    }
}
