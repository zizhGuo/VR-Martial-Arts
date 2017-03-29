using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShieldDetectObsidian : MonoBehaviour
{
    public ControllerScript controller;

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
        if (other.tag == "EnemyArm" || other.tag == "EnemyWaste")
        {
            controller.controllerActions.TriggerHapticPulse(20f);
            //CreateObsidianArm armObsidian = other.GetComponentInChildren<CreateObsidianArm>();
            //
            //if (armObsidian.isFirstObsidian)
            //{
            //    other.GetComponent<Rigidbody>().isKinematic = false;
            //    other.GetComponent<Rigidbody>().AddExplosionForce(armObsidian.blockBounceOffForce,
            //                        new Vector3(transform.position.x * (1 + (Mathf.Abs(armObsidian.betterRandom((int)(armObsidian.minGeneratePositionOffset * 1000f), (int)(armObsidian.maxGeneratePositionOffset * 1000f))) / 10000f)),
            //                                    transform.position.y * (1 + (Mathf.Abs(armObsidian.betterRandom((int)(armObsidian.minGeneratePositionOffset * 1000f), (int)(armObsidian.maxGeneratePositionOffset * 1000f))) / 10000f)),
            //                                    transform.position.z * (1 + (Mathf.Abs(armObsidian.betterRandom((int)(armObsidian.minGeneratePositionOffset * 1000f), (int)(armObsidian.maxGeneratePositionOffset * 1000f))) / 10000f))),
            //                        armObsidian.blockBounceOffRange);
            //
            //    armObsidian.previousObsidian.GetComponentInChildren<CreateObsidianArm>().isFirstObsidian = true;
            //    other.GetComponent<DestroyObsidianCube>().enabled = true;
            //}
        }
    }
}
