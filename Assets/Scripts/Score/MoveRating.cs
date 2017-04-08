using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveRating : MonoBehaviour
{
    public float anglePercent;
    public float forcePercent;
    public float swingPercent;
    public float displacementPercent;
    public float timingPercent;

    public float angleRating;
    public float forceRating;
    public float swingRating;
    public float displacementRating;
    public float timingRating;
    public float totalMoveRating;
    public float currentMoveTime = 0;
    public int totalColliderNumber;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        totalMoveRating = (anglePercent * angleRating + 
                          forcePercent * forceRating + 
                          swingPercent * swingRating + 
                          timingPercent * timingRating +
                          displacementPercent * displacementRating) / totalColliderNumber;
	}
}
