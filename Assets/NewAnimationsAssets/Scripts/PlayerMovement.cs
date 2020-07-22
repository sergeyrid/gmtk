using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController cc;
    private float fall_speed = 1f;
    private float velocity;
    GameProperties game_properties;

    void Start()
    {
        cc = GetComponent<CharacterController>();
        game_properties = GameObject.Find("GameManager").GetComponent<GameProperties>();
    }
    void FixedUpdate()
    {
        cc.Move(Input.GetAxisRaw("Horizontal") * Vector3.right * game_properties.potions_level * 0.75f);
        cc.Move(Vector3.down * game_properties.potions_level * Time.deltaTime * 10f);
        Jumping();
    }
    void Jumping()
    {
        if (cc.isGrounded)
        {
            velocity = -fall_speed * Time.deltaTime;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                velocity = game_properties.potions_level * 0.5f;
            }
        }
        else
        {
            velocity -= fall_speed * Time.deltaTime;
        }
        cc.Move(new Vector3(0f, velocity, 0f) * game_properties.potions_level);
    }
}
