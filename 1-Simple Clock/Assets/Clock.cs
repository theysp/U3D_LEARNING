using UnityEngine;
using System;
using System.Collections;

public class Clock : MonoBehaviour {
    public Transform hoursTransform;
    public Transform minutesTransform;
    public Transform secondsTransform;
    float degreePerHour = 30f;
    float degreePerMinute = 6f;
    float degreePerSecond = 6f;
    private void Awake()
    {
        hoursTransform.localRotation = Quaternion.Euler(0f, DateTime.Now.Hour * degreePerHour, 0f);
        minutesTransform.localRotation = Quaternion.Euler(0f, DateTime.Now.Minute * degreePerMinute, 0f);
        secondsTransform.localRotation = Quaternion.Euler(0f, DateTime.Now.Second * degreePerSecond, 0f);
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        DateTime now = DateTime.Now;
        hoursTransform.localRotation = Quaternion.Euler(0f, now.Hour * degreePerHour, 0f);
        minutesTransform.localRotation = Quaternion.Euler(0f, now.Minute * degreePerMinute, 0f);
        secondsTransform.localRotation = Quaternion.Euler(0f, now.Second * degreePerSecond, 0f);
    }
}
