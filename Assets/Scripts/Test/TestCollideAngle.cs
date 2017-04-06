using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCollideAngle : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    void OnCollisionEnter(Collision collision)
    {
        Collider col = collision.collider;

        //print("collide speed: " + blade.bladeVelocity + ", collide direction: " + Mathf.Abs(transform.up.x - col.transform.up.x) + ", cut# " + blade.cutCount);// + transform.up + ", " + col.transform.up);

        print("upX: " + Mathf.Abs(transform.up.normalized.x - col.transform.up.normalized.x));
        print("upY: " + Mathf.Abs(transform.up.normalized.y - col.transform.up.normalized.y));
        print("upZ: " + Mathf.Abs(transform.up.normalized.z - col.transform.up.normalized.z));
    }
}
