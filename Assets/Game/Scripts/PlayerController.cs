using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("---Components---")]
    public Transform tf_Onwer;

    [Header("---Movements---")]
    public float m_MoveSpd;
    public float m_GravityModifier;
    public float m_JumpPower;
    public float m_RunSpd;
    public CharacterController m_CharCon;
    public Transform tf_CamPoint;
    private Vector3 v3_MoveInput;
    public Animator anim_Owner;

    [Header("---Sensivity---")]
    public float m_MouseSen;

    [Header("---Jump---")]
    public bool m_CanJump;
    public bool m_DoubleJump;
    public Transform tf_GroundCheck;
    public LayerMask lm_Ground;

    [Header("---Fire---")]
    public GameObject m_Bullet;
    public GameObject m_Bullet2;
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
        float yStore = v3_MoveInput.y;

        Vector3 vertMove = tf_Onwer.forward * UltimateJoystick.GetVerticalAxisRaw("Movement");
        Vector3 horiMove = tf_Onwer.right * UltimateJoystick.GetHorizontalAxisRaw("Movement");
        // Vector3 vertMove = tf_Onwer.forward * Input.GetAxis("Vertical");
        // Vector3 horiMove = tf_Onwer.right * Input.GetAxis("Horizontal");

        v3_MoveInput = vertMove + horiMove;
        v3_MoveInput.Normalize();

        if (Input.GetKey(KeyCode.LeftShift))
        {
            Debug.Log("m_DoubleJump: " + m_DoubleJump);
            v3_MoveInput = v3_MoveInput * m_RunSpd;
        }
        else
        {
            v3_MoveInput = v3_MoveInput * m_MoveSpd;
        }

        v3_MoveInput.y = yStore;

        if (m_CharCon.isGrounded)
        {
            v3_MoveInput.y = Physics.gravity.y * m_GravityModifier * Time.deltaTime;
        }
        else
        {
            v3_MoveInput.y += Physics.gravity.y * m_GravityModifier * Time.deltaTime;
        }

        m_CanJump = Physics.OverlapSphere(tf_GroundCheck.position, 0.25f, lm_Ground).Length > 0;

        if (Input.GetKeyDown(KeyCode.Space) && m_CanJump)
        {
            v3_MoveInput.y = m_JumpPower;

        }


        m_CharCon.Move(v3_MoveInput * Time.deltaTime);

        // Vector2 mouseInput = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y")) * m_MouseSen;
        Vector2 mouseInput = new Vector2(UltimateJoystick.GetHorizontalAxisRaw("Aim"), UltimateJoystick.GetVerticalAxisRaw("Aim")) * m_MouseSen;

        tf_Onwer.rotation = Quaternion.Euler(tf_Onwer.rotation.eulerAngles.x, tf_Onwer.rotation.eulerAngles.y + mouseInput.x, tf_Onwer.rotation.eulerAngles.z);
        tf_CamPoint.rotation = Quaternion.Euler(tf_CamPoint.rotation.eulerAngles + new Vector3(-mouseInput.y * 1.3f, 0f, 0f));

        // if (Input.GetMouseButtonDown(0))
        // {
        //     FireBullet();
        // }

        if (m_ShotCd < m_MaxShotCd)
        {
            m_ShotCd += Time.deltaTime;
        }

        RaycastHit hit;
        if (Physics.Raycast(tf_CamPoint.position, tf_CamPoint.forward, out hit))
        {
            ITakenDamage iTaken = hit.transform.GetComponent<ITakenDamage>();
            if (iTaken != null && CanShot(hit.transform.position))
            {
                if (m_GunType == GunType.GUN_SNIPER)
                {
                    FireBullet();
                }
                else
                {
                    // StartCoroutine(FireBullet2());
                    FireBullet2();
                }
                // Debug.Log(hit.transform.name);
            }
        }

        anim_Owner.SetFloat("MoveSpd", v3_MoveInput.magnitude);
        anim_Owner.SetBool("OnGround", m_CanJump);
    }

    public bool CanShot(Vector3 _des)
    {
        return ((m_ShotCd >= m_MaxShotCd) && Helper.InRange(tf_Onwer.position, _des, 15f));
    }

    public void CheckDistance()
    {

    }

    public void FireBullet()
    {
        for (int i = 0; i < m_ShotBulet; i++)
        {
            Instantiate(m_Bullet, tf_FirePoint.position, tf_FirePoint.rotation);
        }

        m_ShotCd = 0f;
        m_ShotBulet = 1;
    }

    public void FireBullet2()
    {
        // for (int i = 0; i < m_ShotBulet; i++)
        // {
        //     Instantiate(m_Bullet2, tf_FirePoint.position, tf_FirePoint.rotation);
        //     yield return new WaitForSeconds(0.3f);
        //     Instantiate(m_Bullet2, tf_FirePoint.position, tf_FirePoint.rotation);
        //     yield return new WaitForSeconds(0.3f);
        //     Instantiate(m_Bullet2, tf_FirePoint.position, tf_FirePoint.rotation);
        //     yield return new WaitForSeconds(0.3f);
        // }
        for (int i = 0; i < m_ShotBulet; i++)
        {
            Instantiate(m_Bullet2, tf_FirePoint.position, tf_FirePoint.rotation);
        }
        m_ShotCd = 0f;
        m_ShotBulet = 3;
    }

    public void ChangeGun()
    {
        if (m_GunType == GunType.GUN_SNIPER)
        {
            m_GunType = GunType.GUN_CANON;
            m_CanonGun.SetActive(true);
            m_SniperGun.SetActive(false);
            m_MaxShotCd = 1.5f;
        }
        else
        {
            m_GunType = GunType.GUN_SNIPER;
            m_CanonGun.SetActive(false);
            m_SniperGun.SetActive(true);
            m_MaxShotCd = 3f;
        }
    }

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