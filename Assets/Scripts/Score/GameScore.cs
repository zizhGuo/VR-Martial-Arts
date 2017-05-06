using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameScore : MonoBehaviour
{
    public GameObject moveImgNormal1;
    public GameObject moveImgNormal2;
    public GameObject moveImgNormal3;
    public GameObject moveImgActive1;
    public GameObject moveImgActive2;
    public GameObject moveImgActive3;
    public Text scoreText;
    public Text[] finalStats;
    public GameObject panel;
    public AudioSource gong;
    public AudioSource BGM;

    public int moveIndex;
    public bool active1;
    public bool active2;
    public bool active3;
    public float totalTotalScore;
    public float lastStanceTime;

    public float[,,] roundScore;

	// Use this for initialization
	void Start ()
    {
        active1 = false;
        active2 = false;
        active3 = false;
        moveIndex = 0;
        roundScore = new float[5,3,5];
    }
	
	// Update is called once per frame
	void Update ()
    {
		if(moveIndex == 1 && !active1)
        {
            active1 = true;
            moveImgNormal1.SetActive(false);
            moveImgActive1.SetActive(true);
        }

        if (moveIndex == 2 && !active2)
        {
            active2 = true;
            moveImgNormal2.SetActive(false);
            moveImgActive2.SetActive(true);
        }

        if (moveIndex == 3 && !active3)
        {
            active3 = true;
            moveImgNormal3.SetActive(false);
            moveImgActive3.SetActive(true);
        }
    }

    public void reset()
    {
        active1 = false;
        moveImgNormal1.SetActive(true);
        moveImgActive1.SetActive(false);
        moveImgActive1.GetComponent<Image>().color = Color.white;

        active2 = false;
        moveImgNormal2.SetActive(true);
        moveImgActive2.SetActive(false);
        moveImgActive2.GetComponent<Image>().color = Color.white;

        active3 = false;
        moveImgNormal3.SetActive(true);
        moveImgActive3.SetActive(false);
        moveImgActive3.GetComponent<Image>().color = Color.white;

        scoreText.text = "";

        moveIndex = 0;
    }

    public void gameOver()
    {
        gong.Play();
        BGM.Pause();

        int x = 0;

        for(int i = 0; i < 5; i++)
        {
            for(int j = 0; j < 3; j++)
            {
                for(int k = 0; k < 5; k++)
                {
                    //print(roundScore[i,j,k]);
                    finalStats[x].text = roundScore[i, j, k].ToString() + "%";
                    x++;
                }
            }
        }

        panel.SetActive(true);
    }
}
