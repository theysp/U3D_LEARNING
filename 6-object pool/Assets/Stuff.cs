using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class Stuff : MonoBehaviour {

    public Rigidbody body { get; private set; }
    Transform transf;
    public float force;

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
}
