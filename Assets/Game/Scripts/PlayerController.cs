using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("---Components---")]
    public Transform m_Tf;

    [Header("---Movements---")]
    public float m_MoveSpd;
    public CharacterController m_CharCon;
    public Transform m_CamPointTf;
    private Vector3 m_MoveInput;

    [Header("---Sensivity---")]
    public float m_MouseSen;
    // public bool m_InvertX;
    // public bool m_InvertY;

    private void Update()
    {
        Vector3 vertMove = m_Tf.forward * Input.GetAxis("Vertical");
        Vector3 horiMove = m_Tf.right * Input.GetAxis("Horizontal");

        m_MoveInput = vertMove + horiMove;
        m_MoveInput.Normalize();
        m_MoveInput = m_MoveInput * m_MoveSpd * Time.deltaTime;

        m_CharCon.Move(m_MoveInput);

        Vector2 mouseInput = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y")) * m_MouseSen;

        m_Tf.rotation = Quaternion.Euler(m_Tf.rotation.eulerAngles.x, m_Tf.rotation.eulerAngles.y + mouseInput.x, m_Tf.rotation.eulerAngles.z);
        m_CamPointTf.rotation = Quaternion.Euler(m_CamPointTf.rotation.eulerAngles + new Vector3(-mouseInput.y, 0f, 0f));
    }
}