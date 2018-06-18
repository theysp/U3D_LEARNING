using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class Grid : MonoBehaviour {
    public int xSize, ySize;
    private Vector3[] vertices;
    
    private Mesh mesh;

    private void Awake()
    {
        StartCoroutine(Generate());
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    private IEnumerator Generate()
    {
        WaitForSeconds wait = new WaitForSeconds(0.5f);
        vertices = new Vector3[(xSize+1)*(ySize+1)];
        Vector2[] uvs = new Vector2[vertices.Length];
        Vector4[] tangents = new Vector4[vertices.Length];
        Vector4 tangent = new Vector4(1f, 0f, 0f, -1f);
        for(int i = 0, y = 0; y <= ySize; ++y)
        {
            for (int x = 0; x <= xSize; x++, ++i)
            {
                vertices[i] = new Vector3(x, y);
                uvs[i] = new Vector2(x*1f/xSize,y*1f/ySize);
                tangents[i] = tangent;
               // Debug.Log("creating " + vertices[i]);

            }
        }

        GetComponent<MeshFilter>().mesh = mesh = new Mesh();
        mesh.vertices = vertices;

        int[] triangles = new int[xSize*ySize*2*3];
        for(int ti=0, vi=0,y=0;y<ySize;++y,vi++)
        {
            for(int x = 0; x < xSize; x++,ti+=6, vi++)
            {
                triangles[ti] = vi;
                triangles[ti + 2] = triangles[ti + 3] = vi + 1;
                triangles[ti + 1] = triangles[ti + 4] = vi + xSize+1;
                triangles[ti + 5] = vi + xSize + 2;
            }
        }
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        mesh.uv = uvs;
        mesh.tangents = tangents;
        yield return wait;
    }
    private void OnDrawGizmos()
    {
        if (vertices == null)
            return;
        Gizmos.color = Color.black;
        for (int i = 0; i < vertices.Length; ++i)
        {
            Gizmos.DrawSphere(vertices[i], 0.1f);
            Debug.Log(vertices[i]);
        }
    }
}
