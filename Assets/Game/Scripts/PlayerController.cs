using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ControlFreak2;

public class PlayerController : MonoBehaviour
{
    [Header("---Components---")]
    public Transform tf_Owner;

    [Header("---Movements---")]
    public float m_MoveSpd;
    public float m_GravityModifier;
    public float m_RunSpd;
    public CharacterController m_CharCon;
    public Transform tf_CamPoint;
    public Transform tf_CamPoint2;
    private Vector3 v3_MoveInput;
    public Animator an_Owner;

    [Header("---Sensivity---")]
    public float m_MouseSen;

    [Header("---Jump---")]
    public bool m_CanJump;
    public float m_JumpPower;
    public Transform tf_GroundCheck;
    public LayerMask lm_Ground;

    [Header("---Shoot---")]
    public GameObject g_Bullet;
    public Transform tf_FirePoint;
    public float m_ShootCd;
    public float m_MaxShootCd;
    public int m_ShotBulet;

    [Header("---Test Gun---")]
    public GunType m_GunType;
    public Transform m_TestLook;

    private void OnEnable()
    {
        m_GunType = GunType.GUN_SNIPER;
        m_ShotBulet = 1;
        m_ShootCd = m_MaxShootCd + 1;
    }

    private void Update()
    {
        // OnRotating();

        OnRunning();

        OnJumping();

        // Vector2 mouseInput = new Vector2(CF2Input.GetAxis("Mouse X"), CF2Input.GetAxis("Mouse Y")) * m_MouseSen; //JOYSTICK

        Vector2 mouseInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")) * m_MouseSen; //MOUSE

        // Debug.Log(Input.GetAxis("Mouse Y"));

        if (mouseInput.magnitude > 0.1f)
        {
            OnRotating(mouseInput);
        }

        if (m_ShootCd < m_MaxShootCd)
        {
            m_ShootCd += Time.deltaTime;
        }

        if (CheckShoot())
        {
            OnShooting();
        }
    }

    public bool CanShot(Vector3 _des)
    {
        return ((m_ShootCd >= m_MaxShootCd) && Helper.InRange(tf_Owner.position, _des, 15f));
    }

    public bool CheckShoot()
    {
        RaycastHit hit;
        // Debug.DrawRay(tf_CamPoint2.position, tf_CamPoint2.forward * 20, Color.red);
        // Debug.DrawRay(tf_CamPoint.position, tf_CamPoint.forward * 20, Color.green);
        if (Physics.Raycast(tf_CamPoint.position, tf_CamPoint.forward, out hit))
        {
            ITakenDamage iTaken = hit.transform.GetComponent<ITakenDamage>();
            if (iTaken != null && CanShot(hit.transform.position))
            {
                // tf_Onwer.LookAt(hit.transform);
                // tf_CamPoint.LookAt(hit.transform);
                // tf_FirePoint.LookAt(hit.transform);
                return true;
            }

            return false;
        }

        return false;
    }

    public void OnShooting()
    {
        for (int i = 0; i < m_ShotBulet; i++)
        {
            Instantiate(g_Bullet, tf_FirePoint.position, tf_FirePoint.rotation);
        }

        m_ShootCd = 0f;
        m_ShotBulet = 1;
    }

    public void OnRotating(Vector2 _input)
    {
        tf_Owner.rotation = Quaternion.Euler(tf_Owner.rotation.eulerAngles.x, tf_Owner.rotation.eulerAngles.y + _input.x, tf_Owner.rotation.eulerAngles.z);

        float limit = tf_CamPoint.rotation.eulerAngles.x - _input.y;
        limit = Helper.ClampAngle(limit, -40, 60);
        tf_CamPoint.rotation = Quaternion.Euler(limit, tf_CamPoint.rotation.eulerAngles.y, tf_CamPoint.rotation.eulerAngles.z);
    }

    public void OnRotating()
    {
        tf_Owner.LookAt(m_TestLook);
        tf_Owner.rotation = Quaternion.Euler(0f, tf_Owner.rotation.eulerAngles.y, 0f);

        tf_CamPoint.LookAt(m_TestLook);
        Debug.DrawRay(tf_CamPoint.position, tf_CamPoint.forward * 20, Color.green);
    }

    public void OnRunning()
    {
        float yStore = v3_MoveInput.y;

        // Vector3 horiMove = tf_Onwer.right * CF2Input.GetAxis("Joystick Move X"); //JOYSTICK
        // Vector3 vertMove = tf_Onwer.forward * CF2Input.GetAxis("Joystick Move Y"); //JOYSTICK


        Vector3 vertMove = tf_Owner.forward * Input.GetAxis("Vertical"); //MOUSE
        Vector3 horiMove = tf_Owner.right * Input.GetAxis("Horizontal"); //MOUSE

        v3_MoveInput = vertMove + horiMove;
        v3_MoveInput.Normalize();

        // if (Input.GetKey(KeyCode.LeftShift))
        // {
        //     v3_MoveInput = v3_MoveInput * m_RunSpd;
        // }
        // else
        // {
        //     v3_MoveInput = v3_MoveInput * m_MoveSpd;
        // }

        v3_MoveInput.y = yStore;

        OnGroundHandle();

        m_CharCon.Move(v3_MoveInput * 15f * Time.deltaTime); //MOUSE 
        // m_CharCon.Move(v3_MoveInput * Time.deltaTime); //JOYSTICK
    }

    public void OnGroundHandle()
    {
        if (m_CharCon.isGrounded)
        {
            v3_MoveInput.y = Physics.gravity.y * m_GravityModifier * Time.deltaTime;
        }
        else
        {
            v3_MoveInput.y += Physics.gravity.y * m_GravityModifier * Time.deltaTime;
        }
    }

    // public 

    public void OnJumping()
    {
        // m_CanJump = Physics.OverlapSphere(tf_GroundCheck.position, 0.25f, lm_Ground).Length > 0;

        // if (Input.GetKeyDown(KeyCode.Space) && m_CanJump)
        // {
        //     v3_MoveInput.y = m_JumpPower;
        // }
    }

    // public void Shoot2()
    // {
    //     // for (int i = 0; i < m_ShotBulet; i++)
    //     // {
    //     //     Instantiate(m_Bullet2, tf_FirePoint.position, tf_FirePoint.rotation);
    //     //     yield return new WaitForSeconds(0.3f);
    //     //     Instantiate(m_Bullet2, tf_FirePoint.position, tf_FirePoint.rotation);
    //     //     yield return new WaitForSeconds(0.3f);
    //     //     Instantiate(m_Bullet2, tf_FirePoint.position, tf_FirePoint.rotation);
    //     //     yield return new WaitForSeconds(0.3f);
    //     // }
    //     for (int i = 0; i < m_ShotBulet; i++)
    //     {
    //         Instantiate(g_Bullet2, tf_FirePoint.position, tf_FirePoint.rotation);
    //     }
    //     m_ShotCd = 0f;
    //     m_ShotBulet = 3;
    // }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Player trigger!!!");
    }
}

public enum GunType
{
    GUN_SNIPER = 0,
    GUN_CANON = 1
}