using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffsetMeshColor : MonoBehaviour
{
    public GameObject skinsGroup;
    public float rOffset=.5f, gOffset=-.5f, bOffset=-.5f;

    public Renderer[] renderers;
    public List<Color> defaultColors = new List<Color>();
    public List<Color> defaultEmissionColors = new List<Color>();

    void Start()
    {
        RecordColor();
    }

    public void RecordColor()
    {
        renderers=skinsGroup.GetComponentsInChildren<Renderer>();

        for(int j=0; j<renderers.Length; j++)
        {
            for(int i=0; i<renderers[j].materials.Length; i++)
            {
                defaultColors.Add(renderers[j].materials[i].color);

                defaultEmissionColors.Add(renderers[j].materials[i].GetColor("_EmissionColor"));
            }
        }
    }

    public void OffsetColor(float rOffset=0, float gOffset=0, float bOffset=0, bool offsetEmission=true)
    {
        for(int j=0; j<renderers.Length; j++)
        {
            for(int i=0; i<renderers[j].materials.Length; i++)
            {
                int index = i + (j * renderers[j].materials.Length);

                Color newColor = new Color(defaultColors[index].r+rOffset,
                                        defaultColors[index].g+gOffset,
                                        defaultColors[index].b+bOffset);

                renderers[j].materials[i].color = newColor;

                if(offsetEmission)
                {
                    Color newEmissionColor = new Color(defaultEmissionColors[index].r+rOffset,
                                                defaultEmissionColors[index].g+gOffset,
                                                defaultEmissionColors[index].b+bOffset);
            
                    renderers[j].materials[i].SetColor("_EmissionColor", newEmissionColor);
                }
            }
        }
    }

    public void FlashColor(float time, bool offsetEmission=true)
    {
        if(flashRt!=null) StopCoroutine(flashRt);
        flashRt = StartCoroutine(FlashingColor(time, offsetEmission));
    }
    Coroutine flashRt;
    IEnumerator FlashingColor(float t, bool e)
    {
        OffsetColor(rOffset, gOffset, bOffset, e);
        yield return new WaitForSeconds(t);
        OffsetColor();
    }

    // void Update() // testing
    // {
    //     if(Input.GetKeyDown(KeyCode.Backspace)) FlashColor(.1f);
    // }
}
