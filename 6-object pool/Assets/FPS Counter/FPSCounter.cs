using UnityEngine;
using System.Collections;

public class FPSCounter : MonoBehaviour {

    public int FPS { get; private set; }
    public int maxFPS { get; private set; }
    public int meanFPS { get; private set; }
    int calcTimeFPS;
    // Use this for initialization
    void Start () {
        maxFPS = 0;
        meanFPS = 0;
        calcTimeFPS = 0;
    }
	
	// Update is called once per frame
	void Update () {
        FPS = (int)(1f / Time.unscaledDeltaTime);
        meanFPS = (int)((meanFPS * calcTimeFPS + FPS) / 1.0 / (++calcTimeFPS));
        maxFPS = FPS > maxFPS ? FPS : maxFPS;
    }
}
