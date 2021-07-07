using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DontGoThroughThings : MonoBehaviour
{
    // Careful when setting this to true - it might cause double
    // events to be fired - but it won't pass through the trigger
    public bool sendTriggerMessage = false;

    public LayerMask lm_BodyPart; //make sure we aren't in this layer 
    public LayerMask lm_Map; //make sure we aren't in this layer 
    public float skinWidth = 0.1f; //probably doesn't need to be changed 

    private float minimumExtent;
    private float partialExtent;
    private float sqrMinimumExtent;
    private Vector3 previousPosition;
    public Rigidbody myRigidbody;
    public Collider myCollider;

    public Bullet m_Bullet;
    public bool m_Collided;
    public Collider col_Onwer;

    private void OnEnable()
    {
        previousPosition = myRigidbody.position;
        minimumExtent = Mathf.Min(Mathf.Min(myCollider.bounds.extents.x, myCollider.bounds.extents.y), myCollider.bounds.extents.z);
        partialExtent = minimumExtent * (1.0f - skinWidth);
        sqrMinimumExtent = minimumExtent * minimumExtent;

        m_Collided = false;
        // col_Onwer.enabled = true;
    }

    private void OnDisable()
    {
        m_Collided = true;
        // col_Onwer.enabled = false;
    }

    void FixedUpdate()
    {
        //have we moved more than our minimum extent? 
        Vector3 movementThisStep = myRigidbody.position - previousPosition;
        float movementSqrMagnitude = movementThisStep.sqrMagnitude;

        if (movementSqrMagnitude > sqrMinimumExtent)
        {
            float movementMagnitude = Mathf.Sqrt(movementSqrMagnitude);
            RaycastHit hitInfo;

            if (Physics.Raycast(previousPosition, movementThisStep, out hitInfo, movementMagnitude, lm_Map.value) && !m_Collided)
            {
                // Helper.DebugLog("Name: " + hitInfo.collider.name);
                m_Collided = true;
                col_Onwer.enabled = false;

                if (!hitInfo.collider)
                {
                    return;
                }

                m_Bullet.v3_CollisionPoint = hitInfo.point;
                m_Bullet.OnHit(hitInfo.collider.gameObject);

            }
            else if (Physics.Raycast(previousPosition, movementThisStep, out hitInfo, movementMagnitude, lm_BodyPart.value) && !m_Collided)
            {
                // Helper.DebugLog("Name: " + hitInfo.collider.name);
                m_Collided = true;
                col_Onwer.enabled = false;

                if (!hitInfo.collider)
                {
                    return;
                }

                m_Bullet.v3_CollisionPoint = hitInfo.point;
                m_Bullet.OnHit(hitInfo.collider.gameObject);
            }

            // if (Physics.Raycast(previousPosition, movementThisStep, out hitInfo, movementMagnitude, lm_BodyPart.value) && !m_Collided)
            // {
            //     Helper.DebugLog("Name: " + hitInfo.collider.name);
            //     m_Collided = true;
            //     col_Onwer.enabled = false;

            //     if (!hitInfo.collider)
            //     {
            //         return;
            //     }

            //     m_Bullet.v3_CollisionPoint = hitInfo.point;
            //     m_Bullet.OnHit(hitInfo.collider.gameObject);
            // }
            // else if (Physics.Raycast(previousPosition, movementThisStep, out hitInfo, movementMagnitude, lm_Map.value) && !m_Collided)
            // {
            //     Helper.DebugLog("Name: " + hitInfo.collider.name);
            //     m_Collided = true;
            //     col_Onwer.enabled = false;

            //     if (!hitInfo.collider)
            //     {
            //         return;
            //     }

            //     m_Bullet.v3_CollisionPoint = hitInfo.point;
            //     m_Bullet.OnHit(hitInfo.collider.gameObject);

            // }
        }

        previousPosition = myRigidbody.position;
    }
}