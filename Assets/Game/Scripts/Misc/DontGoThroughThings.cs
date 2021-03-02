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
    public bool m_Collided;
    public Collider col_Onwer;

    //initialize values 
    // void Start()
    // {
    //     previousPosition = myRigidbody.position;
    //     minimumExtent = Mathf.Min(Mathf.Min(myCollider.bounds.extents.x, myCollider.bounds.extents.y), myCollider.bounds.extents.z);
    //     partialExtent = minimumExtent * (1.0f - skinWidth);
    //     sqrMinimumExtent = minimumExtent * minimumExtent;
    // }

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

            bool collideChar = Physics.Raycast(previousPosition, movementThisStep, out hitInfo, movementMagnitude, m_LMChar.value);
            bool collideMap = Physics.Raycast(previousPosition, movementThisStep, out hitInfo, movementMagnitude, m_LMChar.value);

            if (collideChar && !m_Collided)
            {
                m_Collided = true;
                col_Onwer.enabled = false;

                if (!hitInfo.collider)
                {
                    return;
                }

                m_Bullet.v3_CollisionPoint = hitInfo.point;
                m_Bullet.OnHit(hitInfo.collider.gameObject);
            }
            else if (collideMap && !m_Collided)
            {
                m_Collided = true;
                col_Onwer.enabled = false;

                if (!hitInfo.collider)
                {
                    return;
                }

                m_Bullet.v3_CollisionPoint = hitInfo.point;
                m_Bullet.OnHit();
            }
        }

        previousPosition = myRigidbody.position;
    }
}