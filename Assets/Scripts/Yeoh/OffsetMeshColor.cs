using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffsetMeshColor : MonoBehaviour
{
    public GameObject skinsGroup;
    public float rOffset=.5f, gOffset=-.5f, bOffset=-.5f;
    public bool ignoreEmission=true;

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

                if(!ignoreEmission) defaultEmissionColors.Add(renderers[j].materials[i].GetColor("_EmissionColor"));
            }
        }
    }

    public void OffsetColor(float rOffset=0, float gOffset=0, float bOffset=0, bool eOffset=true)
    {
        for(int j=0; j<renderers.Length; j++)
        {
            for(int i=0; i<renderers[j].materials.Length; i++)
            {
                int index = i + (j * renderers[j].materials.Length);

                if(index < defaultColors.Count)
                {
                    Color newColor = new Color(defaultColors[index].r+rOffset,
                                            defaultColors[index].g+gOffset,
                                            defaultColors[index].b+bOffset);

                    renderers[j].materials[i].color = newColor;
                }

                if(eOffset && !ignoreEmission && index < defaultEmissionColors.Count)
                {
                    Color newEmissionColor = new Color(defaultEmissionColors[index].r+rOffset,
                                                defaultEmissionColors[index].g+gOffset,
                                                defaultEmissionColors[index].b+bOffset);
            
                    renderers[j].materials[i].SetColor("_EmissionColor", newEmissionColor);
                }
            }
        }
    }

    Coroutine flashRt;
    
    public void FlashColor(float time=.1f, bool eOffset=true)
    {
        if(flashRt!=null) StopCoroutine(flashRt);
        flashRt = StartCoroutine(FlashingColor(time, eOffset));
    }
    IEnumerator FlashingColor(float t, bool e)
    {
        OffsetColor(rOffset, gOffset, bOffset, e);
        yield return new WaitForSeconds(t);
        OffsetColor();
    }

    public void FlashColor(float time=.1f, float rOffset=0, float gOffset=0, float bOffset=0, bool eOffset=true)
    {
        if(flashRt!=null) StopCoroutine(flashRt);
        flashRt = StartCoroutine(FlashingColor(time, rOffset, gOffset, bOffset, eOffset));
    }
    IEnumerator FlashingColor(float t, float r, float g, float b, bool e)
    {
        OffsetColor(r, g, b, e);
        yield return new WaitForSeconds(t);
        OffsetColor();
    }

    // void Update() // testing
    // {
    //     if(Input.GetKeyDown(KeyCode.Backspace)) FlashColor(.1f);
    // }
}
