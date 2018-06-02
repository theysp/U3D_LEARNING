using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectPool : MonoBehaviour {

    PooledObject prefab;
    List<PooledObject> availableObjects = new List<PooledObject>();

    // Use this for initialization
    void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public PooledObject GetObject()
    {
        PooledObject obj;
        if (availableObjects.Count > 0)
        {
            obj = availableObjects[0];
            availableObjects.RemoveAt(0);
            obj.gameObject.SetActive(true);
        }
        else
        {
            obj = Instantiate<PooledObject>(prefab);
            obj.transform.SetParent(transform, false);
            obj.Pool = this;
        }
        return obj;
    }

    public void AddObject(PooledObject obj)
    {
        obj.gameObject.SetActive(false);
        availableObjects.Add(obj);
    }

    static Dictionary<string, ObjectPool> poolDict = new Dictionary<string, ObjectPool>();

    public static ObjectPool GetPool(PooledObject prefab)
    {
        string poolName = prefab.name + "Pool";
        if (poolDict.ContainsKey(poolName) && GameObject.Find(prefab.name + " Pool"))
        {
            return poolDict[poolName];
        }
        else
        {
            GameObject obj = new GameObject(prefab.name + " Pool");
            DontDestroyOnLoad(obj);
            ObjectPool pool = obj.AddComponent<ObjectPool>();
            pool.prefab = prefab;
            poolDict[poolName] = pool;
            return pool;
        }
    }
}
