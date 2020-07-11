using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controls : MonoBehaviour
{
    float speed;
    float acceleration;
    float groundControl;
    float jumpHeight;
    float airControl;
    float hp;
    float strength;
    float attackSpeed;
    float dizziness;

    Rigidbody2D body;
    Characteristics characteristics;

    void Start()
    {
        characteristics = GetComponent<Characteristics>();
        speed = characteristics.speed;
        acceleration = characteristics.acceleration;
        jumpHeight = characteristics.jumpHeight;
        groundControl = characteristics.groundControl;
        airControl = characteristics.airControl;
        hp = characteristics.hp;
        strength = characteristics.strength;
        attackSpeed = characteristics.attackSpeed;
        dizziness = characteristics.dizziness;
        body = GetComponent<Rigidbody2D>();
        Debug.Log(characteristics.speed);
    }

    void FixedUpdate()
    {
        float deltaSpeed = speed * acceleration;
        float movement = Input.GetAxisRaw("Horizontal") * deltaSpeed;
        if (movement == 0)
        {
            body.velocity = new Vector2(body.velocity.x * groundControl, body.velocity.y);
        }
        else
        {
            body.AddForce(new Vector2(movement, 0));
        }
        if (Mathf.Abs(body.velocity.x) > speed)
        {
            body.velocity = new Vector2(speed * Mathf.Sign(body.velocity.x), body.velocity.y);
        }
    }
}
