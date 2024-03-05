using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelManager : Monostate<ModelManager>
{
    // GETTERS
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public List<Renderer> GetRenderers(GameObject target)
    {
        List<Renderer> renderers = new List<Renderer>();

        renderers.AddRange(target.GetComponents<Renderer>());
        renderers.AddRange(target.GetComponentsInChildren<Renderer>());

        return renderers;
    }
    
    public List<SkinnedMeshRenderer> GetSkinnedMeshRenderers(GameObject target)
    {
        List<SkinnedMeshRenderer> renderers = new List<SkinnedMeshRenderer>();

        renderers.AddRange(target.GetComponents<SkinnedMeshRenderer>());
        renderers.AddRange(target.GetComponentsInChildren<SkinnedMeshRenderer>());

        return renderers;
    }
    
    public List<MeshFilter> GetMeshFilters(GameObject target)
    {
        List<MeshFilter> meshFilters = new List<MeshFilter>();

        meshFilters.AddRange(target.GetComponents<MeshFilter>());
        meshFilters.AddRange(target.GetComponentsInChildren<MeshFilter>());

        return meshFilters;
    }

    public List<Mesh> GetMeshes(GameObject target)
    {
        List<Mesh> meshes = new List<Mesh>();

        foreach(SkinnedMeshRenderer smr in GetSkinnedMeshRenderers(target))
        {
            meshes.Add(smr.sharedMesh);
        }
        foreach(MeshFilter mf in GetMeshFilters(target))
        {
            meshes.Add(mf.sharedMesh);
        }
        
        return meshes;
    }

    public List<Vector3> GetVertices(GameObject target)
    {
        List<Vector3> vertices = new List<Vector3>();

        foreach(Mesh mesh in GetMeshes(target))
        {
            vertices.AddRange(mesh.vertices);
        }
        
        return vertices;
    }

    public List<Material> GetMaterials(GameObject target, Material materialToGet=null)
    {
        List<Material> materials = new List<Material>();

        foreach(Renderer renderer in GetRenderers(target))
        {
            foreach(Material material in renderer.materials)
            {
                if(materialToGet)
                {
                    if(material.shader == materialToGet.shader)
                    {
                        materials.Add(material);
                    }
                }
                else
                {
                    materials.Add(material);
                }
            }
        }

        return materials;
    }

    public List<Color> GetColors(GameObject target)
    {
        List<Color> colors = new List<Color>();

        foreach(Material material in GetMaterials(target))
        {
            colors.Add(material.color);
        }

        return colors;
    }

    public List<Color> GetEmissionColors(GameObject target)
    {
        List<Color> emissionColors = new List<Color>();

        foreach(Material material in GetMaterials(target))
        {
            emissionColors.Add(material.GetColor("_EmissionColor"));
        }

        return emissionColors;
    }

    // MATERIAL ADD/REMOVE
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public void AddMaterial(GameObject target, Material materialToAdd)
    {
        List<Material> materials = GetMaterials(target);

        materials.Add(materialToAdd);

        foreach(Renderer renderer in GetRenderers(target))
        {
            renderer.materials = materials.ToArray();
        }
    }

    public void RemoveMaterial(GameObject target, Material materialToRemove)
    {
        List<Material> newMaterials = new List<Material>();

        foreach(Material material in GetMaterials(target))
        {
            if(material.shader != materialToRemove.shader)
            {
                newMaterials.Add(material);
            }
        }

        foreach(Renderer renderer in GetRenderers(target))
        {
            renderer.materials = newMaterials.ToArray();
        }
    }

    // OFFSET MESH COLORS
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    Dictionary<Material, Color> originalColors = new Dictionary<Material, Color>();
    Dictionary<Material, Color> originalEmissionColors = new Dictionary<Material, Color>();

    public void RecordColors(GameObject target)
    {
        foreach(Material material in GetMaterials(target))
        {
            if(material.HasProperty("_Color") && !originalColors.ContainsKey(material))
            {
                originalColors[material] = material.color;
            }

            if(material.HasProperty("_EmissionColor") && !originalEmissionColors.ContainsKey(material))
            {
                originalEmissionColors[material] = material.GetColor("_EmissionColor");
            }
        }
    }

    public void OffsetColor(GameObject target, float rOffset, float gOffset, float bOffset)
    {
        RecordColors(target);

        Color colorOffset = new Color(rOffset, gOffset, bOffset);

        foreach(Material material in GetMaterials(target))
        {
            if(material.HasProperty("_Color"))
            {
                material.color += colorOffset;
            }

            if(material.HasProperty("_EmissionColor"))
            {
                material.SetColor("_EmissionColor", material.GetColor("_EmissionColor") + colorOffset);
            }
        }
    }

    public void RevertColor(GameObject target)
    {
        List<Material> materialsToRevert = new List<Material>(); // new list

        foreach(Material material in GetMaterials(target))
        {
            if(originalColors.ContainsKey(material) || originalEmissionColors.ContainsKey(material))
            {
                materialsToRevert.Add(material); // Add materials to the list for reverting
            }
        }

        foreach(Material material in materialsToRevert)
        {
            if(material.HasProperty("_Color") && originalColors.ContainsKey(material))
            {
                material.color = originalColors[material];

                originalColors.Remove(material); // clean up
            }
            
            if(material.HasProperty("_EmissionColor") && originalEmissionColors.ContainsKey(material))
            {
                material.SetColor("_EmissionColor", originalEmissionColors[material]);

                originalEmissionColors.Remove(material); //clean up
            }
        }
    }
    
    public void FlashColor(GameObject target, float time=.1f, float rOffset=0, float gOffset=0, float bOffset=0)
    {
        StartCoroutine(FlashingColor(target, time, rOffset, gOffset, bOffset));
    }
    IEnumerator FlashingColor(GameObject target, float t, float r, float g, float b)
    {
        OffsetColor(target, r, g, b);
        yield return new WaitForSeconds(t);
        RevertColor(target);
    }

    // TOP VERTEX FINDER
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public Vector3 GetTopVertex(GameObject target)
    {
        List<Vector3> vertices = GetVertices(target);

        if(vertices.Count>0)
        {
            Vector3 topMostVertex = target.transform.TransformPoint(vertices[0]);

            foreach(Vector3 vertex in vertices)
            {
                Vector3 worldPoint;

                foreach(MeshFilter mf in GetMeshFilters(target))
                {
                    worldPoint = mf.transform.TransformPoint(vertex);

                    if(worldPoint.y > topMostVertex.y)
                    {
                        topMostVertex = worldPoint;
                    }
                }

                foreach(SkinnedMeshRenderer smr in GetSkinnedMeshRenderers(target))
                {
                    worldPoint = smr.transform.TransformPoint(vertex);

                    if(worldPoint.y > topMostVertex.y)
                    {
                        topMostVertex = worldPoint;
                    }
                }
            }

            return topMostVertex;
        }

        Debug.LogError($"GetTopVertex: Can't find vertices on {target.name}");

        return target.transform.position;
    }
}
