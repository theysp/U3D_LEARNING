using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class Stuff : PooledObject
{

    public Rigidbody body { get; private set; }
    public float force;

    private void Awake()
    {
        body = gameObject.GetComponent<Rigidbody>();
    }
    // Use this for initialization
    void Start () {
    }

    void OnTriggerEnter(Collider enteredCollider)
    {
        //Debug.Log("enter zone " + enteredCollider.tag);
        if (enteredCollider.CompareTag("Kill Zone"))
        {
            ReturnToPool();
        }
    }


    // Update is called once per frame
    void Update () {
    }
}
