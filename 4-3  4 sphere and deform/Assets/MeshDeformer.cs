using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(MeshFilter))]
public class MeshDeformer : MonoBehaviour {
    Mesh deformingMesh;
    Vector3[] originalVertices, deformedVertices;
    Vector3[] vertexVelocities;
    public float springForce = 20f;
    public float damping = 5f;
    public float uniformScale = 1.0f;

    // Use this for initialization
    void Start () {
        deformingMesh = GetComponent<MeshFilter>().mesh;
        originalVertices = deformingMesh.vertices;
        deformedVertices = new Vector3[originalVertices.Length];
        vertexVelocities = new Vector3[originalVertices.Length];
        for (int i=0;i<originalVertices.Length;++i)
        {
            deformedVertices[i] = originalVertices[i];
        }   
    }
	
	// Update is called once per frame
	void Update () {
        uniformScale = transform.localScale.x;
        for (int i = 0; i < deformedVertices.Length; i++)
        {
            UpdateVertex(i);
        }
        deformingMesh.vertices = deformedVertices;

        deformingMesh.RecalculateNormals();
    }

    void UpdateVertex(int i)
    {
        Vector3 velocity = vertexVelocities[i];
        Vector3 displacement = deformedVertices[i] - originalVertices[i];
        velocity -= displacement * springForce * Time.deltaTime;
        velocity *= 1f - damping * Time.deltaTime;
        vertexVelocities[i] = velocity;
        deformedVertices[i] += velocity * Time.deltaTime;
    }

    public void AddDeformingForce(Vector3 point, float force)
    {
        point = transform.InverseTransformPoint(point);
        Debug.DrawLine(Camera.main.transform.position, point);
        for(int i = 0; i < deformedVertices.Length; ++i)
        {
            AddForceToVertex(i, point, force);
        }
    }

    void AddForceToVertex(int i,Vector3 point, float force)
    {
        Vector3 pointToVertex = (deformedVertices[i] - point)* uniformScale;
        float attenuatedForce = force / (1f + pointToVertex.sqrMagnitude);
        float velocity = attenuatedForce * Time.deltaTime;
        vertexVelocities[i] += pointToVertex.normalized * velocity;
    }
}
