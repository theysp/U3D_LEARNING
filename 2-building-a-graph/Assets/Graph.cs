using UnityEngine;
using System.Collections;

public class Graph : MonoBehaviour {
    public Transform pointPrefab;
    public int resolution = 10;
    public float xmin = -1f;
    public float xmax = 1f;
    Transform[] points;
    int updateCount=0;
    private void Awake()
    {
        points = new Transform[resolution];
        if(resolution < 2)
            Debug.Log("resolution not valid"+resolution);
        Vector3 pos;
        pos.z = 0f;
        float step = (xmax - xmin) / resolution;
        for (int i = 0; i < resolution; ++i)
        {
            float curx = xmin+step*i;
            points[i] = Instantiate(pointPrefab);
            pos.x = curx;
            pos.y = curx*curx;
            points[i].localPosition = pos;
            points[i].localScale = Vector3.one / resolution;
            points[i].SetParent(this.transform, false);
        }
    }
    // Use this for initialization
    void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 pos;
        updateCount++;
        pos.z = 0f;
        float step = (xmax - xmin) / resolution;
        for (int i = 0; i < resolution; ++i)
        {
            float curx = xmin + step * i;
            pos.x = curx;
            pos.y = Mathf.Sin((curx + updateCount*0.01f)*5);
            points[i].localPosition = pos;
            points[i].localScale = Vector3.one / resolution;
            
        }
	}
}
