using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StanceRating : MonoBehaviour
{
    public float rating;
    public string letterRating;
    public Text scoreDisplay;
    public Text performanceResponse;

    public float scoreForS;
    public float scoreForA;
    public float scoreForB;
    public float scoreForC;
    public float scoreForD;
    public float scoreForF;

    public int moveCount;
    public int stanceIndex;

	// Use this for initialization
	void Start ()
    {
        rating = 0;
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void displayRating()
    {
        rating /= moveCount;

        if(rating > scoreForS)
        {
            letterRating = "S";
        }
        else if(rating > scoreForA)
        {
            letterRating = "A";
        }
        else if (rating > scoreForB)
        {
            letterRating = "B";
        }
        else if (rating > scoreForC)
        {
            letterRating = "C";
        }
        else if (rating > scoreForD)
        {
            letterRating = "D";
        }
        else
        {
            letterRating = "F";
        }

        scoreDisplay.text = letterRating;
    }
}
