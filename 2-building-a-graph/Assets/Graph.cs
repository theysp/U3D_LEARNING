using UnityEngine;
using System.Collections;

public class Graph : MonoBehaviour {
    public Transform pointPrefab;

    private void Awake()
    {
        for(int i=0;i<10;++i)
        {
            Transform point = Instantiate(pointPrefab);
            point.localPosition = Vector3.right*((i+0.5f)/5f-1f);
            point.localScale = Vector3.one / 5f;
        }
    }
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    
	}
}
