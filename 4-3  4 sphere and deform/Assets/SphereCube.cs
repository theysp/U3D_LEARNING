using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class SphereCube : MonoBehaviour
{
    public int gridSize = 5;
    public float radius = 1.0f;
    private Mesh mesh;
    private Vector3[] vertices;
    private Vector3[] normals;
    public Material[] materials;
    public Color32[] cubeUV;
    //public int roundness=2;

	// Use this for initialization
	void Start () {
        //StartCoroutine(CoGenertate());
        Genertate();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnDrawGizmos()
    {
        return;
        if (vertices == null)
        {
            return;
        }
        Gizmos.color = Color.black;
        for (int i = 0; i < vertices.Length; i++)
        {
            Gizmos.color = Color.black;
            Gizmos.DrawSphere(vertices[i], 0.1f);
            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(vertices[i], normals[i]);
        }
    }

    private void Genertate()
    {
        GetComponent<MeshFilter>().mesh = mesh = new Mesh();
        mesh.name = "Procedural Cube";

        //create vertices
        CreateVertices();
        //create triangles
        CreateTriangles3();
        CreateColliders();
        GetComponent<MeshRenderer>().materials = materials;
    }

    private IEnumerator CoGenertate()
    {
        WaitForSeconds wait = new WaitForSeconds(0.5f);
        GetComponent<MeshFilter>().mesh = mesh = new Mesh();
        mesh.name = "Procedural Cube";
        
        //CreateVertices();
        int edge = (gridSize + gridSize + gridSize + 3) * 4;
        int totvn = ((gridSize + 1) * (gridSize + 1) + (gridSize + 1) * (gridSize + 1) + (gridSize + 1) * (gridSize + 1)) * 2 - edge + 8;
        vertices = new Vector3[totvn];
        

        int vi = 0;

        //create vertices
        CreateVertices();
        //create triangles
        CreateTriangles();

        //around
        for (int i = 0; i <= gridSize; ++i)
        {
            for (int x = 0; x < gridSize; ++x)
            {
                vertices[vi++] = new Vector3(x, i, 0f);
                //yield return wait;
            }

            for (int z = 0; z < gridSize; ++z)
            {
                vertices[vi++] = new Vector3(gridSize, i, z);
                //yield return wait;
            }

            for (int x = gridSize; x > 0; --x)
            {
                vertices[vi++] = new Vector3(x, i, gridSize);
                //yield return wait;
            }

            for (int z = gridSize; z > 0; --z)
            {
                vertices[vi++] = new Vector3(0, i, z);
                //yield return wait;
            }
        }
        //top
        for (int z = 1; z < gridSize; ++z)
        {
            for (int x = 1; x < gridSize; ++x)
            {
                vertices[vi++] = new Vector3(x, gridSize, z);
                //yield return wait;
            }
        }

        //bottom
        for (int z = 1; z < gridSize; ++z)
        {
            for (int x = 1; x < gridSize; ++x)
            {
                vertices[vi++] = new Vector3(x, 0, z);
                //yield return wait;
            }
        }


        //CreateTriangles();
        mesh.vertices = vertices; //the vertices needs to be assigned after initiate
        int quads = (gridSize * gridSize + gridSize * gridSize + gridSize * gridSize) * 2;
        int[] triangles = new int[quads*6];
        //around
        int ti = 0;
        int roundsize = (gridSize + gridSize) * 2;
        for (int y = 0; y < gridSize; ++y)
        {
            for (int i = 0; i < (roundsize - 1); ++i)
                ti = SetQuad(triangles, ti, i + y * roundsize, i + 1 + y * roundsize, i + roundsize + y * roundsize, i + roundsize + 1 + y * roundsize);
            ti = SetQuad(triangles, ti, roundsize - 1 + y * roundsize, y * roundsize, roundsize - 1 + roundsize + y * roundsize, roundsize + y * roundsize);
        }

        mesh.triangles = triangles;
        yield return wait;
        //top
        if(gridSize>0&&gridSize>0){
            int viTopRoundStart = roundsize * gridSize;
            int viTopStart = roundsize * (gridSize + 1);
            //first row
            if (gridSize > 0 && gridSize > 0)
            {
                //first quad
                ti = SetQuad(triangles, ti, viTopRoundStart, viTopRoundStart + 1, viTopRoundStart + roundsize - 1, viTopStart);
                //mid
                for (int i = 0; i < gridSize - 2; ++i)
                {
                    ti = SetQuad(triangles, ti, viTopRoundStart + i + 1, viTopRoundStart + i + 2, viTopStart + i, viTopStart + i + 1);
                }
                //last quad
                ti = SetQuad(triangles, ti, viTopRoundStart + gridSize - 1, viTopRoundStart + gridSize, viTopStart + gridSize - 2, viTopRoundStart + gridSize + 1);
            }
            //middle rows
            for (int zi = 0; zi < (gridSize - 2); ++zi)
            {
                //first quad
                ti = SetQuad(triangles, ti, roundsize - zi - 1 + viTopRoundStart, viTopStart + (gridSize - 1) * zi, roundsize - zi - 2 + viTopRoundStart, viTopStart + (gridSize - 1) * (zi + 1));

                //mid
                for (int xi = 0; xi < (gridSize - 2); ++xi)
                {
                    ti = SetQuad(triangles, ti, viTopStart + (gridSize - 1) * zi + xi, viTopStart + (gridSize - 1) * zi + xi + 1, viTopStart + (gridSize - 1) * (zi + 1) + xi, viTopStart + (gridSize - 1) * (zi + 1) + xi + 1);

                }
                //last quad
                ti = SetQuad(triangles, ti, viTopStart + (gridSize - 1) * zi + gridSize - 2, viTopRoundStart + gridSize + zi + 1, viTopStart + (gridSize - 1) * (zi + 1) + gridSize - 2, viTopRoundStart + gridSize + zi + 2);
 
            }
            //last row
            if (gridSize > 1 && gridSize > 1)
            {
                //first quad
                ti = SetQuad(triangles, ti, viTopRoundStart + roundsize - gridSize + 1, viTopStart + (gridSize - 1) * (gridSize - 2), viTopRoundStart + roundsize - gridSize, viTopRoundStart + roundsize - gridSize - 1);
                //mid
                for (int i = 0; i < gridSize - 2; ++i)
                {
                    ti = SetQuad(triangles, ti, viTopStart + (gridSize - 1) * (gridSize - 2) + i, viTopStart + (gridSize - 1) * (gridSize - 2) + i + 1, viTopRoundStart + roundsize - gridSize - 1 - i, viTopRoundStart + roundsize - gridSize - 2 - i);
                }
                //last quad
                ti = SetQuad(triangles, ti, viTopStart + (gridSize - 1) * (gridSize - 1) - 1, viTopRoundStart + gridSize + gridSize - 1, viTopRoundStart + gridSize + gridSize + 1, viTopRoundStart + gridSize + gridSize);
            }
        }

        //bottom
        if (gridSize > 0 && gridSize > 0 && gridSize > 0)
        {
            int viBottomStart = roundsize * (gridSize + 1) + (gridSize-1)*(gridSize-1);
            //first row according to z coord
            if (gridSize > 0 && gridSize > 0)
            {
                //first
                ti = SetQuad(triangles, ti, 1, 0, viBottomStart, roundsize - 1);
                mesh.triangles = triangles;
                yield return wait;
                //mid
                for(int i=0;i<gridSize-2;++i)
                {
                    ti = SetQuad(triangles, ti, i + 2, i + 1, viBottomStart + i + 1, viBottomStart + i);
                    mesh.triangles = triangles;
                    yield return wait;
                }
                //last
                ti = SetQuad(triangles, ti, gridSize, gridSize - 1, gridSize + 1, viBottomStart + gridSize - 2);
                mesh.triangles = triangles;
                yield return wait;
            }
            //mid rows
            for(int zi = 0;zi<(gridSize-2);++zi)
            {
                //first
                ti = SetQuad(triangles, ti, viBottomStart+zi * (gridSize - 1), roundsize - zi - 1, viBottomStart + (zi + 1) * (gridSize - 1), roundsize - zi - 2);
                mesh.triangles = triangles;
                yield return wait;
                //mid
                for(int xi=0;xi<(gridSize-2);++xi)
                {
                    ti = SetQuad(triangles, ti, viBottomStart + zi * (gridSize - 1) + xi + 1, viBottomStart + zi * (gridSize - 1) + xi, viBottomStart + (zi + 1) * (gridSize - 1) + xi + 1, viBottomStart + (zi + 1) * (gridSize - 1) + xi);
                    mesh.triangles = triangles;
                    yield return wait;
                }
                //last
                ti = SetQuad(triangles, ti, gridSize+zi+1, viBottomStart + (zi + 1) * (gridSize - 1) - 1, gridSize + zi + 2, viBottomStart + (zi + 2) * (gridSize - 1)-1);
                mesh.triangles = triangles;
                yield return wait;
            }
            //last row
            if (gridSize > 1 && gridSize > 1)
            {
                //first
                ti = SetQuad(triangles, ti, viBottomStart + (gridSize-2)*(gridSize-1), roundsize - gridSize+1, roundsize - gridSize - 1, roundsize - gridSize);
                mesh.triangles = triangles;
                yield return wait;
                //mid
                for (int i = 0; i < gridSize - 2; ++i)
                {
                    ti = SetQuad(triangles, ti, viBottomStart + (gridSize - 2) * (gridSize - 1) + i + 1,viBottomStart+(gridSize-2)*(gridSize-1)+i, roundsize - gridSize - 2 - i, roundsize - gridSize - 1 - i);
                    mesh.triangles = triangles;
                    yield return wait;
                }
                //last
                ti = SetQuad(triangles, ti, gridSize+gridSize-1,viBottomStart+ (gridSize - 1) * (gridSize - 1)-1,gridSize+gridSize,gridSize+gridSize+1);
                mesh.triangles = triangles;
                yield return wait;
            }
        }

        mesh.triangles = triangles;
        yield return wait;
    }

    private void SetVertice(int i, float x, float y, float z)
    {
        Vector3 v = new Vector3(x, y, z) * 2f / gridSize - Vector3.one;
        float x2 = v.x * v.x;
        float y2 = v.y * v.y;
        float z2 = v.z * v.z;
        Vector3 s;
        s.x = v.x *Mathf.Sqrt(1-y2/2f-z2/2f + y2 * z2 / 3f);
        s.y = v.y * Mathf.Sqrt(1 - x2 / 2f - z2 / 2f + x2 * z2 / 3f);
        s.z = v.z * Mathf.Sqrt(1 - x2 / 2f - y2 / 2f + x2 * y2 / 3f);

        normals[i] = s;
        vertices[i] = s*radius  ;
        cubeUV[i] = new Color32((byte)x, (byte)y, (byte)z, 0);
    }

    private void CreateVertices()
    {
        int edge = (gridSize + gridSize + gridSize + 3) * 4;
        int totvn = ((gridSize + 1) * (gridSize + 1) + (gridSize + 1) * (gridSize + 1) + (gridSize + 1) * (gridSize + 1)) * 2 - edge + 8;
        vertices = new Vector3[totvn];
        normals = new Vector3[vertices.Length];
        cubeUV = new Color32[vertices.Length];
        int vi = 0;

        //create vertices
        //around
        for (int i = 0; i <= gridSize; ++i)
        {
            for (int x = 0; x < gridSize; ++x)
            {
                SetVertice(vi++, x, i, 0f);
            }

            for (int z = 0; z < gridSize; ++z)
            {
                SetVertice(vi++, gridSize, i, z);
            }

            for (int x = gridSize; x > 0; --x)
            {
                SetVertice(vi++, x, i, gridSize);
            }

            for (int z = gridSize; z > 0; --z)
            {
                SetVertice(vi++, 0, i, z);
            }
        }
        //top
        for (int z = 1; z < gridSize; ++z)
        {
            for (int x = 1; x < gridSize; ++x)
            {
                SetVertice(vi++, x, gridSize, z);
            }
        }

        //bottom
        for (int z = 1; z < gridSize; ++z)
        {
            for (int x = 1; x < gridSize; ++x)
            {
                SetVertice(vi++, x, 0, z);
            }
        }
        mesh.vertices = vertices;
        mesh.normals = normals;
        mesh.colors32 = cubeUV;
    }
    private void CreateTriangles3()
    {
        int[] trianglesX = new int[gridSize * gridSize * 12];
        int[] trianglesZ = new int[gridSize * gridSize * 12];
        int[] trianglesY = new int[gridSize * gridSize * 12];
        //around
        int roundsize = (gridSize + gridSize) * 2;
        int tiX = 0;  //x axis
        int tiZ = 0;  //z axis
        int tiY = 0;  //y axis
        for (int y = 0; y < gridSize; ++y)
        {
            Debug.Log("Y:"+y);
            for (int i = 0; i < gridSize; ++i)
                tiZ = SetQuad(trianglesZ, tiZ, i + y * roundsize, i + 1 + y * roundsize, i + roundsize + y * roundsize, i + roundsize + 1 + y * roundsize);
            for (int i = gridSize; i < gridSize+gridSize; ++i)
                tiX = SetQuad(trianglesX, tiX, i + y * roundsize, i + 1 + y * roundsize, i + roundsize + y * roundsize, i + roundsize + 1 + y * roundsize);
            for (int i = gridSize + gridSize; i < gridSize + gridSize + gridSize; ++i)
                tiZ = SetQuad(trianglesZ, tiZ, i + y * roundsize, i + 1 + y * roundsize, i + roundsize + y * roundsize, i + roundsize + 1 + y * roundsize);
            for (int i = gridSize + gridSize + gridSize; i < (roundsize - 1); ++i)
                tiX = SetQuad(trianglesX, tiX, i + y * roundsize, i + 1 + y * roundsize, i + roundsize + y * roundsize, i + roundsize + 1 + y * roundsize);
            tiX = SetQuad(trianglesX, tiX, roundsize - 1 + y * roundsize, y * roundsize, roundsize - 1 + roundsize + y * roundsize, roundsize + y * roundsize);
        }


        //top
        if (gridSize > 0 && gridSize > 0)
        {
            int viTopRoundStart = roundsize * gridSize;
            int viTopStart = roundsize * (gridSize + 1);
            //first row
            if (gridSize > 0 && gridSize > 0)
            {
                //first quad
                tiY = SetQuad(trianglesY, tiY, viTopRoundStart, viTopRoundStart + 1, viTopRoundStart + roundsize - 1, viTopStart);
                //mid
                for (int i = 0; i < gridSize - 2; ++i)
                {
                    tiY = SetQuad(trianglesY, tiY, viTopRoundStart + i + 1, viTopRoundStart + i + 2, viTopStart + i, viTopStart + i + 1);
                }
                //last quad
                tiY = SetQuad(trianglesY, tiY, viTopRoundStart + gridSize - 1, viTopRoundStart + gridSize, viTopStart + gridSize - 2, viTopRoundStart + gridSize + 1);
            }
            //middle rows
            for (int zi = 0; zi < (gridSize - 2); ++zi)
            {
                //first quad
                tiY = SetQuad(trianglesY, tiY, roundsize - zi - 1 + viTopRoundStart, viTopStart + (gridSize - 1) * zi, roundsize - zi - 2 + viTopRoundStart, viTopStart + (gridSize - 1) * (zi + 1));

                //mid
                for (int xi = 0; xi < (gridSize - 2); ++xi)
                {
                    tiY = SetQuad(trianglesY, tiY, viTopStart + (gridSize - 1) * zi + xi, viTopStart + (gridSize - 1) * zi + xi + 1, viTopStart + (gridSize - 1) * (zi + 1) + xi, viTopStart + (gridSize - 1) * (zi + 1) + xi + 1);

                }
                //last quad
                tiY = SetQuad(trianglesY, tiY, viTopStart + (gridSize - 1) * zi + gridSize - 2, viTopRoundStart + gridSize + zi + 1, viTopStart + (gridSize - 1) * (zi + 1) + gridSize - 2, viTopRoundStart + gridSize + zi + 2);

            }
            //last row
            if (gridSize > 1 && gridSize > 1)
            {
                //first quad
                tiY = SetQuad(trianglesY, tiY, viTopRoundStart + roundsize - gridSize + 1, viTopStart + (gridSize - 1) * (gridSize - 2), viTopRoundStart + roundsize - gridSize, viTopRoundStart + roundsize - gridSize - 1);
                //mid
                for (int i = 0; i < gridSize - 2; ++i)
                {
                    tiY = SetQuad(trianglesY, tiY, viTopStart + (gridSize - 1) * (gridSize - 2) + i, viTopStart + (gridSize - 1) * (gridSize - 2) + i + 1, viTopRoundStart + roundsize - gridSize - 1 - i, viTopRoundStart + roundsize - gridSize - 2 - i);
                }
                //last quad
                tiY = SetQuad(trianglesY, tiY, viTopStart + (gridSize - 1) * (gridSize - 1) - 1, viTopRoundStart + gridSize + gridSize - 1, viTopRoundStart + gridSize + gridSize + 1, viTopRoundStart + gridSize + gridSize);
            }
        }

        //bottom
        if (gridSize > 0 && gridSize > 0 && gridSize > 0)
        {
            int viBottomStart = roundsize * (gridSize + 1) + (gridSize - 1) * (gridSize - 1);
            //first row according to z coord
            if (gridSize > 0 && gridSize > 0)
            {
                //first
                tiY = SetQuad(trianglesY, tiY, 1, 0, viBottomStart, roundsize - 1);
                //mid
                for (int i = 0; i < gridSize - 2; ++i)
                {
                    tiY = SetQuad(trianglesY, tiY, i + 2, i + 1, viBottomStart + i + 1, viBottomStart + i);
                }
                //last
                tiY = SetQuad(trianglesY, tiY, gridSize, gridSize - 1, gridSize + 1, viBottomStart + gridSize - 2);
            }
            //mid rows
            for (int zi = 0; zi < (gridSize - 2); ++zi)
            {
                //first
                tiY = SetQuad(trianglesY, tiY, viBottomStart + zi * (gridSize - 1), roundsize - zi - 1, viBottomStart + (zi + 1) * (gridSize - 1), roundsize - zi - 2);
                //mid
                for (int xi = 0; xi < (gridSize - 2); ++xi)
                {
                    tiY = SetQuad(trianglesY, tiY, viBottomStart + zi * (gridSize - 1) + xi + 1, viBottomStart + zi * (gridSize - 1) + xi, viBottomStart + (zi + 1) * (gridSize - 1) + xi + 1, viBottomStart + (zi + 1) * (gridSize - 1) + xi);
                }
                //last
                tiY = SetQuad(trianglesY, tiY, gridSize + zi + 1, viBottomStart + (zi + 1) * (gridSize - 1) - 1, gridSize + zi + 2, viBottomStart + (zi + 2) * (gridSize - 1) - 1);
            }
            //last row
            if (gridSize > 1 && gridSize > 1)
            {
                //first
                tiY = SetQuad(trianglesY, tiY, viBottomStart + (gridSize - 2) * (gridSize - 1), roundsize - gridSize + 1, roundsize - gridSize - 1, roundsize - gridSize);
                //mid
                for (int i = 0; i < gridSize - 2; ++i)
                {
                    tiY = SetQuad(trianglesY, tiY, viBottomStart + (gridSize - 2) * (gridSize - 1) + i + 1, viBottomStart + (gridSize - 2) * (gridSize - 1) + i, roundsize - gridSize - 2 - i, roundsize - gridSize - 1 - i);
                }
                //last
                tiY = SetQuad(trianglesY, tiY, gridSize + gridSize - 1, viBottomStart + (gridSize - 1) * (gridSize - 1) - 1, gridSize + gridSize, gridSize + gridSize + 1);
            }
        }
        mesh.subMeshCount = 3;
        mesh.SetTriangles(trianglesX, 0);
        mesh.SetTriangles(trianglesZ, 1);
        mesh.SetTriangles(trianglesY, 2);
    }


    private void CreateTriangles()
    {
        int quads = (gridSize * gridSize + gridSize * gridSize + gridSize * gridSize) * 2;
        int[] triangles = new int[quads * 6];

        int[] trianglesX = new int[gridSize * gridSize *12];
        int[] trianglesZ = new int[gridSize * gridSize * 12];
        int[] trianglesY = new int[gridSize * gridSize * 12];
        //around
        int ti = 0;
        int roundsize = (gridSize + gridSize) * 2;
        int tiX = 0;
        int tiZ = 0;
        int tiY = 0;
        for (int y = 0; y < gridSize; ++y)
        {
            for (int i = 0; i < (roundsize - 1); ++i)
                ti = SetQuad(triangles, ti, i + y * roundsize, i + 1 + y * roundsize, i + roundsize + y * roundsize, i + roundsize + 1 + y * roundsize);
            ti = SetQuad(triangles, ti, roundsize - 1 + y * roundsize, y * roundsize, roundsize - 1 + roundsize + y * roundsize, roundsize + y * roundsize);


        }

        
        //top
        if (gridSize > 0 && gridSize > 0)
        {
            int viTopRoundStart = roundsize * gridSize;
            int viTopStart = roundsize * (gridSize + 1);
            //first row
            if (gridSize > 0 && gridSize > 0)
            {
                //first quad
                ti = SetQuad(triangles, ti, viTopRoundStart, viTopRoundStart + 1, viTopRoundStart + roundsize - 1, viTopStart);
                //mid
                for (int i = 0; i < gridSize - 2; ++i)
                {
                    ti = SetQuad(triangles, ti, viTopRoundStart + i + 1, viTopRoundStart + i + 2, viTopStart + i, viTopStart + i + 1);
                }
                //last quad
                ti = SetQuad(triangles, ti, viTopRoundStart + gridSize - 1, viTopRoundStart + gridSize, viTopStart + gridSize - 2, viTopRoundStart + gridSize + 1);
            }
            //middle rows
            for (int zi = 0; zi < (gridSize - 2); ++zi)
            {
                //first quad
                ti = SetQuad(triangles, ti, roundsize - zi - 1 + viTopRoundStart, viTopStart + (gridSize - 1) * zi, roundsize - zi - 2 + viTopRoundStart, viTopStart + (gridSize - 1) * (zi + 1));

                //mid
                for (int xi = 0; xi < (gridSize - 2); ++xi)
                {
                    ti = SetQuad(triangles, ti, viTopStart + (gridSize - 1) * zi + xi, viTopStart + (gridSize - 1) * zi + xi + 1, viTopStart + (gridSize - 1) * (zi + 1) + xi, viTopStart + (gridSize - 1) * (zi + 1) + xi + 1);

                }
                //last quad
                ti = SetQuad(triangles, ti, viTopStart + (gridSize - 1) * zi + gridSize - 2, viTopRoundStart + gridSize + zi + 1, viTopStart + (gridSize - 1) * (zi + 1) + gridSize - 2, viTopRoundStart + gridSize + zi + 2);

            }
            //last row
            if (gridSize > 1 && gridSize > 1)
            {
                //first quad
                ti = SetQuad(triangles, ti, viTopRoundStart + roundsize - gridSize + 1, viTopStart + (gridSize - 1) * (gridSize - 2), viTopRoundStart + roundsize - gridSize, viTopRoundStart + roundsize - gridSize - 1);
                //mid
                for (int i = 0; i < gridSize - 2; ++i)
                {
                    ti = SetQuad(triangles, ti, viTopStart + (gridSize - 1) * (gridSize - 2) + i, viTopStart + (gridSize - 1) * (gridSize - 2) + i + 1, viTopRoundStart + roundsize - gridSize - 1 - i, viTopRoundStart + roundsize - gridSize - 2 - i);
                }
                //last quad
                ti = SetQuad(triangles, ti, viTopStart + (gridSize - 1) * (gridSize - 1) - 1, viTopRoundStart + gridSize + gridSize - 1, viTopRoundStart + gridSize + gridSize + 1, viTopRoundStart + gridSize + gridSize);
            }
        }

        //bottom
        if (gridSize > 0 && gridSize > 0 && gridSize > 0)
        {
            int viBottomStart = roundsize * (gridSize + 1) + (gridSize - 1) * (gridSize - 1);
            //first row according to z coord
            if (gridSize > 0 && gridSize > 0)
            {
                //first
                ti = SetQuad(triangles, ti, 1, 0, viBottomStart, roundsize - 1);
                //mid
                for (int i = 0; i < gridSize - 2; ++i)
                {
                    ti = SetQuad(triangles, ti, i + 2, i + 1, viBottomStart + i + 1, viBottomStart + i);
                }
                //last
                ti = SetQuad(triangles, ti, gridSize, gridSize - 1, gridSize + 1, viBottomStart + gridSize - 2);
            }
            //mid rows
            for (int zi = 0; zi < (gridSize - 2); ++zi)
            {
                //first
                ti = SetQuad(triangles, ti, viBottomStart + zi * (gridSize - 1), roundsize - zi - 1, viBottomStart + (zi + 1) * (gridSize - 1), roundsize - zi - 2);
                //mid
                for (int xi = 0; xi < (gridSize - 2); ++xi)
                {
                    ti = SetQuad(triangles, ti, viBottomStart + zi * (gridSize - 1) + xi + 1, viBottomStart + zi * (gridSize - 1) + xi, viBottomStart + (zi + 1) * (gridSize - 1) + xi + 1, viBottomStart + (zi + 1) * (gridSize - 1) + xi);
                }
                //last
                ti = SetQuad(triangles, ti, gridSize + zi + 1, viBottomStart + (zi + 1) * (gridSize - 1) - 1, gridSize + zi + 2, viBottomStart + (zi + 2) * (gridSize - 1) - 1);
            }
            //last row
            if (gridSize > 1 && gridSize > 1)
            {
                //first
                ti = SetQuad(triangles, ti, viBottomStart + (gridSize - 2) * (gridSize - 1), roundsize - gridSize + 1, roundsize - gridSize - 1, roundsize - gridSize);
                //mid
                for (int i = 0; i < gridSize - 2; ++i)
                {
                    ti = SetQuad(triangles, ti, viBottomStart + (gridSize - 2) * (gridSize - 1) + i + 1, viBottomStart + (gridSize - 2) * (gridSize - 1) + i, roundsize - gridSize - 2 - i, roundsize - gridSize - 1 - i);
                }
                //last
                ti = SetQuad(triangles, ti, gridSize + gridSize - 1, viBottomStart + (gridSize - 1) * (gridSize - 1) - 1, gridSize + gridSize, gridSize + gridSize + 1);
            }
        }
        mesh.triangles = triangles;
    }

    private void CreateColliders()
    {
        gameObject.AddComponent<SphereCollider>();
    }


    private static int SetQuad(int []triangles, int ti, int lb,int rb, int lt, int rt)
    {
        //Debug.Log("SetQuad->ti:" + ti);
        triangles[ti] = lb;
        triangles[ti + 2] = triangles[ti + 3] = rb;
        triangles[ti + 1] = triangles[ti + 4] = lt;
        triangles[ti + 5] = rt;

        return ti + 6;
    }
}
