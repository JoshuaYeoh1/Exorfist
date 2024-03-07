using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPopUpText : MonoBehaviour
{
    public Transform tf;
    public float force=3.5f;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P)) // move to vfx manager later
        VFXManager.Current.SpawnPopUpText(tf.position, "YOOOO!", Color.red);
    }
}
