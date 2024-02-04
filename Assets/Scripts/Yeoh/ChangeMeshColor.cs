using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMeshColor : MonoBehaviour
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

    public void ChangeColor(string reset="")
    {
        for(int j=0; j<renderers.Length; j++)
        {
            for(int i=0; i<renderers[j].materials.Length; i++)
            {
                if(reset=="reset")
                {
                    int index = i + (j * renderers[j].materials.Length);

                    renderers[j].materials[i].color = defaultColors[index];
                
                    renderers[j].materials[i].SetColor("_EmissionColor", defaultEmissionColors[index]);
                }
                else
                {
                    Color newColor = new Color(defaultColors[i].r+rOffset,
                                            defaultColors[i].g+gOffset,
                                            defaultColors[i].b+bOffset);

                    renderers[j].materials[i].color = newColor;

                    Color newEmissionColor = new Color(defaultEmissionColors[i].r+rOffset,
                                                    defaultEmissionColors[i].g+gOffset,
                                                    defaultEmissionColors[i].b+bOffset);
                
                    renderers[j].materials[i].SetColor("_EmissionColor", newEmissionColor);
                }
            }
        }
    }

    public void FlashColor(float time)
    {
        if(flashRt!=null) StopCoroutine(flashRt);
        flashRt = StartCoroutine(FlashingColor(time));
    }
    Coroutine flashRt;
    IEnumerator FlashingColor(float t)
    {
        ChangeColor();
        yield return new WaitForSeconds(t);
        ChangeColor("reset");
    }

    // void Update() // testing
    // {
    //     if(Input.GetKeyDown(KeyCode.Backspace)) FlashColor(.1f);
    // }
}
