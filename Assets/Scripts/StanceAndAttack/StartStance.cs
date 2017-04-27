using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartStance : MonoBehaviour
{
    public GameObject[] move;
    public float moveMoveSpeed;
    public StanceRating rating;
    public float stanceStartTime;
    public int moveNumber;
    public float moveInterval;

    public GameObject previousMove;
    public GameObject currentMove;
    public bool hasPlayerCut;
    public Quaternion moveRotation;
    public bool hasDisplayedRating;
    public float stanceCreateTime;

    // Use this for initialization
    void Start()
    {
        hasPlayerCut = false;
        rating.moveCount = moveNumber;
        hasDisplayedRating = false;
        stanceStartTime = -1;
        stanceCreateTime = Time.time;

        StartCoroutine(createMoves());
    }

    // Update is called once per frame
    void Update()
    {
        if (hasPlayerCut)
        {
            if (Time.time > stanceStartTime + moveNumber * moveInterval + move[0].GetComponent<StartMove>().maxAllowTimingError && !hasDisplayedRating && stanceStartTime != -1)
            {
                hasDisplayedRating = true;
                rating.displayRating();
                //print("Missed Last Cut");
            }
        }

        if (!hasPlayerCut && Time.time > stanceCreateTime + 20 && !hasDisplayedRating)
        {
            hasDisplayedRating = true;
            rating.displayRating();
            //print("Stance Time Out");
        }
    }

    IEnumerator createMoves()
    {
        for (int i = 0; i < moveNumber; i++)
        {
            Quaternion moveR = new Quaternion();
            moveR.eulerAngles = transform.right;
            moveR.eulerAngles = new Vector3(moveR.eulerAngles.x, moveR.eulerAngles.y + move[i].transform.localEulerAngles.y, moveR.eulerAngles.z);
            currentMove = Instantiate(move[i], transform.TransformPoint(move[i].transform.localPosition), moveR, transform);
            currentMove.SetActive(true);

            StartMove currentRating = currentMove.GetComponent<StartMove>();

            if (i != 0) //If this is not the first move in this stance, we can assgin previous move to its previousMove, and assign current move to previous move's nextMove
            {
                currentRating.previousMove = previousMove;
                previousMove.GetComponent<StartMove>().nextMove = currentMove;
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
}
