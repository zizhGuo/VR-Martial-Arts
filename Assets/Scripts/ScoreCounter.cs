using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreCounter : MonoBehaviour
{
    public Text scoreText;
    public int score;

    // Use this for initialization
    void Start ()
    {
        score = 0;
        scoreText.text = score.ToString();
	}
	
	// Update is called once per frame
	void Update ()
    {
        
	}
}
