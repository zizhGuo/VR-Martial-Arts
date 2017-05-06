using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Security.Cryptography;

public class PlaySwingAudio : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] audioClips;
    public Rigidbody rigidBody;
    public float minimumTriggerVelocity;
    public float differentDirDotValue;

    public bool isAudioPlaying;
    public float lastPlayedTime;
    public AudioClip currentClip;
    public Vector3 velocity;
    public Vector3 lastVelocity;
    public float speed;

	// Use this for initialization
	void Start ()
    {
        isAudioPlaying = false;
        lastVelocity = new Vector3(0, 0, 0);
	}
	
	// Update is called once per frame
	void Update ()
    {
        velocity = rigidBody.velocity;
        speed = rigidBody.velocity.magnitude;


        if (rigidBody.velocity.magnitude >= minimumTriggerVelocity)
        {
            //print(Vector3.Dot(rigidBody.velocity.normalized, lastVelocity.normalized));

            if (Vector3.Dot(rigidBody.velocity.normalized, lastVelocity.normalized) < differentDirDotValue || !isAudioPlaying)
            {
                print("DIFF" + Vector3.Dot(rigidBody.velocity.normalized, lastVelocity.normalized));

                isAudioPlaying = true;
                currentClip = audioClips[betterRandom(0, audioClips.Length - 1)];

                audioSource.clip = currentClip;
                audioSource.Play();
                lastVelocity = rigidBody.velocity;
                lastPlayedTime = Time.time;
            }
        }

        if(isAudioPlaying && Time.time - lastPlayedTime >= currentClip.length)
        {
            isAudioPlaying = false;
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
