using UnityEngine;
using System.Collections;

public class StuffSpawnRing : MonoBehaviour {
    public StuffSpawner []spawners;
    public int matNum=4;
    public Material mat0;
    public Material mat1;
    public Material mat2;
    public Material mat3;
    public Material mat4;

    Material []mats;
    private void Awake()
    {
        mats = new Material[matNum];
        mats[0] = mat0;
        mats[1] = mat1;
        mats[2] = mat2;
        mats[3] = mat3;
        spawners = new StuffSpawner[9];
        for (int i = 0; i < 9; ++i)
        {
            Transform trans = spawners[i].GetComponent<Transform>();
        }
    }
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    
	}
}
