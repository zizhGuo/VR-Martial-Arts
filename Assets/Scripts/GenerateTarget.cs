using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class GenerateTarget : MonoBehaviour
{
    public GameObject target;
    public Transform player;
    public Text scoreDisplay;
    public GameScore gameManager;
    public Text performanceResponse;

    public Quaternion shootDirection;
    public int stanceIndex;
	// Use this for initialization
	void Start ()
    {
        StartCoroutine(createTarget(8f));
        gameManager = FindObjectOfType<GameScore>();
        stanceIndex = 0;
	}
	
	// Update is called once per frame
	void Update ()
    {
        transform.LookAt(player);
        shootDirection = transform.rotation;
        shootDirection.eulerAngles = new Vector3(0, shootDirection.eulerAngles.y, 0);

        if(stanceIndex == 5)
        {
            StartCoroutine(wait());
            GetComponent<GenerateTarget>().enabled = false;
        }
	}

    IEnumerator wait()
    {
        yield return new WaitForSeconds(10);

        gameManager.gameOver();
    }

    IEnumerator createTarget(float interval)
    {
        while (true && stanceIndex < 5)
        {
            yield return new WaitForSeconds(5f);

            GameObject newTarget = Instantiate(target, transform.position, shootDirection);
            newTarget.GetComponent<StanceRating>().scoreDisplay = scoreDisplay;
            newTarget.GetComponent<StanceRating>().performanceResponse = performanceResponse;
            newTarget.GetComponent<StanceRating>().stanceIndex = stanceIndex;
            //newTarget.GetComponent<Rigidbody>().AddForce(newTarget.transform.forward * 3f, ForceMode.Impulse);
            Destroy(newTarget, 30f);

            yield return new WaitForSeconds(6.5f);
            gameManager.lastStanceTime = Time.time;
            gameManager.reset();

            yield return new WaitForSeconds(interval - 5f);

            stanceIndex++;
        }

        //while (true && stanceIndex < 5)
        //{
        //    yield return new WaitForSeconds(5f);

        //    GameObject newTarget = Instantiate(target, transform.position, shootDirection);
        //    newTarget.GetComponent<StanceRating>().scoreDisplay = scoreDisplay;
        //    newTarget.GetComponent<StanceRating>().performanceResponse = performanceResponse;
        //    newTarget.GetComponent<StanceRating>().stanceIndex = stanceIndex;
        //    //newTarget.GetComponent<Rigidbody>().AddForce(newTarget.transform.forward * 3f, ForceMode.Impulse);
        //    Destroy(newTarget, 30f);


        //    gameManager.lastStanceTime = Time.time;
        //    gameManager.reset();


        //    stanceIndex++;
        //}
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
