﻿using UnityEngine;
using System.Collections;

public class StuffSpawner : MonoBehaviour {

    public float velocity;
    public float spawnDistance = 10.0f;
    public float timeBetweenSpawns;
    float timeSinceLastSpawn;
    public int numprefabs = 0;
    public Color color;
    
    public Stuff prefab0;
    public Stuff prefab1;
    public Stuff prefab2;
    public Stuff prefab3;
    public Stuff prefab4;
    public Stuff prefab5;
    public Stuff prefab6;
    public Stuff prefab7;
    public Stuff prefab8;
    
    public Stuff[] stuffPrefabs;

    public Material material;

    private void Awake()
    {
        stuffPrefabs = new Stuff[8];
        stuffPrefabs[0] = prefab0;
        stuffPrefabs[1] = prefab1;
        stuffPrefabs[2] = prefab2;
        stuffPrefabs[3] = prefab3;
        stuffPrefabs[4] = prefab4;
        stuffPrefabs[5] = prefab5;
        stuffPrefabs[6] = prefab6;
        stuffPrefabs[7] = prefab7;
    }
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    private void FixedUpdate()
    {
        timeSinceLastSpawn += Time.deltaTime;
        if(timeSinceLastSpawn > timeBetweenSpawns)
        {
            timeSinceLastSpawn -= timeBetweenSpawns;
            SpawnStuff();
        }
    }

    void SpawnStuff()
    {
        if (numprefabs < 1)
            return;
        Stuff prefab = stuffPrefabs[Random.Range(0, numprefabs)];
        Stuff spawn = Instantiate<Stuff>(prefab);
        Vector3 pos = new Vector3(spawnDistance,0f, 0f);
        spawn.transform.localPosition = pos;
        Vector3 velocityVec = new Vector3(-1f, 0.7f, 0f);
        spawn.body.velocity = velocity*velocityVec;
        spawn.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
        spawn.GetComponent<MeshRenderer>().material = material;
    }
}
