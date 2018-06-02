using UnityEngine;
using System.Collections;

public class FPSCounter : MonoBehaviour {

    public int FPS { get; private set; }
    public int maxFPS { get; private set; }
    public int meanFPS { get; private set; }
    int calcTimeFPS;
    float startTime = -1f;
    int frameCount=0;
    // Use this for initialization
    void Start () {
        maxFPS = 0;
        meanFPS = 0;
        calcTimeFPS = 0;
        startTime = Time.time;
    }
	
	// Update is called once per frame
	void Update () {
        frameCount++;
        FPS = (int)(1f / Time.unscaledDeltaTime);
        float endTime = Time.time - startTime;
        meanFPS = (int)(frameCount/ endTime);
        maxFPS = FPS > maxFPS ? FPS : maxFPS;
    }
}
