using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Security.Cryptography;

public class InkSpill : MonoBehaviour
{
    public int minDropAmount;
    public int maxDropAmount;
    public int maxVelocityDiff;
    public int maxAngleDiff;
    public GameObject inkDrop;
    public GameObject warrior;
    public float destroyDelay;
    public float forceTuneDown;
    public AudioClip[] audioClips;
    public AudioSource audioSource;

    public bool isSpilled;
    public Vector3 cutForce;
    public Vector3 colPos;
    //public Vector3 cutDir;
    public Quaternion spillDir;
    public AudioClip currentClip;

    // Use this for initialization
    void Start ()
    {
        isSpilled = false;
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void OnCollisionEnter(Collision col)
    {
        //print("AAAAAAAAAAAAAAAA");
        Collider coL = col.collider;
        colPos = col.contacts[0].point;
        //print(coL.name);

        if (col.transform.name == "BladeEdgeA" || col.transform.name == "BladeEdgeB")
        {
            if (!isSpilled)
            {
                isSpilled = true;

                //BladeLogic blade = coL.GetComponent<BladeLogic>();
                currentClip = audioClips[betterRandom(0, audioClips.Length - 1)];
                audioSource.clip = currentClip;
                audioSource.Play();
                //cutForce = blade.bladeVelocity / forceTuneDown;

                cutForce = col.impulse / forceTuneDown;
                cutForce.x += betterRandom(-maxAngleDiff, maxAngleDiff) / 100f;
                cutForce.y += betterRandom(-maxAngleDiff, maxAngleDiff) / 100f;
                cutForce.z += betterRandom(-maxAngleDiff, maxAngleDiff) / 100f;
                //print(cutForce);

                spillDir.SetLookRotation(col.impulse.normalized, Vector3.up);

                for (int i = 0; i <= betterRandom(minDropAmount, maxDropAmount); i++)
                {
                    GameObject newDrop = Instantiate(inkDrop, colPos, spillDir);
                    newDrop.GetComponent<Rigidbody>().AddForce(cutForce * (1f + (betterRandom(-maxVelocityDiff, maxVelocityDiff)) / 100f), ForceMode.Impulse);
                }

                Destroy(warrior, 0.06f);
            }
        }
    }

    #region Better random number generator

    private static readonly RNGCryptoServiceProvider _generator = new RNGCryptoServiceProvider();

    public static int betterRandom(int minimumValue, int maximumValue)
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
