using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Animator anim_1;
    public Animator anim_2;
    public Animator anim_3;
    public Animator anim_4;
    public Animator anim_5;
    
    private Rigidbody rb;
    private CharacterController cc;
    GameProperties game_properties;

    void Start()
    {
        cc = GetComponent<CharacterController>();
        game_properties = GameObject.Find("GameManager").GetComponent<GameProperties>();
    }
    void FixedUpdate()
    {
        Jumping();


    }
    void Jumping()
    {
        if (cc.isGrounded)
        {

        }
        
    }
}
