using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Security.Cryptography;
using System;

public class ObsidianFormCoreBehavior : MonoBehaviour
{
    public GameObject bodyStarter;
    public GameObject[] playerBodyParts;
    public int minContinueAttackTime;
    public int maxContinueAttackTime;
    public GameObject armStart;
    public float nextAttackWaitTime;

    public float currentAttackStartTime;
    public float currentAttackEndTime;
    public float currentAttackDuration;
    public bool isAttacking;
    public bool isCoreReady;
    public float gameStartTime;

	// Use this for initialization
	void Start ()
    {
		for(int i = 0; i < 15; i ++)
        {
            for(int j = 0; j < 15; j ++)
            {
                GameObject newStarter = Instantiate(bodyStarter, transform.position, transform.rotation);
                newStarter.transform.eulerAngles = new Vector3(i * 24, j * 24, 0);
            }
        }

        isAttacking = false;

        gameStartTime = Time.time;
	}
	
	// Update is called once per frame
	void Update ()
    {
		if(Time.time - gameStartTime >= 5)
        {
            isCoreReady = true;
        }

        if(isCoreReady && !isAttacking)
        {
            isAttacking = true;
            currentAttackStartTime = Time.time;
            currentAttackDuration = (betterRandom(minContinueAttackTime * 1000, maxContinueAttackTime * 1000)) / 1000f;
            currentAttackEndTime = Time.time + currentAttackDuration;
            armStart.transform.LookAt(playerBodyParts[betterRandom(0, playerBodyParts.Length - 1)].transform);
            armStart.SetActive(true);
        }

        if(isAttacking && Time.time - currentAttackStartTime >= currentAttackDuration)
        {
            armStart.SetActive(false);
        }

        if(isAttacking && Time.time - currentAttackEndTime >= nextAttackWaitTime)
        {
            isAttacking = false;
        }
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
