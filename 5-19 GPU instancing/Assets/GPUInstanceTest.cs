using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class GPUInstanceTest : MonoBehaviour{
    public Transform prefab;
    public int instanceNum = 50000;
    public float radius = 50f;

	// Use this for initialization
	void Start () {
		for(int i = 0; i < instanceNum; ++i)
        {
            Transform t = Instantiate(prefab);
            t.localPosition = Random.insideUnitSphere * radius;
            t.SetParent(transform);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void DoAdvanced()
    {

    }
}
