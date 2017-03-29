using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyMaintainRotation : MonoBehaviour
{


    Vector3 verticalRotation;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		verticalRotation = new Vector3(0, transform.eulerAngles.y, 0);
        transform.eulerAngles = verticalRotation;
	}
}
