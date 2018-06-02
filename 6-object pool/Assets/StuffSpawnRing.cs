using UnityEngine;
using System.Collections;

public class StuffSpawnRing : MonoBehaviour {
    public StuffSpawner spawnerPrefab;
    public float radius = 10.0f;
    public float tiltAngle = 45f;
    public int numSpawns = 9;
    public int matNum;
    public Material mat0;
    public Material mat1;
    public Material mat2;
    public Material mat3;
    public Material mat4;

    Material [] mats;
    private void Awake()
    {
        mats = new Material[4];
        mats[0] = mat0;
        mats[1] = mat1;
        mats[2] = mat2;
        mats[3] = mat3;
        for (int i = 0; i < numSpawns; ++i)
        {
            Transform rotater = new GameObject("rotater").transform;
            rotater.SetParent(transform);
            rotater.localRotation = Quaternion.Euler(0f, i * 360 / numSpawns, 0f);

            StuffSpawner spawner = Instantiate<StuffSpawner>(spawnerPrefab);
            spawner.transform.SetParent(rotater, false);
            spawner.transform.localPosition = new Vector3(0f, 0f, radius);
            spawner.transform.localRotation = Quaternion.Euler(-30f, 0f, 0f);
            spawner.material = mats[i % mats.Length];
        }
    }
    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {
	    
	}
}
