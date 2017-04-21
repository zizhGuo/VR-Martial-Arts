using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BladeLogic : MonoBehaviour
{
    //public int bladeRayCount;
    //public float bladeRaySpacing;
    //public float bladeRayLength;
    public Transform speedMeter;
    public Rigidbody bladeRigid;

    public Vector3 originalPosition;
    public Quaternion originalRotation;
    //public Vector3 bladeTip;
    //public Bounds bounds;
    public Vector3 oldPosition;
    public Vector3 newPosition;
    public Vector3 bladeDisplacement;
    public Vector3 bladeVelocity;
    public int cutCount;

    void Awake()
    {
        originalPosition = transform.localPosition;
        originalRotation = transform.localRotation;
    }

    // Use this for initialization
    void Start ()
    {
        //bounds = GetComponent<Collider>().bounds;

        oldPosition = speedMeter.position;
        cutCount = 0;
    }
	
	// Update is called once per frame
	void Update ()
    {
        //transform.localPosition = originalPosition;
        //transform.localRotation = originalRotation;


        newPosition = speedMeter.position;
        bladeDisplacement = (newPosition - oldPosition);
        bladeVelocity = bladeDisplacement / Time.deltaTime;
        bladeVelocity = transform.InverseTransformDirection(bladeVelocity);
        //bladeVelocity *= 10000000;
        oldPosition = newPosition;
        newPosition = transform.position;

    }


    void FixedUpdate()
    {

    }


    //void updateRaycastOrigins()
    //{
    //    bladeTip = new Vector3(bounds.min.x, bounds.min.y, bounds.center.z);
    //}
    //
    //void detectBladeCollision()
    //{
    //    for (int i = 0; i < bladeRayCount; i++)
    //    {
    //        Vector3 rayOrigin = bladeTip;
    //        rayOrigin += transform.forward * (bladeRaySpacing * i);
    //        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, -transform.up, bladeRayLength);
    //
    //        Debug.DrawRay(rayOrigin, -transform.up, Color.red);
    //
    //        if (hit)
    //        {
    //
    //        }
    //    }
    //}
}
