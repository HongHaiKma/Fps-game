using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DontGoThroughThings : MonoBehaviour
{
    // Careful when setting this to true - it might cause double
    // events to be fired - but it won't pass through the trigger
    public bool sendTriggerMessage = false;

    public LayerMask m_LMChar; //make sure we aren't in this layer 
    public LayerMask m_LMMap; //make sure we aren't in this layer 
    public float skinWidth = 0.1f; //probably doesn't need to be changed 

    private float minimumExtent;
    private float partialExtent;
    private float sqrMinimumExtent;
    private Vector3 previousPosition;
    public Rigidbody myRigidbody;
    public Collider myCollider;

    public Bullet m_Bullet;

    //initialize values 
    void Start()
    {
        previousPosition = myRigidbody.position;
        minimumExtent = Mathf.Min(Mathf.Min(myCollider.bounds.extents.x, myCollider.bounds.extents.y), myCollider.bounds.extents.z);
        partialExtent = minimumExtent * (1.0f - skinWidth);
        sqrMinimumExtent = minimumExtent * minimumExtent;
    }

    void Update()
    {
        //have we moved more than our minimum extent? 
        Vector3 movementThisStep = myRigidbody.position - previousPosition;
        float movementSqrMagnitude = movementThisStep.sqrMagnitude;

        if (movementSqrMagnitude > sqrMinimumExtent)
        {
            float movementMagnitude = Mathf.Sqrt(movementSqrMagnitude);
            RaycastHit hitInfo;

            //check for obstructions we might have missed 
            if (Physics.Raycast(previousPosition, movementThisStep, out hitInfo, movementMagnitude, m_LMChar.value))
            {
                if (!hitInfo.collider)
                {
                    return;
                }

                m_Bullet.OnHit(hitInfo.collider.gameObject);
                m_Bullet.VFXEffect();

                // if (hitInfo.collider.isTrigger)
                // {
                //     hitInfo.collider.SendMessage("OnTriggerEnter", myCollider);
                // }

                // if (!hitInfo.collider.isTrigger)
                // {
                //     myRigidbody.position = hitInfo.point - (movementThisStep / movementMagnitude) * partialExtent;
                // }
            }
            else if (Physics.Raycast(previousPosition, movementThisStep, out hitInfo, movementMagnitude, m_LMMap.value))
            {
                if (!hitInfo.collider)
                {
                    return;
                }

                m_Bullet.OnHit();
                m_Bullet.VFXEffect();

                // if (hitInfo.collider.isTrigger)
                // {
                //     hitInfo.collider.SendMessage("OnTriggerEnter", myCollider);
                // }

                // if (!hitInfo.collider.isTrigger)
                // {
                //     myRigidbody.position = hitInfo.point - (movementThisStep / movementMagnitude) * partialExtent;
                // }
            }
        }

        previousPosition = myRigidbody.position;
    }
}