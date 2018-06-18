using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class Cube : MonoBehaviour {
    public int xSize=5, ySize=5, zSize=5; 
    private Mesh mesh;
    private Vector3[] vertices;
    private Vector3[] normals;
    public Material[] materials;
    public Color32[] cubeUV;
    public int roundness=2;

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
        int edge = (xSize + ySize + zSize + 3) * 4;
        int totvn = ((xSize + 1) * (ySize + 1) + (ySize + 1) * (zSize + 1) + (xSize + 1) * (zSize + 1)) * 2 - edge + 8;
        vertices = new Vector3[totvn];
        

        int vi = 0;

        //create vertices
        CreateVertices();
        //create triangles
        CreateTriangles();

        //around
        for (int i = 0; i <= ySize; ++i)
        {
            for (int x = 0; x < xSize; ++x)
            {
                vertices[vi++] = new Vector3(x, i, 0f);
                //yield return wait;
            }

            for (int z = 0; z < zSize; ++z)
            {
                vertices[vi++] = new Vector3(xSize, i, z);
                //yield return wait;
            }

            for (int x = xSize; x > 0; --x)
            {
                vertices[vi++] = new Vector3(x, i, zSize);
                //yield return wait;
            }

            for (int z = zSize; z > 0; --z)
            {
                vertices[vi++] = new Vector3(0, i, z);
                //yield return wait;
            }
        }
        //top
        for (int z = 1; z < zSize; ++z)
        {
            for (int x = 1; x < xSize; ++x)
            {
                vertices[vi++] = new Vector3(x, ySize, z);
                //yield return wait;
            }
        }

        //bottom
        for (int z = 1; z < zSize; ++z)
        {
            for (int x = 1; x < xSize; ++x)
            {
                vertices[vi++] = new Vector3(x, 0, z);
                //yield return wait;
            }
        }


        //CreateTriangles();
        mesh.vertices = vertices; //the vertices needs to be assigned after initiate
        int quads = (xSize * ySize + xSize * zSize + ySize * zSize) * 2;
        int[] triangles = new int[quads*6];
        //around
        int ti = 0;
        int roundsize = (xSize + zSize) * 2;
        for (int y = 0; y < ySize; ++y)
        {
            for (int i = 0; i < (roundsize - 1); ++i)
                ti = SetQuad(triangles, ti, i + y * roundsize, i + 1 + y * roundsize, i + roundsize + y * roundsize, i + roundsize + 1 + y * roundsize);
            ti = SetQuad(triangles, ti, roundsize - 1 + y * roundsize, y * roundsize, roundsize - 1 + roundsize + y * roundsize, roundsize + y * roundsize);
        }

        mesh.triangles = triangles;
        yield return wait;
        //top
        if(xSize>0&&zSize>0){
            int viTopRoundStart = roundsize * ySize;
            int viTopStart = roundsize * (ySize + 1);
            //first row
            if (xSize > 0 && ySize > 0)
            {
                //first quad
                ti = SetQuad(triangles, ti, viTopRoundStart, viTopRoundStart + 1, viTopRoundStart + roundsize - 1, viTopStart);
                //mid
                for (int i = 0; i < xSize - 2; ++i)
                {
                    ti = SetQuad(triangles, ti, viTopRoundStart + i + 1, viTopRoundStart + i + 2, viTopStart + i, viTopStart + i + 1);
                }
                //last quad
                ti = SetQuad(triangles, ti, viTopRoundStart + xSize - 1, viTopRoundStart + xSize, viTopStart + xSize - 2, viTopRoundStart + xSize + 1);
            }
            //middle rows
            for (int zi = 0; zi < (zSize - 2); ++zi)
            {
                //first quad
                ti = SetQuad(triangles, ti, roundsize - zi - 1 + viTopRoundStart, viTopStart + (xSize - 1) * zi, roundsize - zi - 2 + viTopRoundStart, viTopStart + (xSize - 1) * (zi + 1));

                //mid
                for (int xi = 0; xi < (xSize - 2); ++xi)
                {
                    ti = SetQuad(triangles, ti, viTopStart + (xSize - 1) * zi + xi, viTopStart + (xSize - 1) * zi + xi + 1, viTopStart + (xSize - 1) * (zi + 1) + xi, viTopStart + (xSize - 1) * (zi + 1) + xi + 1);

                }
                //last quad
                ti = SetQuad(triangles, ti, viTopStart + (xSize - 1) * zi + xSize - 2, viTopRoundStart + xSize + zi + 1, viTopStart + (xSize - 1) * (zi + 1) + xSize - 2, viTopRoundStart + xSize + zi + 2);
 
            }
            //last row
            if (xSize > 1 && ySize > 1)
            {
                //first quad
                ti = SetQuad(triangles, ti, viTopRoundStart + roundsize - zSize + 1, viTopStart + (xSize - 1) * (ySize - 2), viTopRoundStart + roundsize - zSize, viTopRoundStart + roundsize - zSize - 1);
                //mid
                for (int i = 0; i < xSize - 2; ++i)
                {
                    ti = SetQuad(triangles, ti, viTopStart + (xSize - 1) * (ySize - 2) + i, viTopStart + (xSize - 1) * (ySize - 2) + i + 1, viTopRoundStart + roundsize - zSize - 1 - i, viTopRoundStart + roundsize - zSize - 2 - i);
                }
                //last quad
                ti = SetQuad(triangles, ti, viTopStart + (xSize - 1) * (ySize - 1) - 1, viTopRoundStart + xSize + zSize - 1, viTopRoundStart + xSize + zSize + 1, viTopRoundStart + xSize + zSize);
            }
        }

        //bottom
        if (xSize > 0 && zSize > 0 && ySize > 0)
        {
            int viBottomStart = roundsize * (ySize + 1) + (xSize-1)*(zSize-1);
            //first row according to z coord
            if (xSize > 0 && zSize > 0)
            {
                //first
                ti = SetQuad(triangles, ti, 1, 0, viBottomStart, roundsize - 1);
                mesh.triangles = triangles;
                yield return wait;
                //mid
                for(int i=0;i<xSize-2;++i)
                {
                    ti = SetQuad(triangles, ti, i + 2, i + 1, viBottomStart + i + 1, viBottomStart + i);
                    mesh.triangles = triangles;
                    yield return wait;
                }
                //last
                ti = SetQuad(triangles, ti, xSize, xSize - 1, xSize + 1, viBottomStart + xSize - 2);
                mesh.triangles = triangles;
                yield return wait;
            }
            //mid rows
            for(int zi = 0;zi<(zSize-2);++zi)
            {
                //first
                ti = SetQuad(triangles, ti, viBottomStart+zi * (xSize - 1), roundsize - zi - 1, viBottomStart + (zi + 1) * (xSize - 1), roundsize - zi - 2);
                mesh.triangles = triangles;
                yield return wait;
                //mid
                for(int xi=0;xi<(xSize-2);++xi)
                {
                    ti = SetQuad(triangles, ti, viBottomStart + zi * (xSize - 1) + xi + 1, viBottomStart + zi * (xSize - 1) + xi, viBottomStart + (zi + 1) * (xSize - 1) + xi + 1, viBottomStart + (zi + 1) * (xSize - 1) + xi);
                    mesh.triangles = triangles;
                    yield return wait;
                }
                //last
                ti = SetQuad(triangles, ti, xSize+zi+1, viBottomStart + (zi + 1) * (xSize - 1) - 1, xSize + zi + 2, viBottomStart + (zi + 2) * (xSize - 1)-1);
                mesh.triangles = triangles;
                yield return wait;
            }
            //last row
            if (xSize > 1 && zSize > 1)
            {
                //first
                ti = SetQuad(triangles, ti, viBottomStart + (zSize-2)*(xSize-1), roundsize - xSize+1, roundsize - xSize - 1, roundsize - xSize);
                mesh.triangles = triangles;
                yield return wait;
                //mid
                for (int i = 0; i < xSize - 2; ++i)
                {
                    ti = SetQuad(triangles, ti, viBottomStart + (zSize - 2) * (xSize - 1) + i + 1,viBottomStart+(zSize-2)*(xSize-1)+i, roundsize - xSize - 2 - i, roundsize - xSize - 1 - i);
                    mesh.triangles = triangles;
                    yield return wait;
                }
                //last
                ti = SetQuad(triangles, ti, xSize+ySize-1,viBottomStart+ (zSize - 1) * (xSize - 1)-1,xSize+ySize,xSize+ySize+1);
                mesh.triangles = triangles;
                yield return wait;
            }
        }

        mesh.triangles = triangles;
        yield return wait;
    }

    private void SetVertice(int i, float x, float y, float z)
    {
        Vector3 inner = new Vector3(x, y, z);
        if (x < roundness)
        {
            inner.x = roundness;
        }
        else if (x > xSize - roundness)
        {
            inner.x = xSize - roundness;
        }

        if(y<roundness)
        {
            inner.y = roundness;
        }else if (y > ySize - roundness)
        {
            inner.y = ySize - roundness;
        }

        if(z<roundness)
        {
            inner.z = roundness;
        }else if(z>zSize-roundness)
        {
            inner.z = zSize - roundness;
        }

        vertices[i] = new Vector3(x, y, z);

        normals[i] = (vertices[i] - inner).normalized;
		vertices[i] = inner + normals[i] * roundness;
        cubeUV[i] = new Color32((byte)x, (byte)y, (byte)z,0);
    }

    private void CreateVertices()
    {
        int edge = (xSize + ySize + zSize + 3) * 4;
        int totvn = ((xSize + 1) * (ySize + 1) + (ySize + 1) * (zSize + 1) + (xSize + 1) * (zSize + 1)) * 2 - edge + 8;
        vertices = new Vector3[totvn];
        normals = new Vector3[vertices.Length];
        cubeUV = new Color32[vertices.Length];
        int vi = 0;

        //create vertices
        //around
        for (int i = 0; i <= ySize; ++i)
        {
            for (int x = 0; x < xSize; ++x)
            {
                SetVertice(vi++, x, i, 0f);
            }

            for (int z = 0; z < zSize; ++z)
            {
                SetVertice(vi++, xSize, i, z);
            }

            for (int x = xSize; x > 0; --x)
            {
                SetVertice(vi++, x, i, zSize);
            }

            for (int z = zSize; z > 0; --z)
            {
                SetVertice(vi++, 0, i, z);
            }
        }
        //top
        for (int z = 1; z < zSize; ++z)
        {
            for (int x = 1; x < xSize; ++x)
            {
                SetVertice(vi++, x, ySize, z);
            }
        }

        //bottom
        for (int z = 1; z < zSize; ++z)
        {
            for (int x = 1; x < xSize; ++x)
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
        int[] trianglesX = new int[zSize * ySize * 12];
        int[] trianglesZ = new int[xSize * ySize * 12];
        int[] trianglesY = new int[xSize * zSize * 12];
        //around
        int roundsize = (xSize + zSize) * 2;
        int tiX = 0;  //x axis
        int tiZ = 0;  //z axis
        int tiY = 0;  //y axis
        for (int y = 0; y < ySize; ++y)
        {
            Debug.Log("Y:"+y);
            for (int i = 0; i < xSize; ++i)
                tiZ = SetQuad(trianglesZ, tiZ, i + y * roundsize, i + 1 + y * roundsize, i + roundsize + y * roundsize, i + roundsize + 1 + y * roundsize);
            for (int i = xSize; i < xSize+zSize; ++i)
                tiX = SetQuad(trianglesX, tiX, i + y * roundsize, i + 1 + y * roundsize, i + roundsize + y * roundsize, i + roundsize + 1 + y * roundsize);
            for (int i = xSize + zSize; i < xSize + zSize + xSize; ++i)
                tiZ = SetQuad(trianglesZ, tiZ, i + y * roundsize, i + 1 + y * roundsize, i + roundsize + y * roundsize, i + roundsize + 1 + y * roundsize);
            for (int i = xSize + zSize + xSize; i < (roundsize - 1); ++i)
                tiX = SetQuad(trianglesX, tiX, i + y * roundsize, i + 1 + y * roundsize, i + roundsize + y * roundsize, i + roundsize + 1 + y * roundsize);
            tiX = SetQuad(trianglesX, tiX, roundsize - 1 + y * roundsize, y * roundsize, roundsize - 1 + roundsize + y * roundsize, roundsize + y * roundsize);
        }


        //top
        if (xSize > 0 && zSize > 0)
        {
            int viTopRoundStart = roundsize * ySize;
            int viTopStart = roundsize * (ySize + 1);
            //first row
            if (xSize > 0 && ySize > 0)
            {
                //first quad
                tiY = SetQuad(trianglesY, tiY, viTopRoundStart, viTopRoundStart + 1, viTopRoundStart + roundsize - 1, viTopStart);
                //mid
                for (int i = 0; i < xSize - 2; ++i)
                {
                    tiY = SetQuad(trianglesY, tiY, viTopRoundStart + i + 1, viTopRoundStart + i + 2, viTopStart + i, viTopStart + i + 1);
                }
                //last quad
                tiY = SetQuad(trianglesY, tiY, viTopRoundStart + xSize - 1, viTopRoundStart + xSize, viTopStart + xSize - 2, viTopRoundStart + xSize + 1);
            }
            //middle rows
            for (int zi = 0; zi < (zSize - 2); ++zi)
            {
                //first quad
                tiY = SetQuad(trianglesY, tiY, roundsize - zi - 1 + viTopRoundStart, viTopStart + (xSize - 1) * zi, roundsize - zi - 2 + viTopRoundStart, viTopStart + (xSize - 1) * (zi + 1));

                //mid
                for (int xi = 0; xi < (xSize - 2); ++xi)
                {
                    tiY = SetQuad(trianglesY, tiY, viTopStart + (xSize - 1) * zi + xi, viTopStart + (xSize - 1) * zi + xi + 1, viTopStart + (xSize - 1) * (zi + 1) + xi, viTopStart + (xSize - 1) * (zi + 1) + xi + 1);

                }
                //last quad
                tiY = SetQuad(trianglesY, tiY, viTopStart + (xSize - 1) * zi + xSize - 2, viTopRoundStart + xSize + zi + 1, viTopStart + (xSize - 1) * (zi + 1) + xSize - 2, viTopRoundStart + xSize + zi + 2);

            }
            //last row
            if (xSize > 1 && ySize > 1)
            {
                //first quad
                tiY = SetQuad(trianglesY, tiY, viTopRoundStart + roundsize - zSize + 1, viTopStart + (xSize - 1) * (zSize - 2), viTopRoundStart + roundsize - zSize, viTopRoundStart + roundsize - zSize - 1);
                //mid
                for (int i = 0; i < xSize - 2; ++i)
                {
                    tiY = SetQuad(trianglesY, tiY, viTopStart + (xSize - 1) * (zSize - 2) + i, viTopStart + (xSize - 1) * (zSize - 2) + i + 1, viTopRoundStart + roundsize - zSize - 1 - i, viTopRoundStart + roundsize - zSize - 2 - i);
                }
                //last quad
                tiY = SetQuad(trianglesY, tiY, viTopStart + (xSize - 1) * (zSize - 1) - 1, viTopRoundStart + xSize + zSize - 1, viTopRoundStart + xSize + zSize + 1, viTopRoundStart + xSize + zSize);
            }
        }

        //bottom
        if (xSize > 0 && zSize > 0 && ySize > 0)
        {
            int viBottomStart = roundsize * (ySize + 1) + (xSize - 1) * (zSize - 1);
            //first row according to z coord
            if (xSize > 0 && zSize > 0)
            {
                //first
                tiY = SetQuad(trianglesY, tiY, 1, 0, viBottomStart, roundsize - 1);
                //mid
                for (int i = 0; i < xSize - 2; ++i)
                {
                    tiY = SetQuad(trianglesY, tiY, i + 2, i + 1, viBottomStart + i + 1, viBottomStart + i);
                }
                //last
                tiY = SetQuad(trianglesY, tiY, xSize, xSize - 1, xSize + 1, viBottomStart + xSize - 2);
            }
            //mid rows
            for (int zi = 0; zi < (zSize - 2); ++zi)
            {
                //first
                tiY = SetQuad(trianglesY, tiY, viBottomStart + zi * (xSize - 1), roundsize - zi - 1, viBottomStart + (zi + 1) * (xSize - 1), roundsize - zi - 2);
                //mid
                for (int xi = 0; xi < (xSize - 2); ++xi)
                {
                    tiY = SetQuad(trianglesY, tiY, viBottomStart + zi * (xSize - 1) + xi + 1, viBottomStart + zi * (xSize - 1) + xi, viBottomStart + (zi + 1) * (xSize - 1) + xi + 1, viBottomStart + (zi + 1) * (xSize - 1) + xi);
                }
                //last
                tiY = SetQuad(trianglesY, tiY, xSize + zi + 1, viBottomStart + (zi + 1) * (xSize - 1) - 1, xSize + zi + 2, viBottomStart + (zi + 2) * (xSize - 1) - 1);
            }
            //last row
            if (xSize > 1 && zSize > 1)
            {
                //first
                tiY = SetQuad(trianglesY, tiY, viBottomStart + (zSize - 2) * (xSize - 1), roundsize - xSize + 1, roundsize - xSize - 1, roundsize - xSize);
                //mid
                for (int i = 0; i < xSize - 2; ++i)
                {
                    tiY = SetQuad(trianglesY, tiY, viBottomStart + (zSize - 2) * (xSize - 1) + i + 1, viBottomStart + (zSize - 2) * (xSize - 1) + i, roundsize - xSize - 2 - i, roundsize - xSize - 1 - i);
                }
                //last
                tiY = SetQuad(trianglesY, tiY, xSize + ySize - 1, viBottomStart + (zSize - 1) * (xSize - 1) - 1, xSize + ySize, xSize + ySize + 1);
            }
        }
        mesh.subMeshCount = 3;
        mesh.SetTriangles(trianglesX, 0);
        mesh.SetTriangles(trianglesZ, 1);
        mesh.SetTriangles(trianglesY, 2);
    }


    private void CreateTriangles()
    {
        int quads = (xSize * ySize + xSize * zSize + ySize * zSize) * 2;
        int[] triangles = new int[quads * 6];

        int[] trianglesX = new int[xSize * ySize *12];
        int[] trianglesZ = new int[xSize * zSize * 12];
        int[] trianglesY = new int[zSize * ySize * 12];
        //around
        int ti = 0;
        int roundsize = (xSize + zSize) * 2;
        int tiX = 0;
        int tiZ = 0;
        int tiY = 0;
        for (int y = 0; y < ySize; ++y)
        {
            for (int i = 0; i < (roundsize - 1); ++i)
                ti = SetQuad(triangles, ti, i + y * roundsize, i + 1 + y * roundsize, i + roundsize + y * roundsize, i + roundsize + 1 + y * roundsize);
            ti = SetQuad(triangles, ti, roundsize - 1 + y * roundsize, y * roundsize, roundsize - 1 + roundsize + y * roundsize, roundsize + y * roundsize);


        }

        
        //top
        if (xSize > 0 && zSize > 0)
        {
            int viTopRoundStart = roundsize * ySize;
            int viTopStart = roundsize * (ySize + 1);
            //first row
            if (xSize > 0 && ySize > 0)
            {
                //first quad
                ti = SetQuad(triangles, ti, viTopRoundStart, viTopRoundStart + 1, viTopRoundStart + roundsize - 1, viTopStart);
                //mid
                for (int i = 0; i < xSize - 2; ++i)
                {
                    ti = SetQuad(triangles, ti, viTopRoundStart + i + 1, viTopRoundStart + i + 2, viTopStart + i, viTopStart + i + 1);
                }
                //last quad
                ti = SetQuad(triangles, ti, viTopRoundStart + xSize - 1, viTopRoundStart + xSize, viTopStart + xSize - 2, viTopRoundStart + xSize + 1);
            }
            //middle rows
            for (int zi = 0; zi < (zSize - 2); ++zi)
            {
                //first quad
                ti = SetQuad(triangles, ti, roundsize - zi - 1 + viTopRoundStart, viTopStart + (xSize - 1) * zi, roundsize - zi - 2 + viTopRoundStart, viTopStart + (xSize - 1) * (zi + 1));

                //mid
                for (int xi = 0; xi < (xSize - 2); ++xi)
                {
                    ti = SetQuad(triangles, ti, viTopStart + (xSize - 1) * zi + xi, viTopStart + (xSize - 1) * zi + xi + 1, viTopStart + (xSize - 1) * (zi + 1) + xi, viTopStart + (xSize - 1) * (zi + 1) + xi + 1);

                }
                //last quad
                ti = SetQuad(triangles, ti, viTopStart + (xSize - 1) * zi + xSize - 2, viTopRoundStart + xSize + zi + 1, viTopStart + (xSize - 1) * (zi + 1) + xSize - 2, viTopRoundStart + xSize + zi + 2);

            }
            //last row
            if (xSize > 1 && ySize > 1)
            {
                //first quad
                ti = SetQuad(triangles, ti, viTopRoundStart + roundsize - zSize + 1, viTopStart + (xSize - 1) * (ySize - 2), viTopRoundStart + roundsize - zSize, viTopRoundStart + roundsize - zSize - 1);
                //mid
                for (int i = 0; i < xSize - 2; ++i)
                {
                    ti = SetQuad(triangles, ti, viTopStart + (xSize - 1) * (ySize - 2) + i, viTopStart + (xSize - 1) * (ySize - 2) + i + 1, viTopRoundStart + roundsize - zSize - 1 - i, viTopRoundStart + roundsize - zSize - 2 - i);
                }
                //last quad
                ti = SetQuad(triangles, ti, viTopStart + (xSize - 1) * (ySize - 1) - 1, viTopRoundStart + xSize + zSize - 1, viTopRoundStart + xSize + zSize + 1, viTopRoundStart + xSize + zSize);
            }
        }

        //bottom
        if (xSize > 0 && zSize > 0 && ySize > 0)
        {
            int viBottomStart = roundsize * (ySize + 1) + (xSize - 1) * (zSize - 1);
            //first row according to z coord
            if (xSize > 0 && zSize > 0)
            {
                //first
                ti = SetQuad(triangles, ti, 1, 0, viBottomStart, roundsize - 1);
                //mid
                for (int i = 0; i < xSize - 2; ++i)
                {
                    ti = SetQuad(triangles, ti, i + 2, i + 1, viBottomStart + i + 1, viBottomStart + i);
                }
                //last
                ti = SetQuad(triangles, ti, xSize, xSize - 1, xSize + 1, viBottomStart + xSize - 2);
            }
            //mid rows
            for (int zi = 0; zi < (zSize - 2); ++zi)
            {
                //first
                ti = SetQuad(triangles, ti, viBottomStart + zi * (xSize - 1), roundsize - zi - 1, viBottomStart + (zi + 1) * (xSize - 1), roundsize - zi - 2);
                //mid
                for (int xi = 0; xi < (xSize - 2); ++xi)
                {
                    ti = SetQuad(triangles, ti, viBottomStart + zi * (xSize - 1) + xi + 1, viBottomStart + zi * (xSize - 1) + xi, viBottomStart + (zi + 1) * (xSize - 1) + xi + 1, viBottomStart + (zi + 1) * (xSize - 1) + xi);
                }
                //last
                ti = SetQuad(triangles, ti, xSize + zi + 1, viBottomStart + (zi + 1) * (xSize - 1) - 1, xSize + zi + 2, viBottomStart + (zi + 2) * (xSize - 1) - 1);
            }
            //last row
            if (xSize > 1 && zSize > 1)
            {
                //first
                ti = SetQuad(triangles, ti, viBottomStart + (zSize - 2) * (xSize - 1), roundsize - xSize + 1, roundsize - xSize - 1, roundsize - xSize);
                //mid
                for (int i = 0; i < xSize - 2; ++i)
                {
                    ti = SetQuad(triangles, ti, viBottomStart + (zSize - 2) * (xSize - 1) + i + 1, viBottomStart + (zSize - 2) * (xSize - 1) + i, roundsize - xSize - 2 - i, roundsize - xSize - 1 - i);
                }
                //last
                ti = SetQuad(triangles, ti, xSize + ySize - 1, viBottomStart + (zSize - 1) * (xSize - 1) - 1, xSize + ySize, xSize + ySize + 1);
            }
        }
        mesh.triangles = triangles;
    }

    private void CreateColliders()
    {
        AddBoxCollider(xSize, ySize - roundness * 2, zSize - roundness * 2);
        AddBoxCollider(xSize - roundness * 2, ySize, zSize - roundness * 2);
        AddBoxCollider(xSize - roundness * 2, ySize - roundness * 2, zSize);

        Vector3 min = Vector3.one * roundness;
        Vector3 half = new Vector3(xSize, ySize, zSize) * 0.5f;
        Vector3 max = new Vector3(xSize, ySize, zSize) - min;

        AddCapsuleCollider(0, half.x, min.y, min.z);
        AddCapsuleCollider(0, half.x, min.y, max.z);
        AddCapsuleCollider(0, half.x, max.y, min.z);
        AddCapsuleCollider(0, half.x, max.y, max.z);

        AddCapsuleCollider(1, min.x, half.y, min.z);
        AddCapsuleCollider(1, min.x, half.y, max.z);
        AddCapsuleCollider(1, max.x, half.y, min.z);
        AddCapsuleCollider(1, max.x, half.y, max.z);

        AddCapsuleCollider(2, min.x, min.y, half.z);
        AddCapsuleCollider(2, min.x, max.y, half.z);
        AddCapsuleCollider(2, max.x, min.y, half.z);
        AddCapsuleCollider(2, max.x, max.y, half.z);
    }

    private void AddBoxCollider(float x, float y, float z)
    {
        BoxCollider c = gameObject.AddComponent<BoxCollider>();
        c.size = new Vector3(x, y, z);
    }

    private void AddCapsuleCollider(int direction, float x, float y, float z)
    {
        CapsuleCollider c = gameObject.AddComponent<CapsuleCollider>();
        c.center = new Vector3(x, y, z);
        c.direction = direction;
        c.radius = roundness;
        c.height = c.center[direction] * 2f;
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
