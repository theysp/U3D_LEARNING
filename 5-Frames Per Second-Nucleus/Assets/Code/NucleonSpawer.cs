using UnityEngine;
using System.Collections;

//scene --> object --> components

public class NucleonSpawer : MonoBehaviour {

    public float timeBetweenSpawns = 1;

    public float spawnDistance = 1;

    public Nucleon[] nucleonPrefabs = { null, null };

    public Nucleon prefab1;
    public Nucleon prefab2;

    float timeSinceLastSpawn;

    //object创建完成后立即调用
    private void Awake()
    {
        nucleonPrefabs[0] = prefab1;
        nucleonPrefabs[1] = prefab2;
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
        Random.Range(-1f, 1f);
        if (timeSinceLastSpawn >= timeBetweenSpawns)
        {
            timeSinceLastSpawn -= timeBetweenSpawns;
            SpawnNucleon();
        }
    }

    void SpawnNucleon () {
		Nucleon prefab = nucleonPrefabs[Random.Range(0, nucleonPrefabs.Length)];
		Nucleon spawn = Instantiate<Nucleon>(prefab);
		spawn.transform.localPosition = Random.onUnitSphere * spawnDistance;
	}
}
