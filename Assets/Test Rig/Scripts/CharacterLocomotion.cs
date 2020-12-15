using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterLocomotion : MonoBehaviour
{
    Animator anim_Onwer;
    Vector2 v2_Input;

    // Start is called before the first frame update
    void Start()
    {
        anim_Onwer = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        v2_Input.x = Input.GetAxis("Horizontal");
        v2_Input.y = Input.GetAxis("Vertical");

        anim_Onwer.SetFloat("InputX", v2_Input.x);
        anim_Onwer.SetFloat("InputY", v2_Input.y);
    }
}
