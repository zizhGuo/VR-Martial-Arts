using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Security.Cryptography;
using System;

public class CreateObsidianBody : MonoBehaviour
{
    public float generateSpeed;
    public float generateDistance;
    public GameObject obsidianCube;
    public float minObsidianLength;
    public float maxObsidianLength;
    public float minGeneratePositionOffset;
    public float maxGeneratePositionOffset;

    public float generateInterval;
    public GameObject obsidianLeader;
    public GameObject nextObsidian;
    public GameObject previousObsidian;
    public bool isFirstObsidian;
    public bool isCreatingObsidian;
    public GameObject thisObsidianCube;
    public Rigidbody thisObsidianCubeRigidbody;
    public float cubeShrinkScale;
    public float startTime;

    // Use this for initialization
    void Start()
    {
        generateInterval = 100f / (generateSpeed + 100f) * (betterRandom(80, 160) / 100f);
        //print(generateInterval);
        isFirstObsidian = true;
        isCreatingObsidian = false;
        thisObsidianCube = transform.parent.gameObject;
        thisObsidianCubeRigidbody = thisObsidianCube.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(obsidianLeader.transform);

        if (nextObsidian != null)
        {
            isFirstObsidian = false;
        }

        //else if (nextObsidian = null)
        //{
        //    isFirstObsidian = true;
        //}

        if (isFirstObsidian && !isCreatingObsidian)
        {
            isCreatingObsidian = true;
            StartCoroutine(generateObsidian(generateInterval));
        }
    }

    public IEnumerator generateObsidian(float waitTime)
    {
        yield return new WaitForSecondsRealtime(waitTime);

        if (Time.time - startTime <= obsidianLeader.GetComponent<BezierWalker>().duration + 2)
        {
            Vector3 generatePosi = transform.position + transform.forward.normalized * generateDistance;

            GameObject newObsidian = Instantiate(obsidianCube, new Vector3(generatePosi.x + ((betterRandom((int)(minGeneratePositionOffset * 1000f), (int)(maxGeneratePositionOffset * 1000f))) / 1000f),
                                                                           generatePosi.y + ((betterRandom((int)(minGeneratePositionOffset * 1000f), (int)(maxGeneratePositionOffset * 1000f))) / 1000f),
                                                                           generatePosi.z + ((betterRandom((int)(minGeneratePositionOffset * 1000f), (int)(maxGeneratePositionOffset * 1000f))) / 1000f)), transform.rotation);
            newObsidian.SetActive(false);
            newObsidian.transform.localScale = new Vector3((betterRandom((int)(minObsidianLength * 1000f), (int)(maxObsidianLength * 1000f))) / (1000f * cubeShrinkScale),
                                                           (betterRandom((int)(minObsidianLength * 1000f), (int)(maxObsidianLength * 1000f))) / (1000f * cubeShrinkScale),
                                                           (betterRandom((int)(minObsidianLength * 1000f), (int)(maxObsidianLength * 1000f))) / (1000f * cubeShrinkScale));
            newObsidian.transform.rotation = transform.rotation;
            newObsidian.GetComponentInChildren<CreateObsidianBody>().obsidianLeader = obsidianLeader;
            newObsidian.GetComponentInChildren<CreateObsidianBody>().previousObsidian = transform.parent.gameObject;
            newObsidian.GetComponentInChildren<CreateObsidianBody>().cubeShrinkScale = cubeShrinkScale;
            newObsidian.GetComponentInChildren<CreateObsidianBody>().startTime = startTime;
            newObsidian.tag = "EnemyBody";
            newObsidian.SetActive(true);
            nextObsidian = newObsidian.gameObject;
            isFirstObsidian = false;
            isCreatingObsidian = false;
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
