using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideControllerModel : MonoBehaviour
{
    public GameObject model;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    { 
		if(model.activeInHierarchy)
        {
            model.SetActive(false);
        }
	}
}
