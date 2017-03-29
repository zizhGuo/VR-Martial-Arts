using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Security.Cryptography;
using System;

public class ArmStartAttack : MonoBehaviour
{
    public GameObject obsidianLeader;
    public GameObject leaderPath;
    public GameObject obsidianLeaderShort;
    public GameObject leaderPathShort;
    public GameObject obsidianCreator;
    public GameObject player;
    public float attackCooldown;
    public float minCurveWidth;
    public float maxCurveWidth;
    public float minCurveWidthShort;
    public float maxCurveWidthShort;
    public ObsidianFormCoreBehavior obsidianCore;

    public float attackStartTime;
    public float attackDuration;

    // Use this for initialization
    void Start ()
    {
        StartCoroutine(startAttack());
	}
	
	// Update is called once per frame
	void Update ()
    {
        transform.LookAt(player.transform);

        if(Time.time - attackStartTime >= attackCooldown)
        {
            StartCoroutine(startAttack());
        }
	}

    public IEnumerator startAttack()
    {
        attackStartTime = Time.time;

        //for (int i = 0; i < 100; i++)
        //{
        //    print(betterRandom(0, obsidianCore.playerBodyParts.Length - 1));
        //}

        player = obsidianCore.playerBodyParts[betterRandom(0, obsidianCore.playerBodyParts.Length - 1)];

        if (betterRandom(0, 100) <= 50)
        {
            GameObject newPath = Instantiate(leaderPath, transform.position, transform.rotation);
            Quaternion newRotation = transform.rotation;
            newRotation.eulerAngles = new Vector3(newRotation.eulerAngles.x, newRotation.eulerAngles.y, betterRandom(0, 360));
            newPath.transform.rotation = newRotation;
            newPath.GetComponent<BezierCurve>().points[2].x = (betterRandom((int)(minCurveWidth * 1000f), (int)(maxCurveWidth * 1000f))) / 1000f;

            yield return new WaitForEndOfFrame();

            GameObject newLeader = Instantiate(obsidianLeader, transform.position, transform.rotation);
            newLeader.GetComponent<BezierWalker>().spline = newPath.GetComponent<BezierCurve>();
            attackDuration = newLeader.GetComponent<BezierWalker>().duration + 1f;

            yield return new WaitForEndOfFrame();

            GameObject newCreator = Instantiate(obsidianCreator, transform.position, transform.rotation);
            newCreator.GetComponentInChildren<CreateObsidianArm>().obsidianLeader = newLeader;
            newCreator.transform.localScale = new Vector3((betterRandom((int)(newCreator.GetComponentInChildren<CreateObsidianArm>().minObsidianLength * 1000f), (int)(newCreator.GetComponentInChildren<CreateObsidianArm>().maxObsidianLength * 1000f))) / 1000f,
                                                          (betterRandom((int)(newCreator.GetComponentInChildren<CreateObsidianArm>().minObsidianLength * 1000f), (int)(newCreator.GetComponentInChildren<CreateObsidianArm>().maxObsidianLength * 1000f))) / 1000f,
                                                          (betterRandom((int)(newCreator.GetComponentInChildren<CreateObsidianArm>().minObsidianLength * 1000f), (int)(newCreator.GetComponentInChildren<CreateObsidianArm>().maxObsidianLength * 1000f))) / 1000f);

            newCreator.GetComponentInChildren<CreateObsidianArm>().attackStartTime = attackStartTime;
            newCreator.GetComponentInChildren<CreateObsidianArm>().attackDuration = attackDuration;

            Destroy(newPath, attackDuration + 1.55f);
            Destroy(newLeader, attackDuration + 1.5f);
            Destroy(newCreator, attackDuration + 0.05f);
        }

        else
        {
            GameObject newPath = Instantiate(leaderPathShort, transform.position, transform.rotation);
            Quaternion newRotation = transform.rotation;
            newRotation.eulerAngles = new Vector3(newRotation.eulerAngles.x, newRotation.eulerAngles.y, betterRandom(0, 360));
            newPath.transform.rotation = newRotation;
            newPath.GetComponent<BezierCurve>().points[1].x = (betterRandom((int)(minCurveWidthShort * 1000f), (int)(maxCurveWidthShort * 1000f))) / 1000f;

            yield return new WaitForEndOfFrame();

            GameObject newLeader = Instantiate(obsidianLeaderShort, transform.position, transform.rotation);
            newLeader.GetComponent<BezierWalker>().spline = newPath.GetComponent<BezierCurve>();
            attackDuration = newLeader.GetComponent<BezierWalker>().duration * 6f / 5f;

            yield return new WaitForEndOfFrame();

            GameObject newCreator = Instantiate(obsidianCreator, transform.position, transform.rotation);
            newCreator.GetComponentInChildren<CreateObsidianArm>().obsidianLeader = newLeader;
            newCreator.transform.localScale = new Vector3((betterRandom((int)(newCreator.GetComponentInChildren<CreateObsidianArm>().minObsidianLength * 1000f), (int)(newCreator.GetComponentInChildren<CreateObsidianArm>().maxObsidianLength * 1000f))) / 1000f,
                                                          (betterRandom((int)(newCreator.GetComponentInChildren<CreateObsidianArm>().minObsidianLength * 1000f), (int)(newCreator.GetComponentInChildren<CreateObsidianArm>().maxObsidianLength * 1000f))) / 1000f,
                                                          (betterRandom((int)(newCreator.GetComponentInChildren<CreateObsidianArm>().minObsidianLength * 1000f), (int)(newCreator.GetComponentInChildren<CreateObsidianArm>().maxObsidianLength * 1000f))) / 1000f);

            newCreator.GetComponentInChildren<CreateObsidianArm>().attackStartTime = attackStartTime;
            newCreator.GetComponentInChildren<CreateObsidianArm>().attackDuration = attackDuration;

            Destroy(newPath, attackDuration + 1.55f);
            Destroy(newLeader, attackDuration + 1.5f);
            Destroy(newCreator, attackDuration + 0.05f);
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
