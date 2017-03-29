using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObsidianCube : MonoBehaviour
{
    public float timeTilDestroy;
    public float destroyAnimationDuration;

	// Use this for initialization
	void Start ()
    {
        StartCoroutine(destroyObsidian(timeTilDestroy));
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public IEnumerator destroyObsidian(float waitTime)
    {
        yield return new WaitForSecondsRealtime(waitTime);

        StartCoroutine(destroyObsidianAnimation(destroyAnimationDuration));
        Destroy(gameObject, destroyAnimationDuration);
    }

    public IEnumerator destroyObsidianAnimation(float duration)
    {
        yield return new WaitForSecondsRealtime(duration);


    }
}
