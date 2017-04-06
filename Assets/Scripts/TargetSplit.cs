using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSplit : MonoBehaviour
{
    public GameObject target;
    //public GameObject left;
    //public GameObject right;
    public float forceToCut;
    public float horizontalSwingGrace;
    public float bladeCutAngleGrace;
    public float horizontalDisplacementGrace;

    public bool isBlade;
    public bool isSwing;
    public Vector3 originalPosition;
    public Quaternion originalRotation;

    void Awake ()
    {
        originalPosition = transform.localPosition;
        originalRotation = transform.localRotation;
    }

    // Use this for initialization
    void Start ()
    {
        isBlade = false;
        isSwing = false;
	}
	
	// Update is called once per frame
	void Update ()
    {
        transform.localPosition = originalPosition;
        transform.localRotation = originalRotation;
    }

    //void OnTriggerEnter(Collider cut)
    //{
    //    print("trigger is: " + cut.name);
    //
    //    if (cut.transform.name == "Blade")
    //    {
    //        isBlade = true;
    //    }
    //}

    void OnCollisionEnter(Collision collision)
    {
        Collider col = collision.collider;

        if (col.transform.name == "BladeEdge")
        {
            BladeLogic blade = col.GetComponent<BladeLogic>();

            blade.cutCount++;

            print("collide speed: " + blade.bladeVelocity + ", collide direction: " + Mathf.Abs(transform.up.y - col.transform.up.y) + ", cut# " + blade.cutCount);// + transform.up + ", " + col.transform.up);

            isBlade = true;

            if (blade.bladeVelocity.y <= -forceToCut && 
                //Mathf.Abs(blade.bladeVelocity.x) <= horizontalSwingGraceDistance && 
                Mathf.Abs(blade.bladeVelocity.z) <= horizontalSwingGrace)
            {
                print("cut speed: " + blade.bladeVelocity + ", collide direction: " + Mathf.Abs(transform.up.y - col.transform.up.y) + ", cut# " + blade.cutCount);
                isSwing = true;

                if(Mathf.Abs(transform.up.y - col.transform.up.y) <= bladeCutAngleGrace)
                {

                    print("cut direction: " + Mathf.Abs(transform.up.y - col.transform.up.y) + ", cut# " + blade.cutCount);

                    //split();

                }
            }
        }
    }

    /*
    void split()
    {
        //ScoreCounter counter = FindObjectOfType<ScoreCounter>();
        //counter.score += 1;
        //counter.scoreText.text = counter.score.ToString();
        left.SetActive(true);
        right.SetActive(true);
        left.GetComponent<Rigidbody>().AddForce(-target.transform.right, ForceMode.Impulse);
        right.GetComponent<Rigidbody>().AddForce(target.transform.right, ForceMode.Impulse);
        target.GetComponent<MeshRenderer>().enabled = false;
        Destroy(target, 5f);
        gameObject.SetActive(false);
    }
    */
}
