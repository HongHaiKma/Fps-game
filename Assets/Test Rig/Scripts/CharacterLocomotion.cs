using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using ControlFreak2;

public class CharacterLocomotion : MonoBehaviour
{
    Animator anim_Onwer;
    Vector2 v2_Input;

    [Header("---Animation Rigging---")]
    public Rig r_Aiming;

    // Start is called before the first frame update
    void Start()
    {
        anim_Onwer = GetComponent<Animator>();
        // r_Aiming.weight = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        // v2_Input.x = Input.GetAxis("Horizontal");
        // v2_Input.y = Input.GetAxis("Vertical");

        v2_Input.x = CF2Input.GetAxis("Joystick Move X");
        v2_Input.y = CF2Input.GetAxis("Joystick Move Y");

        // Debug.Log(CF2Input.GetAxis("Mouse X"));

        // anim_Onwer.SetFloat("InputX", v2_Input.x);
        // anim_Onwer.SetFloat("InputY", v2_Input.y);

        anim_Onwer.SetFloat("InputX", v2_Input.x);
        anim_Onwer.SetFloat("InputY", v2_Input.y);
    }
}
