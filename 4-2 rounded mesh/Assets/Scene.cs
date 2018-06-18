using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene : MonoBehaviour {
    public Cube cubePrefab;
    private Cube[] cubes;
	// Use this for initialization
	void Start () {
        cubePrefab.xSize = 40;
        cubePrefab.ySize = 2;
        cubePrefab.zSize = 40;
        cubePrefab.roundness = 1;
        Cube cube1 = Instantiate(cubePrefab,this.transform);
        cube1.transform.localPosition += new Vector3(-20f,0f, -20f);
        for(int i = 0; i < 10; ++i)
        {
            cubePrefab.xSize = Random.Range(3,6);
            cubePrefab.ySize = Random.Range(3, 6);
            cubePrefab.zSize = Random.Range(3, 6);
            cubePrefab.roundness = Random.Range(1, (int)Mathf.Min(Mathf.Min(cubePrefab.xSize, cubePrefab.ySize), cubePrefab.zSize)/2);
            Cube cube = Instantiate(cubePrefab, this.transform);
            cube.transform.localPosition = Random.onUnitSphere*10 + new Vector3(0f,30f,0f);
            cube.gameObject.AddComponent<Rigidbody>();
        }

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
