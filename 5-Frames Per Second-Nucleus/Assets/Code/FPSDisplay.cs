using UnityEngine;
using UnityEngine.UI;   
using System.Collections;

[RequireComponent(typeof(FPSCounter))]
public class FPSDisplay : MonoBehaviour {

    public Text fpsLabel;
    FPSCounter fpsCounter;

    private void Awake()
    {
        fpsCounter = gameObject.GetComponent<FPSCounter>();
    }
    // Use this for initialization
    void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
        fpsLabel.text = Mathf.Clamp(fpsCounter.FPS,0,99).ToString();

    }
}
