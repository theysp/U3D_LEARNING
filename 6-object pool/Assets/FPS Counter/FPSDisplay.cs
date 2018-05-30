using UnityEngine;
using UnityEngine.UI;   
using System.Collections;

[RequireComponent(typeof(FPSCounter))]
public class FPSDisplay : MonoBehaviour {

    public Text fpsLabel1;
    public Text fpsLabel2;
    public Text fpsLabel3;
    FPSCounter fpsCounter;
    int fpsCalcTimes = 0;
    int maxFps = 0;
    int totalFps = 0;

    private void Awake()
    {
        fpsCounter = gameObject.GetComponent<FPSCounter>();
    }
    // Use this for initialization
    void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
        fpsLabel1.text = Mathf.Clamp(fpsCounter.FPS,0,99).ToString();
        fpsLabel1.color = Color.HSVToRGB((float)(0.33*(100- Mathf.Clamp(fpsCounter.FPS, 0, 99))/100), 1.0f, 1.0f);
        fpsLabel2.text = Mathf.Clamp(fpsCounter.meanFPS, 0, 99).ToString();
        fpsLabel2.color = Color.HSVToRGB((float)(0.33 * (100 - Mathf.Clamp(fpsCounter.meanFPS, 0, 99)) / 100), 1.0f, 1.0f);
        fpsLabel3.text = Mathf.Clamp(fpsCounter.maxFPS, 0, 99).ToString();
        fpsLabel3.color = Color.HSVToRGB((float)(0.33 * (100 - Mathf.Clamp(fpsCounter.maxFPS, 0, 99)) / 100), 1.0f, 1.0f);
    }
}
