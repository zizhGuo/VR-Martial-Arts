using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObjects : MonoBehaviour
{

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void OnTriggerExit(Collider col)
    {
        //print(col.name);
        if(col.tag == "stanceDestroy")
        {
            Destroy(col.gameObject);
        }
    }

    //public void OnTriggerEnter(Collider col)
    //{
    //    print(col.name);
    //    if (col.tag == "stanceDestroy")
    //    {
    //        Destroy(col.gameObject);
    //    }
    //}
}
