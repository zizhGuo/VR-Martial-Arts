using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfo : MonoBehaviour
{
    public int health;
    public float canBeHitCooldown; //So that the player won't be hit by one attack multiple times.
    public Text healthText;
    public Image damageEffect;
    public Text winLoseText;
    public Color damageEffectColor;

    public bool beenHit;
    public float lastBeenHitTime;

	// Use this for initialization
	void Start ()
    {
        health = 100;
        healthText.text = "Health: " + health.ToString();

    }
	
	// Update is called once per frame
	void Update ()
    {
        healthText.text = "Health: " + health.ToString();

        if (beenHit && Time.time - lastBeenHitTime >= canBeHitCooldown)
        {
            beenHit = false;
        }

        if(health <= 0)
        {
            //Player lose
            winLoseText.text = "You Lose!";
        }

        if(health < 0)
        {
            health = 0;
        }
	}

    public IEnumerator damageEffectAni(float effectTime)
    {
        damageEffect.color = damageEffectColor;

        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / effectTime)
        {
            //print(t);
            Color newColor = new Color(1, 0, 0, Mathf.Lerp(160f / 255f, 0, t));
            damageEffect.color = newColor;
            yield return null;
        }

        damageEffect.color = new Color(1, 0, 0, 0);
    }
}
