using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialManager : MonoBehaviour
{
    public void AddMaterial(GameObject target, Material materialToAdd)
    {
        Renderer[] renderers = target.GetComponents<Renderer>();

        if(renderers.Length>0)
        {
            for(int i=0; i<renderers.Length; i++)
            {
                AddRendererMaterial(renderers[i], materialToAdd);
            }
        }

        Renderer[] childRenderers = target.GetComponentsInChildren<Renderer>();
        
        if(childRenderers.Length>0)
        {
            for(int i=0; i<childRenderers.Length; i++)
            {
                AddRendererMaterial(childRenderers[i], materialToAdd);
            }
        }

        SkinnedMeshRenderer[] smrs = target.GetComponents<SkinnedMeshRenderer>();

        if(smrs.Length>0)
        {
            for(int i=0; i<smrs.Length; i++)
            {
                AddRendererMaterial(smrs[i], materialToAdd);
            }
        }

        Renderer[] childSmrs = target.GetComponentsInChildren<SkinnedMeshRenderer>();
        
        if(childSmrs.Length>0)
        {
            for(int i=0; i<childSmrs.Length; i++)
            {
                AddRendererMaterial(childSmrs[i], materialToAdd);
            }
        }
    }

    public void AddRendererMaterial(Renderer renderer, Material materialToAdd)
    {
        List<Material> materialList = new List<Material>(renderer.materials);

        materialList.Add(materialToAdd);

        renderer.materials = materialList.ToArray();
    }

    public void RemoveMaterial(GameObject target, Material materialToRemove)
    {
        Renderer[] renderers = target.GetComponents<Renderer>();

        if(renderers.Length>0)
        {
            for(int i=0; i<renderers.Length; i++)
            {
                RemoveRendererMaterial(renderers[i], materialToRemove);
            }
        }

        Renderer[] childRenderers = target.GetComponentsInChildren<Renderer>();
        
        if(childRenderers.Length>0)
        {
            for(int i=0; i<childRenderers.Length; i++)
            {
                RemoveRendererMaterial(childRenderers[i], materialToRemove);
            }
        }

        SkinnedMeshRenderer[] smrs = target.GetComponents<SkinnedMeshRenderer>();

        if(smrs.Length>0)
        {
            for(int i=0; i<smrs.Length; i++)
            {
                RemoveRendererMaterial(smrs[i], materialToRemove);
            }
        }

        Renderer[] childSmrs = target.GetComponentsInChildren<SkinnedMeshRenderer>();
        
        if(childSmrs.Length>0)
        {
            for(int i=0; i<childSmrs.Length; i++)
            {
                RemoveRendererMaterial(childSmrs[i], materialToRemove);
            }
        }
    }

    public void RemoveRendererMaterial(Renderer renderer, Material materialToRemove)
    {
        List<Material> materialList = new List<Material>(); // new empty list

        foreach(Material mat in renderer.materials)
        {
            if(mat.shader != materialToRemove.shader) // ignore if the current material equals the one to remove
            {
                materialList.Add(mat);
            }
        }

        renderer.materials = materialList.ToArray();
    }

    public List<Material> GetMaterials(GameObject target, Material material)
    {
        List<Material> materialList = new List<Material>();

        Renderer[] renderers = target.GetComponents<Renderer>();

        if(renderers.Length>0)
        {
            for(int i=0; i<renderers.Length; i++)
            {
                materialList.AddRange(GetRendererMaterials(renderers[i], material));
            }
        }

        Renderer[] childRenderers = target.GetComponentsInChildren<Renderer>();
        
        if(childRenderers.Length>0)
        {
            for(int i=0; i<childRenderers.Length; i++)
            {
                materialList.AddRange(GetRendererMaterials(childRenderers[i], material));
            }
        }

        SkinnedMeshRenderer[] smrs = target.GetComponents<SkinnedMeshRenderer>();

        if(smrs.Length>0)
        {
            for(int i=0; i<smrs.Length; i++)
            {
                materialList.AddRange(GetRendererMaterials(smrs[i], material));
            }
        }

        Renderer[] childSmrs = target.GetComponentsInChildren<SkinnedMeshRenderer>();
        
        if(childSmrs.Length>0)
        {
            for(int i=0; i<childSmrs.Length; i++)
            {
                materialList.AddRange(GetRendererMaterials(childSmrs[i], material));
            }
        }

        return materialList;
    }

    public List<Material> GetRendererMaterials(Renderer renderer, Material material)
    {
        List<Material> materialList = new List<Material>();

        foreach(Material mat in renderer.materials)
        {
            if(mat.shader == material.shader)
            {
                materialList.Add(mat);
            }
        }

        return materialList;
    }
}