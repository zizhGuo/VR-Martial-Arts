using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBodyDetectObsidian : MonoBehaviour
{
    public float damageEffectDuration;
    public PlayerInfo player;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "EnemyArm" && !player.beenHit)
        {
            if (name == "BodyTarget")
            {
                player.health -= 10;
                player.lastBeenHitTime = Time.time;
                player.beenHit = true;
                player.StartCoroutine(player.damageEffectAni(0.5f));
            }

            else if (name == "HeadTarget")
            {
                player.health -= 20;
                player.lastBeenHitTime = Time.time;
                player.beenHit = true;
                player.StartCoroutine(player.damageEffectAni(0.5f));
            }

            //print(player.health);

            StartCoroutine(playDamageEffect(damageEffectDuration));
        }
    }

    IEnumerator playDamageEffect(float duration)
    {
        yield return new WaitForSeconds(duration);
        
    }
}
