using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class CombineMesh : MonoBehaviour
{
    void Start()
    {
        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();

        List<MeshFilter> meshfilterr = new List<MeshFilter>();
        for (int j = 1; j < meshFilters.Length; j++)
            meshfilterr.Add(meshFilters[j]);

        CombineInstance[] combine = new CombineInstance[meshFilters.Length];

        int i = 0;
        while (i < meshfilterr.Count)
        {
            if (meshfilterr[i].sharedMesh != null)
            {
                combine[i].mesh = meshfilterr[i].sharedMesh;
                combine[i].transform = meshfilterr[i].transform.localToWorldMatrix;
                // meshFilters[i].gameObject.SetActive(false);

                i++;
            }
        }
        var meshFilter = transform.GetComponent<MeshFilter>();
        meshFilter.mesh = new Mesh();
        meshFilter.mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt16;
        meshFilter.mesh.CombineMeshes(combine);
        // meshFilter.mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        GetComponent<MeshCollider>().sharedMesh = meshFilter.mesh;
        transform.gameObject.SetActive(true);

        // transform.GetComponent<MeshFilter>().mesh = new Mesh();
        // transform.GetComponent<MeshFilter>().mesh.CombineMeshes(combine);
        // transform.gameObject.SetActive(true);

        transform.localScale = new Vector3(1f, 1f, 1f);
        transform.rotation = Quaternion.identity;
        transform.position = Vector3.zero;
    }
}
