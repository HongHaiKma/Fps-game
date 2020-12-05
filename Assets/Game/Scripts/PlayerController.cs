using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("---Components---")]
    public Transform m_Tf;

    [Header("---Movements---")]
    public float m_MoveSpd;
    public float m_GravityModifier;
    public float m_JumpPower;
    public CharacterController m_CharCon;
    public Transform m_CamPointTf;
    private Vector3 m_MoveInput;

    [Header("---Sensivity---")]
    public float m_MouseSen;

    [Header("---Jump---")]
    public bool m_CanJump;
    public Transform m_GroundCheckTf;
    public LayerMask m_GroundLM;

    private void Update()
    {
        float yStore = m_MoveInput.y;

        Vector3 vertMove = m_Tf.forward * Input.GetAxis("Vertical");
        Vector3 horiMove = m_Tf.right * Input.GetAxis("Horizontal");

        m_MoveInput = vertMove + horiMove;
        m_MoveInput.Normalize();
        m_MoveInput = m_MoveInput * m_MoveSpd * Time.deltaTime;

        m_MoveInput.y = yStore;
        // m_MoveInput.y += Physics.gravity.y * m_GravityModifier * Time.deltaTime;

        if (m_CharCon.isGrounded)
        {
            m_MoveInput.y = Physics.gravity.y * m_GravityModifier * Time.deltaTime;
        }
        else
        {
            m_MoveInput.y += Physics.gravity.y * m_GravityModifier * Time.deltaTime;
        }

        m_CanJump = Physics.OverlapSphere(m_GroundCheckTf.position, 0.25f, m_GroundLM).Length > 0;

        // if (m_CharCon.isGrounded)
        // {
        if (Input.GetKeyDown(KeyCode.Space) && m_CanJump)
        {
            m_MoveInput.y = m_JumpPower;
        }
        // }

        m_CharCon.Move(m_MoveInput);

        Vector2 mouseInput = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y")) * m_MouseSen;

        m_Tf.rotation = Quaternion.Euler(m_Tf.rotation.eulerAngles.x, m_Tf.rotation.eulerAngles.y + mouseInput.x, m_Tf.rotation.eulerAngles.z);
        m_CamPointTf.rotation = Quaternion.Euler(m_CamPointTf.rotation.eulerAngles + new Vector3(-mouseInput.y, 0f, 0f));
    }
}