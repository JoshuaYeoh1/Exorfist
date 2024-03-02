using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopVertexFinder : MonoBehaviour
{
    public GameObject targetObject;

    public List<Vector3> vertices = new List<Vector3>();
    public Vector3 topMostVertex;

    SkinnedMeshRenderer[] smrs, childrenSmrs;
    MeshFilter[] mfs, childrenMfs;

    public Vector3 GetTopVertex(GameObject _targetObject)
    {
        targetObject = _targetObject;

        if(vertices.Count>0) vertices.Clear();

        List<Mesh> meshes = new List<Mesh>();

        smrs = targetObject.GetComponents<SkinnedMeshRenderer>();
        
        if(smrs.Length>0)
        {
            foreach(SkinnedMeshRenderer smr in smrs)
            {
                meshes.Add(smr.sharedMesh);
            }
        }

        childrenSmrs = targetObject.GetComponentsInChildren<SkinnedMeshRenderer>();

        if(childrenSmrs.Length>0)
        {
            foreach(SkinnedMeshRenderer childSmr in childrenSmrs)
            {
                meshes.Add(childSmr.sharedMesh);
            }
        }

        mfs = targetObject.GetComponents<MeshFilter>();

        if(mfs.Length>0)
        {
            foreach(MeshFilter mf in mfs)
            {
                meshes.Add(mf.sharedMesh);
            }
        }

        childrenMfs = targetObject.GetComponentsInChildren<MeshFilter>();

        if(childrenMfs.Length>0)
        {
            foreach(MeshFilter childMf in childrenMfs)
            {
                meshes.Add(childMf.sharedMesh);
            }
        }

        if(meshes.Count>0)
        {
            foreach(Mesh mesh in meshes)
            {
                foreach(Vector3 vertex in mesh.vertices)
                {
                    vertices.Add(vertex);
                }
            }
        }
        
        if(vertices.Count>0)
        {
            topMostVertex = _targetObject.transform.TransformPoint(vertices[0]);

            foreach(Vector3 vertex in vertices)
            {
                Vector3 worldPoint;

                foreach(MeshFilter mf in mfs)
                {
                    worldPoint = mf.transform.TransformPoint(vertex);

                    if(worldPoint.y > topMostVertex.y)
                    {
                        topMostVertex = worldPoint;
                    }
                }

                foreach(SkinnedMeshRenderer smr in smrs)
                {
                    worldPoint = smr.transform.TransformPoint(vertex);

                    if(worldPoint.y > topMostVertex.y)
                    {
                        topMostVertex = worldPoint;
                    }
                }
                
                foreach(MeshFilter childMf in childrenMfs)
                {
                    worldPoint = childMf.transform.TransformPoint(vertex);

                    if(worldPoint.y > topMostVertex.y)
                    {
                        topMostVertex = worldPoint;
                    }
                }

                foreach(SkinnedMeshRenderer childSmr in childrenSmrs)
                {
                    worldPoint = childSmr.transform.TransformPoint(vertex);

                    if(worldPoint.y > topMostVertex.y)
                    {
                        topMostVertex = worldPoint;
                    }
                }
            }
        }

        return topMostVertex - _targetObject.transform.position;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(topMostVertex, .05f);

        if(vertices.Count>0)
        {
            foreach(Vector3 vertex in vertices)
            {
                Gizmos.color = Color.green;

                foreach(MeshFilter mf in mfs)
                {
                    Gizmos.DrawSphere(mf.transform.TransformPoint(vertex), 0.02f);
                }

                foreach(SkinnedMeshRenderer smr in smrs)
                {
                    Gizmos.DrawSphere(smr.transform.TransformPoint(vertex), 0.02f);
                }
                
                foreach(MeshFilter childMf in childrenMfs)
                {
                    Gizmos.DrawSphere(childMf.transform.TransformPoint(vertex), 0.02f);
                }

                foreach(SkinnedMeshRenderer childSmr in childrenSmrs)
                {
                    Gizmos.DrawSphere(childSmr.transform.TransformPoint(vertex), 0.02f);
                }
            }
        }
    }
}