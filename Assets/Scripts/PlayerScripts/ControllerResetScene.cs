using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;
using UnityEngine.SceneManagement;

public class ControllerResetScene : MonoBehaviour
{

    public VRTK_ControllerActions controllerActions;

    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (VRTK_SDK_Bridge.IsButtonOnePressedDownOnIndex(VRTK_DeviceFinder.GetControllerIndex(gameObject)))
        {
            SceneManager.LoadScene("Prototype");
        }
    }
}
