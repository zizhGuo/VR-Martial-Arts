﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderScore : MonoBehaviour
{
    public MoveRating move;
    //public GameObject left;
    //public GameObject right;
    public float forceToCut;
    public float horizontalSwingGrace;
    public float horizontalMaxAllowSwing;
    public float bladeCutAngleGrace;
    public float horizontalDisplacementGrace;
    public float horizontalMaxAllowDisplacement;
    public float timingGrace;

    public bool isBlade;
    public bool isSwing;
    public Vector3 originalPosition;
    public Quaternion originalRotation;
    public float cutAngle;
    public float cutDisplacement;
    public float cutForce;
    public float cutHorizontalSwing;
    public float cutAngleRating;
    public float cutDisplacementRating;
    public float cutForceRating;
    public float cutHorizontalSwingRating;
    public float currentMoveTime;
    public float cutTimingRating;

    void Awake()
    {
        originalPosition = transform.localPosition;
        originalRotation = transform.localRotation;
    }

    // Use this for initialization
    void Start()
    {
        isBlade = false;
        isSwing = false;
    }

    // Update is called once per frame
    void Update()
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

        if (col.transform.name == "BladeEdgeA" || col.transform.name == "BladeEdgeB")
        {
            print(col.transform.name + ", isBlade: " + isBlade + ", transform collider: " + transform.up + ", transform blade: " + col.transform.up);

            BladeLogic blade = col.GetComponent<BladeLogic>();
            
            foreach (ContactPoint contact in collision.contacts)
            {
                Debug.DrawRay(contact.point, contact.normal, Color.white);
            }

            if(isBlade)
            {
                return;
            }

            isBlade = true;

            cutForce = Mathf.Abs(blade.bladeVelocity.y);
            cutForceRating = cutForce / forceToCut;
            if (cutForceRating > 1)
            {
                cutForceRating = 1;
            }
            if (cutForceRating < 0)
            {
                cutForceRating = 0;
            }

            if (Mathf.Abs(transform.up.z - col.transform.up.z) <= 1)
            {
                cutAngle = Mathf.Abs(transform.up.x - col.transform.up.x);
            }
            else
            {
                cutAngle = 2f - Mathf.Abs(transform.up.x - col.transform.up.x);
            }
            cutAngleRating = (2f - (cutAngle - bladeCutAngleGrace)) / (2f - bladeCutAngleGrace);
            if(cutAngleRating > 1)
            {
                cutAngleRating = 1;
            }
            if(cutAngleRating < 0)
            {
                cutAngleRating = 0;
            }

            cutHorizontalSwing = Mathf.Abs(blade.bladeVelocity.z);
            cutHorizontalSwingRating = (horizontalMaxAllowSwing - (cutHorizontalSwing - horizontalSwingGrace)) / (horizontalMaxAllowSwing - horizontalSwingGrace);
            if (cutHorizontalSwingRating > 1)
            {
                cutHorizontalSwingRating = 1;
            }
            
            cutDisplacement = Vector3.Distance(collision.contacts[0].point, transform.position);
            print("Distance: " + cutDisplacement);
            cutDisplacementRating = (horizontalMaxAllowDisplacement - (cutDisplacement - horizontalDisplacementGrace)) / (horizontalMaxAllowDisplacement - horizontalDisplacementGrace);
            if (cutDisplacementRating > 1)
            {
                cutDisplacementRating = 1;
            }

            move.angleRating = cutAngleRating;
            move.forceRating = cutForceRating;
            move.swingRating = cutHorizontalSwingRating;
            move.displacementRating = cutDisplacementRating;
            move.timingRating = cutTimingRating;

            print("Angle rating: " + cutAngleRating + ", Force rating: " + cutForceRating + ", Swing rating: " + cutHorizontalSwingRating + ", Displacement rating" + cutDisplacementRating);
        }
    }
}

