using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Security.Cryptography;

public class InkDropLogic : MonoBehaviour
{
    public GameObject[] inkMarks;
    public Rigidbody thisRigid;
    public float sizeTuneDown;
    public float fadeSpeed;

    public Vector3 newScale;
    public Vector3 spillPos;
    public GameObject newMark;
    public Color newColor;
    public SpriteRenderer markSprite;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(newMark != null && newMark.GetComponentInChildren<SpriteRenderer>().color.a > 0)
        {
            if(markSprite == null)
            {
                markSprite = newMark.GetComponentInChildren<SpriteRenderer>();
            }

            newColor = markSprite.color;
            newColor.a -= Time.deltaTime / fadeSpeed;
            markSprite.color = newColor;
        }

        if(markSprite != null && markSprite.color.a <= 0.05)
        {
            Destroy(gameObject);
            Destroy(newMark);
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.tag == "ground")
        {
            GetComponent<MeshRenderer>().enabled = false;
            spillPos = transform.position;
            spillPos.y += 0.1f;
            newMark = Instantiate(inkMarks[betterRandom(0, inkMarks.Length - 1)], spillPos, transform.rotation);

            newMark.transform.rotation.SetLookRotation(newMark.transform.rotation.eulerAngles, Vector3.up);
            newScale = newMark.transform.localScale / sizeTuneDown;
            newScale.x *= 1f + thisRigid.velocity.x;
            newScale.y *= 1f + thisRigid.velocity.y;
            newScale.z *= 1f + thisRigid.velocity.z;
            newMark.transform.localScale = newScale;
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
