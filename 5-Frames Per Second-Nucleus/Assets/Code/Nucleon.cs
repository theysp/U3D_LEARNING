using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class Nucleon : MonoBehaviour {
    public float attractionForce = 1;
    Rigidbody body;

    private void Awake()
    {
        body = gameObject.GetComponent<Rigidbody>();
    }
    // Use this for initialization
    void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private void FixedUpdate()
    {
        body.AddForce(transform.localPosition * -attractionForce);
    }
}
