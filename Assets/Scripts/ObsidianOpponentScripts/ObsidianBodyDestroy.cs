using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Security.Cryptography;
using System;

public class ObsidianBodyDestroy : MonoBehaviour
{
    public DestroyObsidianCube destroyAni;
    public float bodyExplodeForce;

    public Rigidbody thisRigidbody;
	// Use this for initialization
	void Start ()
    {
        destroyAni.enabled = true;
        gameObject.AddComponent<Rigidbody>();
        thisRigidbody = GetComponent<Rigidbody>();
        thisRigidbody.isKinematic = false;
        thisRigidbody.AddForce(new Vector3(-bodyExplodeForce * transform.forward.x, 
                                           -bodyExplodeForce * transform.forward.y, 
                                           -bodyExplodeForce * transform.forward.z), ForceMode.Impulse);
    }
	
	// Update is called once per frame
	void Update ()
    {

    }

    #region Better random number generator

    public readonly RNGCryptoServiceProvider _generator = new RNGCryptoServiceProvider();

    public int betterRandom(int minimumValue, int maximumValue)
    {
        byte[] randomNumber = new byte[1];

        _generator.GetBytes(randomNumber);

        double asciiValueOfRandomCharacter = Convert.ToDouble(randomNumber[0]);

        // We are using Math.Max, and substracting 0.00000000001, 
        // to ensure "multiplier" will always be between 0.0 and .99999999999
        // Otherwise, it's possible for it to be "1", which causes problems in our rounding.
        double multiplier = Math.Max(0, (asciiValueOfRandomCharacter / 255d) - 0.00000000001d);

        // We need to add one to the range, to allow for the rounding done with Math.Floor
        int range = maximumValue - minimumValue + 1;

        double randomValueInRange = Math.Floor(multiplier * range);

        return (int)(minimumValue + randomValueInRange);
    }
    #endregion
}
