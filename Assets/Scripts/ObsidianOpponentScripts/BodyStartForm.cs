using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Security.Cryptography;
using System;

public class BodyStartForm : MonoBehaviour
{
    public GameObject obsidianLeader;
    public GameObject leaderPath;
    public GameObject obsidianCreator;
    public GameObject player;
    public float cubeShrinkScale;


    // Use this for initialization
    void Start()
    {
        leaderPath = GetComponentInChildren<BezierCurve>().gameObject;
        startGenerate();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void startGenerate()
    {

        GameObject newLeader = Instantiate(obsidianLeader, transform.position, transform.rotation);
        newLeader.GetComponent<BezierWalker>().spline = leaderPath.GetComponent<BezierCurve>();

        GameObject newCreator = Instantiate(obsidianCreator, transform.position, transform.rotation);
        newCreator.GetComponentInChildren<CreateObsidianBody>().obsidianLeader = newLeader;
        newCreator.GetComponentInChildren<CreateObsidianBody>().cubeShrinkScale = cubeShrinkScale;
        newCreator.GetComponentInChildren<CreateObsidianBody>().startTime = Time.time;
        newCreator.transform.localScale = new Vector3((betterRandom((int)(newCreator.GetComponentInChildren<CreateObsidianBody>().minObsidianLength * 1000f), (int)(newCreator.GetComponentInChildren<CreateObsidianBody>().maxObsidianLength * 1000f))) / (1000f * cubeShrinkScale),
                                                      (betterRandom((int)(newCreator.GetComponentInChildren<CreateObsidianBody>().minObsidianLength * 1000f), (int)(newCreator.GetComponentInChildren<CreateObsidianBody>().maxObsidianLength * 1000f))) / (1000f * cubeShrinkScale),
                                                      (betterRandom((int)(newCreator.GetComponentInChildren<CreateObsidianBody>().minObsidianLength * 1000f), (int)(newCreator.GetComponentInChildren<CreateObsidianBody>().maxObsidianLength * 1000f))) / (1000f * cubeShrinkScale));
        
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
