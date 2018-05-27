using UnityEngine;
using System.Collections;

public class Graph : MonoBehaviour {
    public Transform pointPrefab;
    public int resolution = 101;
    public GraphFunctionName funcName = GraphFunctionName.Torus;
    static float pi = Mathf.PI;

    static Vector3 Sin2d(float x, float z, float t)
    {
        Vector3 ret;
        ret.x = x;
        ret.z = z;
        ret.y = Mathf.Sin(x + t) * Mathf.Sin(z + t);
        return ret;
    }

    static Vector3 MultiSin2d(float x, float z, float t)
    {
        Vector3 ret;
        ret.x = x;
        ret.z = z;
        ret.y = (Mathf.Sin(pi * (x + t)) + Mathf.Sin(pi * (z + t))) / 2;
        return ret;
    }

    static Vector3 MultiSine2DFunction(float x, float z, float t)
    {
        Vector3 ret;
        ret.x = x;
        ret.z = z;
        float y = 4f * Mathf.Sin(pi * (x + z + t * 0.5f));
        y += Mathf.Sin(pi * (x + t));
        y += Mathf.Sin(2f * pi * (z + 2f * t)) * 0.5f;
        y *= 1f / 5.5f;
        ret.y = y;
        return ret;
    }

    static Vector3 Ripple(float x, float z, float t)
    {
        Vector3 ret;
        float d = Mathf.Sqrt(x * x + z * z);
        ret.x = x;
        ret.z = z;
        ret.y = Mathf.Sin(4f*Mathf.PI*d+3*t)*0.1f/(d+0.2f);
        return ret;
    }

    static Vector3 Torus(float u, float v, float t)
    {
        Vector3 ret;
        float pi = Mathf.PI;
        float r1 = 1.0f;
        float r2 = 0.5f;
        float s = r1 + Mathf.Sin(pi*v)*r2;
        ret.x = s * Mathf.Sin(pi * u) ;
        ret.z = s * Mathf.Cos(pi * u) ;
        ret.y = r2 * Mathf.Cos(pi * v);        
        return ret;
    }

    static Vector3 RipleTorus(float u, float v, float t)
    {
        Vector3 ret;
        float pi = Mathf.PI;
        float r1 = 1.0f;
        float r2 = 0.5f;
        float s = r1 + Mathf.Sin(pi * (u+v)*0.5f) * r2;
        ret.x = s * Mathf.Sin(pi * Mathf.Sin(pi*u+t));
        ret.z = s * Mathf.Cos(pi * Mathf.Sin(pi*u + t));
        ret.y = r2 * Mathf.Cos(pi * Mathf.Sin(pi*v + t));
        return ret;
    }

    static GraphFunction[] functions =
    {
        Sin2d,
        MultiSin2d,
        MultiSine2DFunction,
        Ripple,
        Torus,
        RipleTorus
    };

    Transform[] points;

    private void Awake()
    {
        points = new Transform[resolution* resolution];
        float step = 2f / resolution;
        Vector3 position;
        for (int i=0;i< resolution; ++i)
        {
            for(int j=0;j<resolution;++j)
            {
                points[i*resolution+j] = Instantiate(pointPrefab);
                position.x = i * step-1f;
                position.z = j * step - 1f;
                position = functions[(int)funcName](position.x, position.z, 0);
                points[i * resolution + j].localPosition = position;
                points[i * resolution + j].localScale = Vector3.one / resolution*2f;
            }

        }
    }
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        float step = 2f / resolution;
        float curTime = Time.time;
        for (int i = 0; i < resolution; ++i)
        {
            for (int j = 0; j < resolution; ++j)
            {
                Vector3 position;
                position.x = i * step - 1f;
                position.z = j * step - 1f;
                position = functions[(int)funcName](position.x, position.z, curTime);
                points[i * resolution + j].localPosition = position;
            }

        }
    }
}
