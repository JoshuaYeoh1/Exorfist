using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialManager : MonoBehaviour
{
    public void AddMaterial(Renderer renderer, Material materialToAdd)
    {
        List<Material> materialList = new List<Material>(renderer.materials);

        materialList.Add(materialToAdd);

        renderer.materials = materialList.ToArray();
    }

    public void RemoveMaterial(Renderer renderer, Material materialToRemove)
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
}