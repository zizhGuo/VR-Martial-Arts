using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class ColliderScore : MonoBehaviour
{
    public StartMove move;
    //public GameObject left;
    //public GameObject right;
    public float forceToCut;
    public float horizontalSwingGrace;
    public float horizontalMaxAllowSwing;
    public float bladeCutAngleGrace;
    public float horizontalDisplacementGrace;
    public float horizontalMaxAllowDisplacement;
    public VRTK_ControllerActions controllerAction;
    public GameScore gameManager;

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

    void Awake()
    {
        originalPosition = transform.localPosition;
        originalRotation = transform.localRotation;
    }

    // Use this for initialization
    void Start()
    {
        controllerAction = FindObjectOfType<VRTK_ControllerActions>();
        gameManager = FindObjectOfType<GameScore>();
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

            BladeLogic blade = col.GetComponent<BladeLogic>();
            
            //foreach (ContactPoint contact in collision.contacts)
            //{
            //    Debug.DrawRay(contact.point, contact.normal, Color.white);
            //}

            if(isBlade)
            {
                return;
            }

            if(move.inkDraw.color == Color.black)
            {
                move.inkDraw.color = Color.red;
                gameManager.moveIndex = move.moveIndex + 1;
            }

            isBlade = true;
            //print("blade edge: " + col.transform.name + ", Move Index:" + move.moveIndex + ", collider rotation: " + transform.up + ", blade rotation: " + col.transform.up + ", blade velocity: " + blade.bladeVelocity);

            cutForce = Mathf.Abs(blade.bladeVelocity.y);

            controllerAction.TriggerHapticPulse(cutForce / 20f, 0.1f, 0.01f);
            //controllerAction.TriggerHapticPulse(0.5f, 0.5f, 0.01f);

            cutForceRating = cutForce / forceToCut;
            if (cutForceRating > 1)
            {
                cutForceRating = 1;
            }
            if (cutForceRating < 0)
            {
                cutForceRating = 0;
            }

            if (Mathf.Abs(transform.up.y) > 0.5f)
            {
                if (Mathf.Abs(transform.up.y - col.transform.up.y) <= 1)
                {
                    cutAngle = Mathf.Abs(Mathf.Abs(transform.up.x) - Mathf.Abs(col.transform.up.x));
                }
                else
                {
                    cutAngle = 2f - Mathf.Abs(Mathf.Abs(transform.up.x) - Mathf.Abs(col.transform.up.x));
                }
            }
            else
            {
                if (Mathf.Abs(transform.up.x - col.transform.up.x) <= 1)
                {
                    cutAngle = Mathf.Abs(Mathf.Abs(transform.up.y) - Mathf.Abs(col.transform.up.y));
                }
                else
                {
                    cutAngle = 2f - Mathf.Abs(Mathf.Abs(transform.up.y) - Mathf.Abs(col.transform.up.y));
                }
            }

            //cutAngle = Mathf.Abs(transform.up.x - col.transform.up.x);
            cutAngleRating = (2f - (cutAngle - bladeCutAngleGrace)) / (2f - bladeCutAngleGrace);
            if(cutAngleRating > 1)
            {
                cutAngleRating = 1;
            }
            if(cutAngleRating < 0)
            {
                cutAngleRating = 0;
            }

            cutHorizontalSwing = Mathf.Abs(blade.bladeVelocity.x);
            cutHorizontalSwingRating = (horizontalMaxAllowSwing - (cutHorizontalSwing - horizontalSwingGrace)) / (horizontalMaxAllowSwing - horizontalSwingGrace);
            if (cutHorizontalSwingRating > 1)
            {
                cutHorizontalSwingRating = 1;
            }
            if (cutHorizontalSwingRating < 0)
            {
                cutHorizontalSwingRating = 0;
            }

            cutDisplacement = Vector3.Distance(collision.contacts[0].point, transform.position);
            //print("Distance: " + cutDisplacement);
            cutDisplacementRating = (horizontalMaxAllowDisplacement - (cutDisplacement - horizontalDisplacementGrace)) / (horizontalMaxAllowDisplacement - horizontalDisplacementGrace);
            if (cutDisplacementRating > 1)
            {
                cutDisplacementRating = 1;
            }
            if (cutAngleRating < 0)
            {
                cutAngleRating = 0;
            }

            if (move.currentCutTime == -1)
            {
                move.currentCutTime = Time.time;
            }

            move.angleRating += cutAngleRating;
            move.forceRating += cutForceRating;
            move.swingRating += cutHorizontalSwingRating;
            move.displacementRating += cutDisplacementRating;
            move.hitColliderNumber++;

            Destroy(gameObject);

            //print("Angle rating: " + cutAngleRating + ", Force rating: " + cutForceRating + ", Swing rating: " + cutHorizontalSwingRating + ", Displacement rating" + cutDisplacementRating);
        }
    }
}

