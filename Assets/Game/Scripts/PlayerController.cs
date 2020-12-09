using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ControlFreak2;

public class PlayerController : MonoBehaviour
{
    [Header("---Components---")]
    public Transform tf_Onwer;

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

    [Header("---Fire---")]
    public GameObject g_Bullet;
    public Transform tf_FirePoint;
    public float m_ShotCd;
    public float m_MaxShotCd;
    public int m_ShotBulet;

    [Header("---Test Gun---")]
    public GameObject m_SniperGun;
    public GameObject m_CanonGun;
    public GunType m_GunType;

    private void OnEnable()
    {
        m_GunType = GunType.GUN_SNIPER;
        m_ShotBulet = 1;
        m_ShotCd = m_MaxShotCd + 1;
    }

    private void Update()
    {
        OnRunning();

        OnJumping();

        // Vector2 mouseInput = new Vector2(CF2Input.GetAxis("Mouse X"), CF2Input.GetAxis("Mouse Y")) * m_MouseSen; //JOYSTICK
        Vector2 mouseInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")) * m_MouseSen; //MOUSE

        if (mouseInput.magnitude > 0.1f)
        {
            OnRotating(mouseInput);
        }

        if (m_ShotCd < m_MaxShotCd)
        {
            m_ShotCd += Time.deltaTime;
        }

        if (CheckShoot())
        {
            OnShooting();
        }

        // an_Owner.SetFloat("MoveSpd", v3_MoveInput.magnitude);
        // an_Owner.SetBool("OnGround", m_CanJump);
    }

    public bool CanShot(Vector3 _des)
    {
        return ((m_ShotCd >= m_MaxShotCd) && Helper.InRange(tf_Onwer.position, _des, 15f));
    }

    public bool CheckShoot()
    {
        RaycastHit hit;
        // Debug.DrawRay(tf_Onwer.position, tf_Onwer.forward, Color.red);
        Debug.DrawRay(tf_CamPoint2.position, tf_CamPoint2.forward * 20, Color.red);
        Debug.DrawRay(tf_CamPoint.position, tf_CamPoint.forward * 20, Color.green);
        // if (Physics.Raycast(tf_CamPoint.position, tf_CamPoint.forward, out hit))
        if (Physics.Raycast(tf_CamPoint2.position, tf_CamPoint2.forward, out hit))
        {
            ITakenDamage iTaken = hit.transform.GetComponent<ITakenDamage>();
            if (iTaken != null && CanShot(hit.transform.position))
            {
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
            // BUllet
            Instantiate(g_Bullet, tf_FirePoint.position, tf_FirePoint.rotation);
        }

        m_ShotCd = 0f;
        m_ShotBulet = 1;
    }

    public void OnRotating(Vector2 _input)
    {
        tf_Onwer.rotation = Quaternion.Euler(tf_Onwer.rotation.eulerAngles.x, tf_Onwer.rotation.eulerAngles.y + _input.x, tf_Onwer.rotation.eulerAngles.z);
        tf_CamPoint.rotation = Quaternion.Euler(tf_CamPoint.rotation.eulerAngles + new Vector3(-_input.y * 1.3f, 0f, 0f));
    }

    public void OnRunning()
    {
        float yStore = v3_MoveInput.y;

        // Vector3 horiMove = tf_Onwer.right * CF2Input.GetAxis("Joystick Move X"); //JOYSTICK
        // Vector3 vertMove = tf_Onwer.forward * CF2Input.GetAxis("Joystick Move Y"); //JOYSTICK


        Vector3 vertMove = tf_Onwer.forward * Input.GetAxis("Vertical"); //MOUSE
        Vector3 horiMove = tf_Onwer.right * Input.GetAxis("Horizontal"); //MOUSE

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