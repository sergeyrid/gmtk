using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    Characteristics stats;
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
        stats = GetComponent<Characteristics>();
        body = GetComponent<Rigidbody2D>();
        height = GetComponent<Collider2D>().bounds.size.y;
        width = GetComponent<Collider2D>().bounds.size.x;
    }

    void Update() {
        speed = stats.speed;
        acceleration = stats.acceleration;
        jumpHeight = stats.jumpHeight;
        slipperiness = stats.slipperiness;
        airControl = stats.airControl;
        strength = stats.strength;
        hp = stats.hp;
        dizziness = stats.dizziness;
        attackReach = stats.attackReach;
        attackCooldown = stats.attackCooldown;
        if (body.velocity.x > 0)
        {
            direction = 1;
        }
        else if (body.velocity.x < 0)
        {
            direction = -1;
        }
        ProcessInput();
        if (hp == 0)
        {
            Death();
        }
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
        RaycastHit2D hit = Physics2D.BoxCast(new Vector2(pos.x, pos.y - height / 2),
                                             new Vector2(width / 2, delta), 0, -Vector2.up, ~(1<<2));
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

    void Death()
    {
        // animation
        SceneManager.LoadScene("SampleScene");
    }
    public void TakeDamage(float damage)
    {
        // animation;
        stats.hp -= damage;
    }
}
