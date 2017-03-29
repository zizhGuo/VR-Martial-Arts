using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkerDetectShield : MonoBehaviour
{

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PlayerShield")
        {
            GetComponentInParent<BezierWalker>().enabled = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "PlayerShield")
        {
            GetComponentInParent<BezierWalker>().enabled = true;
        }
    }
}
