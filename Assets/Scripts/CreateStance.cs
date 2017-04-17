using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Security.Cryptography;

public class CreateStance : MonoBehaviour
{
    public GameObject move;
    public int minMoves;
    public int maxMoves;
    public int minMoveInterval; //The unit will be 1/100 second.
    public int maxMoveInterval;
    public float moveMoveSpeed;
    public StanceRating rating;
    public float stanceStartTime;

    public GameObject previousMove;
    public GameObject currentMove;
    public bool hasPlayerCut;
    public Quaternion moveRotation;
    public float moveInterval;
    public int moveNumber;
    public bool hasDisplayedRating;
    public float stanceCreateTime;

	// Use this for initialization
	void Start ()
    {
        hasPlayerCut = false;
        moveRotation.eulerAngles = new Vector3(0, 270, 0);
        moveInterval = betterRandom(minMoveInterval, maxMoveInterval) / 100f;
        moveNumber = betterRandom(minMoves, maxMoves);
        rating.moveCount = moveNumber;
        hasDisplayedRating = false;
        stanceStartTime = -1;
        stanceCreateTime = Time.time;

        StartCoroutine(createMoves());
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(hasPlayerCut)
        {
            if(Time.time > stanceStartTime + moveNumber * moveInterval + move.GetComponent<MoveRating>().maxAllowTimingError && !hasDisplayedRating && stanceStartTime != -1)
            {
                hasDisplayedRating = true;
                rating.displayRating();
                //print("Missed Last Cut");
            }
        }

        if(!hasPlayerCut && Time.time > stanceCreateTime + 20 && !hasDisplayedRating)
        {
            hasDisplayedRating = true;
            rating.displayRating();
            //print("Stance Time Out");
        }
	}

    IEnumerator createMoves()
    {
        for(int i = 0; i < moveNumber; i++)
        {
            //currentMove = Instantiate(move, transform.position, moveRotation, transform);

            ///For testing
            Quaternion moveR = new Quaternion();
            moveR.eulerAngles = transform.right;
            currentMove = Instantiate(move, transform.position, moveR, transform);
            ///For testing
            
            MoveRating currentRating = currentMove.GetComponent<MoveRating>();

            if(i != 0) //If this is not the first move in this stance, we can assgin previous move to its previousMove, and assign current move to previous move's nextMove
            {
                currentRating.previousMove = previousMove;
                previousMove.GetComponent<MoveRating>().nextMove = currentMove;
            }

            currentRating.moveInterval = moveInterval;
            currentRating.moveIndex = i;
            currentRating.moveSpeed = moveMoveSpeed;
            currentRating.stanceRating = rating;
            currentRating.stanceLogic = this;
            currentRating.creator.attackStartTime = Time.time;
            previousMove = currentMove;

            yield return new WaitForSeconds(moveInterval);
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
