using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controls : MonoBehaviour
{
    float speed;
    float acceleration;
    float slipperiness;
    float jumpHeight;
    float airControl;
    float hp;
    float strength;
    float attackSpeed;
    float attackReach;
    float dizziness;
    Characteristics characteristics;
    Rigidbody2D body;
    int direction = 1;
    float height;
    bool onGround = true;
    float movement;
    int jumpCurrentCooldown = 0;
    public int jumpCooldown;
    int enemyLayer = 8;

    void Start()
    {
        characteristics = GetComponent<Characteristics>();
        body = GetComponent<Rigidbody2D>();
        height = GetComponent<Collider2D>().bounds.size.y;
    }

    void Update() {
        speed = characteristics.speed;
        acceleration = characteristics.acceleration;
        jumpHeight = characteristics.jumpHeight;
        slipperiness = characteristics.slipperiness;
        airControl = characteristics.airControl;
        strength = characteristics.strength;
        attackSpeed = characteristics.attackSpeed;
        attackReach = characteristics.attackReach;
        hp = characteristics.hp;
        dizziness = characteristics.dizziness;
        ProcessInput();
    }

    void FixedUpdate()
    {
        if (jumpCurrentCooldown > 0)
        {
            --jumpCurrentCooldown;
        }
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
        if (Input.GetButton("Attack"))
        {
            Attack();
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
        if (movement == 0 && onGround)
        {
            body.velocity = new Vector2(body.velocity.x * slipperiness, body.velocity.y);
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
        if (onGround && jumpCurrentCooldown == 0)
        {
            body.AddForce(new Vector2(0, jumpHeight));
            jumpCurrentCooldown = jumpCooldown;
        }
    }

    void Attack()
    {
        RaycastHit2D []enemiesHit = new RaycastHit2D[20];
        ContactFilter2D enemyFilter = new ContactFilter2D();
        enemyFilter.layerMask = 1 << enemyLayer;
        Physics2D.Raycast(transform.position, Vector2.right * direction, enemyFilter, enemiesHit);
        foreach (RaycastHit2D enemy in enemiesHit)
        {
            if (enemy.collider != null)
            {
                Debug.Log("BANG");
            }
        }
    }
}
