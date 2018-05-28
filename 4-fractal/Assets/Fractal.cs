using UnityEngine;
using System.Collections;

public class Fractal : MonoBehaviour {
    public Mesh cubeMesh;
    public Mesh sphereMesh;
    Mesh []meshes = { null, null };
    public Material material;
    public int depth=0;
    public int maxDepth=20;
    public float childScale = 0.6f;

    Quaternion[] orientations =
    {
        Quaternion.Euler(0f,0f,0f),
        Quaternion.Euler(90f,0f,0f),
        Quaternion.Euler(-90f,0f,0f),
        Quaternion.Euler(0f,0f,90f),
        Quaternion.Euler(0f,0f,-90f)
    };

    Color[] colors =
    {
        new Color(1f,1f,1f),
        new Color(1f,0f,0f)
    };

	// Use this for initialization
	void Start () {
        if(depth == 0)
        {
            //Debug.Log(depth+":"+meshes.Length);
            meshes[0] = cubeMesh;
            meshes[1] = sphereMesh;
        }
        /*The AddComponent method creates a new component 
         * of a certain type, attaches it to the game object,
         * and returns a reference to it. That's why we can 
         * immediately access the component's values. You could 
         * also use an intermediate variable.*/
        gameObject.AddComponent<MeshFilter>().mesh = meshes[depth%meshes.Length];
        gameObject.AddComponent<MeshRenderer>().material = material;
        gameObject.GetComponent<MeshRenderer>().material.color = colors[depth%colors.Length];
        if (depth < maxDepth)
            StartCoroutine(CreateChildren());

    }

    IEnumerator CreateChildren()
    {
        for(int i = 0; i < orientations.Length; ++i)
        {
            new GameObject("Fractal Child").
            AddComponent<Fractal>().Initialize(this, orientations[i]);
            yield return new WaitForSeconds(0.1f);
        }
    }

    void Initialize(Fractal parent, Quaternion orient)
    {
        meshes = parent.meshes;
        
        material = parent.material;
        maxDepth = parent.maxDepth;
        depth = parent.depth + 1;
        this.transform.parent = parent.transform;
        this.transform.localPosition = orient*Vector3.up*(0.5f+0.5f* childScale);
        this.transform.localRotation = orient;
        this.transform.localScale = Vector3.one* childScale;

    }

    // Update is called once per frame
    void Update () {
	
	}
}
