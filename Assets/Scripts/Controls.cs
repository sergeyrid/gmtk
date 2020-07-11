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
    Characteristics characteristics;
    Rigidbody2D body;
    float height;
    bool onGround = true;
    float movement;

    void Start()
    {
        characteristics = GetComponent<Characteristics>();
        speed = characteristics.speed;
        acceleration = characteristics.acceleration;
        jumpHeight = characteristics.jumpHeight;
        groundControl = characteristics.groundControl;
        airControl = characteristics.airControl;
        body = GetComponent<Rigidbody2D>();
        height = GetComponent<Collider2D>().bounds.size.y;
    }

    void Update() {
        ProcessInput();
    }

    void FixedUpdate()
    {
        CheckIfOnGround();
        HorizontalMovement();
    }

    void ProcessInput()
    {
        movement = Input.GetAxisRaw("Horizontal") * speed * acceleration;
        if (Input.GetButton("Jump"))
        {
            Jump();
        }
    }

    void CheckIfOnGround()
    {
        float delta = 0.1f;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.up, height / 2 + delta);
        if (hit.collider != null)
        {
            onGround = true;
        }
        else
        {
            onGround = false;
        }
    }

    void HorizontalMovement()
    {
        if (!onGround)
        {
            movement *= airControl;
        }
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

    void Jump()
    {
        if (onGround)
        {
            body.AddForce(new Vector2(0, jumpHeight));
        }
    }
}
