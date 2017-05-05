using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Security.Cryptography;

public class MoveRating : MonoBehaviour
{
    public float anglePercent;
    public float forcePercent;
    public float swingPercent;
    public float displacementPercent;
    public float timingPercent;
    public float timingGrace;
    public float maxAllowTimingError;
    public GameObject path;
    public GameObject leader;
    public CreateObsidianArm creator;
    public float minFactorRating; //A scale from 0 - 1, any of the five factors received a percentage score lower than this will result in a 0 in the total rating of this move.

    public float angleRating;
    public float forceRating;
    public float swingRating;
    public float displacementRating;
    public float timingRating;
    public float totalMoveRating;
    public float currentMoveTime = 0; //This is the timing the player need to aim for. The first move will always get a full timing score, and later moves correct timing will be depended by the time for first move.
    public float currentCutTime;
    public float currentTimeError;
    public int totalColliderNumber = 0;
    public int hitColliderNumber = 0;
    public StanceRating stanceRating;
    public CreateStance stanceLogic;
    public bool isFirstMove; //If this is first move the player hit in the current stance, the timing rating will be 1.
    public Vector3 newPosition;
    public float moveSpeed;
    public GameObject previousMove;
    public GameObject nextMove;
    public int moveIndex;
    public float moveInterval;
    public bool hasSendScore;

	// Use this for initialization
	void Start ()
    {
        path = GetComponentInChildren<BezierCurve>().gameObject;
        //Quaternion cutDirection = path.transform.rotation;
        //cutDirection.eulerAngles = new Vector3(betterRandom(0, 360), cutDirection.eulerAngles.y, cutDirection.eulerAngles.z);

        ///For testing
        //cutDirection.eulerAngles = new Vector3(betterRandom(0, 360), cutDirection.eulerAngles.y, -90);
        transform.LookAt(FindObjectOfType<SteamVR_Camera>().transform);
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);

        path.transform.LookAt(FindObjectOfType<SteamVR_Camera>().transform);
        Quaternion cutDirection = path.transform.rotation;
        cutDirection.eulerAngles = new Vector3(betterRandom(0, 360), cutDirection.eulerAngles.y - 90f, 0);
        //cutDirection.eulerAngles = new Vector3(225, cutDirection.eulerAngles.y - 90f, 0);
        path.transform.rotation = cutDirection;
        ///For testing

        //path.transform.rotation = cutDirection;
        path.GetComponent<BezierCurve>().points[1].y = path.GetComponent<BezierCurve>().points[2].y =
            (betterRandom(-40, 40) / 100f);
        //path.GetComponent<BezierCurve>().points[1].y = path.GetComponent<BezierCurve>().points[2].y = 0;

        leader = Instantiate(leader, transform.position, transform.rotation);
        leader.GetComponent<BezierWalker>().spline = path.GetComponent<BezierCurve>();
        GetComponentInChildren<CreateObsidianArm>().obsidianLeader = leader;

        leader.GetComponent<BezierWalker>().enabled = true;
        GetComponentInChildren<CreateObsidianArm>().enabled = true;

        currentCutTime = -1;
        currentMoveTime = 0;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if(currentCutTime != -1 && !stanceLogic.hasPlayerCut) //Assign the stance start time when the player first hit a move.
        {
            stanceLogic.hasPlayerCut = true;
            stanceLogic.stanceStartTime = currentCutTime;
        }

        if(currentCutTime != -1 && stanceLogic.hasPlayerCut && previousMove == null && currentMoveTime == 0) //Assign the move start time when player first hit a move.
        {
            currentMoveTime = currentCutTime;
        }

        if(previousMove != null) //Assign correct time to cut to each move after the player first hit a move.
        {
            if(stanceLogic.hasPlayerCut && currentMoveTime == 0 && previousMove.GetComponent<MoveRating>().currentMoveTime != 0)
            {
                currentMoveTime = stanceLogic.stanceStartTime + moveIndex * moveInterval;
            }
        }

        if(Time.time > stanceLogic.stanceCreateTime + moveIndex * moveInterval + maxAllowTimingError)
        {
            Destroy(leader);
        }

        //newPosition = transform.position;
        //newPosition.z -= moveSpeed * Time.deltaTime;
        //transform.position = newPosition;

        ///Testing
        newPosition = transform.position;
        newPosition += transform.forward * moveSpeed * Time.deltaTime;
        transform.position = newPosition;
        ///Testing

        if (currentMoveTime != 0 && currentCutTime != -1)
        {
            currentTimeError = Mathf.Abs(currentCutTime - currentMoveTime);
            timingRating = (maxAllowTimingError - (currentTimeError - timingGrace)) / (maxAllowTimingError - timingGrace);

            if(timingRating > 1)
            {
                timingRating = 1;
            }

            if(timingRating < 0)
            {
                timingRating = 0;
            }
        }

        timingRating *= totalColliderNumber;

        totalMoveRating = (anglePercent * angleRating +
                          forcePercent * forceRating +
                          swingPercent * swingRating +
                          timingPercent * timingRating +
                          displacementPercent * displacementRating) / totalColliderNumber;

        //if(angleRating / totalColliderNumber <= minFactorRating ||
        //    forceRating / totalColliderNumber <= minFactorRating ||
        //    swingRating / totalColliderNumber <= minFactorRating ||
        //    timingRating <= minFactorRating ||
        //    displacementRating / totalColliderNumber <= minFactorRating)
        //{
        //    totalMoveRating = 0;
        //}

        //Adjust the rating to player's lowest factor (If a player perform bad in anyone of the rating factors, the total rating will suffer a lot)
        float lowestFactorRating = totalColliderNumber;

        if(angleRating < lowestFactorRating)
        {
            lowestFactorRating = angleRating;
        }
        if(forceRating < lowestFactorRating)
        {
            lowestFactorRating = forceRating;
        }
        if(swingRating < lowestFactorRating)
        {
            lowestFactorRating = swingRating;
        }
        if(timingRating < lowestFactorRating)
        {
            lowestFactorRating = timingRating;
        }
        if(displacementRating < lowestFactorRating)
        {
            lowestFactorRating = displacementRating;
        }

        if (lowestFactorRating / totalColliderNumber <= minFactorRating)
        {
            lowestFactorRating /= hitColliderNumber;
            totalMoveRating *= Mathf.Pow(lowestFactorRating, 2f);
        }
        else
        {
            lowestFactorRating /= hitColliderNumber;
            totalMoveRating *= Mathf.Pow(lowestFactorRating, 0.5f);
        }

        if (Time.time > currentMoveTime + maxAllowTimingError && !hasSendScore && currentCutTime != -1 && currentMoveTime != 0)
        {
            hasSendScore = true;
            //print("MoveRating: " + moveIndex + ", " + totalMoveRating + ", Time: " + Time.time + ", move time:" + currentMoveTime);
            stanceRating.rating += totalMoveRating;
            //print("StanceRating: " + moveIndex + ", " + stanceRating.rating);

            if(moveIndex + 1 == stanceLogic.moveNumber && !stanceLogic.hasDisplayedRating)
            {
                stanceLogic.hasDisplayedRating = true;
                stanceRating.displayRating();
                //print("Get Last Cut");
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
