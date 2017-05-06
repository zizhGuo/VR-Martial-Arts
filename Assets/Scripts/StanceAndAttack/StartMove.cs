using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartMove : MonoBehaviour
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
    public float manualZ; //Custom local Z rotation for the colliders
    public float leaderTime;
    public SpriteRenderer inkDraw;
    public GameScore gameManager;

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
    public StartStance stanceLogic;
    public bool isFirstMove; //If this is first move the player hit in the current stance, the timing rating will be 1.
    public Vector3 newPosition;
    public float moveSpeed;
    public GameObject previousMove;
    public GameObject nextMove;
    public int moveIndex;
    public float moveInterval;
    public bool hasSendScore;
    public float originalEulerY;
    public int factorType;
    public bool hasChangeColor;

    // Use this for initialization
    void Start()
    {
        originalEulerY = transform.localEulerAngles.y;
        path = GetComponentInChildren<BezierCurve>().gameObject;

        transform.LookAt(FindObjectOfType<SteamVR_Camera>().transform);
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);

        path.transform.LookAt(FindObjectOfType<SteamVR_Camera>().transform);
        Quaternion cutDirection = path.transform.rotation;
        cutDirection.eulerAngles = new Vector3(0, cutDirection.eulerAngles.y + 180f + originalEulerY, cutDirection.eulerAngles.z);
        path.transform.rotation = cutDirection;
        
        leader = Instantiate(leader, transform.position, transform.rotation);
        leader.GetComponent<BezierWalker>().spline = path.GetComponent<BezierCurve>();
        GetComponentInChildren<CreateObsidianArm>().obsidianLeader = leader;

        leader.GetComponent<BezierWalker>().duration = leaderTime;
        creator.attackDuration = leaderTime * 1.2f;

        leader.GetComponent<BezierWalker>().enabled = true;
        GetComponentInChildren<CreateObsidianArm>().enabled = true;

        currentCutTime = -1;
        currentMoveTime = 0;

        transform.localEulerAngles = new Vector3(0, 0, 0);

        factorType = 0;
        gameManager = FindObjectOfType<GameScore>();
        hasChangeColor = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentCutTime != -1 && !stanceLogic.hasPlayerCut) //Assign the stance start time when the player first hit a move.
        {
            stanceLogic.hasPlayerCut = true;
            stanceLogic.stanceStartTime = currentCutTime;
        }

        if (currentCutTime != -1 && stanceLogic.hasPlayerCut && previousMove == null && currentMoveTime == 0) //Assign the move start time when player first hit a move.
        {
            currentMoveTime = currentCutTime;
        }

        if (previousMove != null) //Assign correct time to cut to each move after the player first hit a move.
        {
            if (stanceLogic.hasPlayerCut && currentMoveTime == 0 && previousMove.GetComponent<StartMove>().currentMoveTime != 0)
            {
                currentMoveTime = stanceLogic.stanceStartTime + moveIndex * moveInterval;
            }
        }

        if (Time.time > stanceLogic.stanceCreateTime + moveIndex * moveInterval + maxAllowTimingError)
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

            if (timingRating > 1)
            {
                timingRating = 1;
            }

            if (timingRating < 0)
            {
                timingRating = 0;
            }
        }


        if(currentCutTime != -1 && Time.time - currentCutTime > 1 && !hasChangeColor)
        {
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

            if (angleRating < lowestFactorRating)
            {
                lowestFactorRating = angleRating;
                factorType = 1;
            }
            if (forceRating < lowestFactorRating)
            {
                lowestFactorRating = forceRating;
                factorType = 2;
            }
            if (swingRating < lowestFactorRating)
            {
                lowestFactorRating = swingRating;
                factorType = 3;
            }
            if (timingRating < lowestFactorRating)
            {
                lowestFactorRating = timingRating;
                factorType = 4;
            }
            if (displacementRating < lowestFactorRating)
            {
                lowestFactorRating = displacementRating;
                factorType = 5;
            }

            //if (lowestFactorRating / totalColliderNumber <= minFactorRating) //doing bad
            //{
            //    lowestFactorRating /= hitColliderNumber;
            //    totalMoveRating *= Mathf.Pow(lowestFactorRating, 2f);
            //    //print("AAA");
            //}
            //else //doing good
            //{
            //    lowestFactorRating /= hitColliderNumber;
            //    totalMoveRating *= Mathf.Pow(lowestFactorRating, 0.5f);
            //    //print("BBB");
            //}

            hasChangeColor = true;
            GameObject markToChangeColor = null;
            //print(hitColliderNumber + ", " + totalMoveRating);

            if (moveIndex + 1 == 1)
            {
                markToChangeColor = gameManager.moveImgActive1;
            }

            else if (moveIndex + 1 == 2)
            {
                markToChangeColor = gameManager.moveImgActive2;
            }

            else if (moveIndex + 1 == 3)
            {
                markToChangeColor = gameManager.moveImgActive3;
            }

            markToChangeColor.GetComponent<Image>().color = Color.green;

            if (totalMoveRating <= 0.85)
            {
                markToChangeColor.GetComponent<Image>().color = Color.yellow;
            }

            if (totalMoveRating <= 0.6)
            {
                markToChangeColor.GetComponent<Image>().color = Color.red;
            }
        }

        if (Time.time > currentMoveTime + maxAllowTimingError && !hasSendScore && currentCutTime != -1 && currentMoveTime != 0)
        {
            hasSendScore = true;
            //print("MoveRating: " + moveIndex + ", " + totalMoveRating + ", Time: " + Time.time + ", move time:" + currentMoveTime);


            stanceRating.rating += totalMoveRating;
            //print("StanceRating: " + moveIndex + ", " + stanceRating.rating);
            for (int i = 0; i < 5; i++)
            {
                if (i == 0)
                {
                    gameManager.roundScore[stanceRating.stanceIndex, moveIndex, i] = Mathf.RoundToInt(angleRating / totalColliderNumber * 100);
                }

                if (i == 1)
                {
                    gameManager.roundScore[stanceRating.stanceIndex, moveIndex, i] = Mathf.RoundToInt(forceRating / totalColliderNumber * 100);
                }

                if (i == 2)
                {
                    gameManager.roundScore[stanceRating.stanceIndex, moveIndex, i] = Mathf.RoundToInt(swingRating / totalColliderNumber * 100);
                }

                if (i == 3)
                {
                    gameManager.roundScore[stanceRating.stanceIndex, moveIndex, i] = Mathf.RoundToInt(displacementRating / totalColliderNumber * 100);
                }

                if (i == 4)
                {
                    gameManager.roundScore[stanceRating.stanceIndex, moveIndex, i] = Mathf.RoundToInt(timingRating * 100);
                }

            }

            if (moveIndex + 1 == stanceLogic.moveNumber && !stanceLogic.hasDisplayedRating)
            {
                stanceLogic.hasDisplayedRating = true;
                stanceRating.displayRating();
                //print("Get Last Cut");
            }

            //if(factorType == 0)
            //{
            //    stanceRating.performanceResponse.text = "Missed!";
            //}

            //else if (factorType == 1)
            //{
            //    stanceRating.performanceResponse.text = "Bad sword angle!";
            //}

            //else if(factorType == 2)
            //{
            //    stanceRating.performanceResponse.text = "Too weak!";
            //}

            //else if(factorType == 3)
            //{
            //    stanceRating.performanceResponse.text = "Swing not straight!";
            //}

            //else if(factorType == 4)
            //{
            //    stanceRating.performanceResponse.text = "Bad timing!";
            //}

            //else if(factorType == 5)
            //{
            //    stanceRating.performanceResponse.text = "Aim is not precise!";
            //}
        }
    }
}
