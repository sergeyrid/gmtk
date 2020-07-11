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
    float dizziness;
    float attackReach;
    int attackCooldown;
    Characteristics characteristics;
    Rigidbody2D body;
    int direction = 1;
    float height;
    float width;
    bool onGround = true;
    float movement;
    int attackCurrentCooldown = 0;
    int jumpCurrentCooldown = 0;

    public int jumpCooldown;
    public int enemyLayer = 8;

    void Start()
    {
        characteristics = GetComponent<Characteristics>();
        body = GetComponent<Rigidbody2D>();
        height = GetComponent<Collider2D>().bounds.size.y;
        width = GetComponent<Collider2D>().bounds.size.x;
    }

    void Update() {
        speed = characteristics.speed;
        acceleration = characteristics.acceleration;
        jumpHeight = characteristics.jumpHeight;
        slipperiness = characteristics.slipperiness;
        airControl = characteristics.airControl;
        strength = characteristics.strength;
        hp = characteristics.hp;
        dizziness = characteristics.dizziness;
        attackReach = characteristics.attackReach;
        attackCooldown = characteristics.attackCooldown;
        if (body.velocity.x > 0)
        {
            direction = 1;
        }
        else if (body.velocity.x < 0)
        {
            direction = -1;
        }
        ProcessInput();
    }

    void FixedUpdate()
    {
        if (jumpCurrentCooldown > 0)
        {
            --jumpCurrentCooldown;
        }
        if (attackCurrentCooldown > 0)
        {
            --attackCurrentCooldown;
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
        float delta = 1;
        Vector3 pos = transform.position;
        Debug.DrawLine(pos, new Vector2(pos.x + (-Vector2.up.x * (height / 2 + delta)), pos.y + (-Vector2.up.y * (height / 2 + delta))), new Color(255, 0, 0));
        RaycastHit2D hit = Physics2D.Raycast(pos, -Vector2.up, height / 2 + delta);
        RaycastHit2D hitLeft = Physics2D.Raycast(new Vector2(pos.x - width / 2, pos.y),
                                                 -Vector2.up, height / 2 + delta);
        RaycastHit2D hitRight = Physics2D.Raycast(new Vector2(pos.x + width / 2, pos.y),
                                                  -Vector2.up, height / 2 + delta);
        Debug.Log(hit.collider);
        Debug.Log(hitLeft.collider);
        Debug.Log(hitRight.collider);
        if (hit.collider != null || hitLeft.collider != null || hitRight.collider != null)
        {
            Debug.Log("Nice");
            onGround = true;
        }
        else
        {
            Debug.Log(":(");
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
        if (attackCurrentCooldown > 0)
        {
            return ;
        }
        RaycastHit2D []enemiesHit = Physics2D.RaycastAll(transform.position, Vector2.right * direction, attackReach, 1<<enemyLayer);
        foreach (RaycastHit2D enemy in enemiesHit)
        {
            Debug.Log("BANG!!!");
        }
        attackCurrentCooldown = attackCooldown;
    }
}
