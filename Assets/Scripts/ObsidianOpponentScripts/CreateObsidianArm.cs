﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class CreateObsidianArm : MonoBehaviour
{
    public float generateSpeed;
    public float generateDistance;
    public GameObject obsidianCube;
    public float minObsidianLength;
    public float maxObsidianLength;
    public float minGeneratePositionOffset;
    public float maxGeneratePositionOffset;
    public float waitForDestroyTime;
    public GameObject moveWrap;

    public float generateInterval;
    public GameObject obsidianLeader;
    public GameObject nextObsidian;
    public GameObject previousObsidian;
    public bool isFirstObsidian;
    public bool isCreatingObsidian;
    public float attackStartTime;
    public float attackDuration;
    public GameObject thisObsidianCube;
    public Rigidbody thisObsidianCubeRigidbody;

    // Use this for initialization
    void Start ()
    {
        generateInterval = 100f / (generateSpeed + 100f);
        transform.LookAt(obsidianLeader.transform);
        isFirstObsidian = true;
        isCreatingObsidian = false;
        thisObsidianCube = transform.parent.gameObject;
        thisObsidianCubeRigidbody = thisObsidianCube.GetComponent<Rigidbody>();

        //if (moveWrap.GetComponent<MoveRating>().currentMoveTime == 0)
        //{
        //    moveWrap.GetComponent<MoveRating>().currentMoveTime = Time.time;
        //}
    }
	
	// Update is called once per frame
	void Update ()
    {
        if(Time.time - attackStartTime >= attackDuration)
        {
            StartCoroutine(waitForDestroy(waitForDestroyTime));
            //thisObsidianCube.GetComponent<Collider>().enabled = false;
        }

        if (obsidianLeader != null)
        {
            transform.LookAt(obsidianLeader.transform);
        }

        if(!isCreatingObsidian)
        {
            //attackStartTime = Time.time;
        }

        if (isFirstObsidian && !isCreatingObsidian && Time.time - attackStartTime < (attackDuration - 0.1f))
        {
            isCreatingObsidian = true;
            StartCoroutine(generateObsidian(generateInterval));
        }
    }

    public IEnumerator generateObsidian(float waitTime)
    {
        yield return new WaitForSecondsRealtime(waitTime);

        Vector3 generatePosi = transform.position + transform.forward.normalized * generateDistance;
        //print(transform.forward.normalized * generateDistance);

        GameObject newObsidian = Instantiate(obsidianCube, new Vector3(generatePosi.x + ((betterRandom((int)(minGeneratePositionOffset * 1000f), (int)(maxGeneratePositionOffset * 1000f))) / 1000f),
                                                                       generatePosi.y + ((betterRandom((int)(minGeneratePositionOffset * 1000f), (int)(maxGeneratePositionOffset * 1000f))) / 1000f),
                                                                       generatePosi.z + ((betterRandom((int)(minGeneratePositionOffset * 1000f), (int)(maxGeneratePositionOffset * 1000f))) / 1000f)), transform.rotation, moveWrap.transform);
        newObsidian.SetActive(false);
        newObsidian.transform.localScale = new Vector3((betterRandom((int)(minObsidianLength * 1000f), (int)(maxObsidianLength * 1000f))) / 1000f,
                                                       (betterRandom((int)(minObsidianLength * 1000f), (int)(maxObsidianLength * 1000f))) / 1000f,
                                                       (betterRandom((int)(minObsidianLength * 1000f), (int)(maxObsidianLength * 1000f))) / 1000f);
        CreateObsidianArm newObsidianCreator = newObsidian.GetComponentInChildren<CreateObsidianArm>();
        newObsidianCreator.obsidianLeader = obsidianLeader;
        newObsidianCreator.previousObsidian = transform.parent.gameObject;
        newObsidianCreator.attackStartTime = attackStartTime;
        newObsidianCreator.attackDuration = attackDuration;
        newObsidianCreator.moveWrap = moveWrap;
        newObsidian.GetComponentInChildren<ColliderScore>().move = GetComponentInParent<MoveRating>();
        newObsidianCreator.generateSpeed = generateSpeed;
        newObsidianCreator.generateDistance = generateDistance;
        newObsidianCreator.minObsidianLength = minObsidianLength;
        newObsidianCreator.maxObsidianLength = maxObsidianLength;
        newObsidianCreator.minGeneratePositionOffset = minGeneratePositionOffset;
        newObsidianCreator.maxGeneratePositionOffset = maxGeneratePositionOffset;
        newObsidianCreator.waitForDestroyTime = waitForDestroyTime;
        newObsidian.SetActive(true);
        moveWrap.GetComponent<MoveRating>().totalColliderNumber += 1;
        nextObsidian = newObsidian.gameObject;
        isFirstObsidian = false;
        isCreatingObsidian = false;
    }
    
    public IEnumerator waitForDestroy(float time)
    {
        yield return new WaitForSeconds(time);

        //thisObsidianCubeRigidbody.isKinematic = false;
        //thisObsidianCubeRigidbody.useGravity = true;
        //thisObsidianCubeRigidbody.AddForce(new Vector3(0, -10, 0), ForceMode.Impulse);
        thisObsidianCube.GetComponent<DestroyObsidianCube>().enabled = true;
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
