﻿using System.Collections;
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
        Debug.Log(width);
        Debug.Log(height);
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
        if (body.velocity.x > 0 && direction == -1)
        {
            Vector3 pos = transform.position;
            transform.position = new Vector3(pos.x, pos.y, -pos.z);
            direction = 1;
            transform.Rotate(0, 180, 0);
            Transform[] allChildren = GetComponentsInChildren<Transform>();
            foreach (Transform child in allChildren)
            {
                Vector3 p = child.localPosition;
                child.localPosition = new Vector3(p.x, p.y, -p.z);
            }
            HingeJoint2D[] allChildrenJoint = GetComponentsInChildren<HingeJoint2D>();
            foreach (HingeJoint2D child in allChildrenJoint)
            {
                Vector3 anc = child.anchor;
                child.anchor = new Vector2(-anc.x, anc.y);
                anc = child.connectedAnchor;
                child.connectedAnchor = new Vector2(-anc.x, anc.y);
            }
        }
        else if (body.velocity.x < 0 && direction == 1)
        {
            Vector3 pos = transform.position;
            transform.position = new Vector3(pos.x, pos.y, -pos.z);
            direction = -1;
            transform.Rotate(0, 180, 0);
            Transform[] allChildrenTransform = GetComponentsInChildren<Transform>();
            foreach (Transform child in allChildrenTransform)
            {
                Vector3 p = child.localPosition;
                child.localPosition = new Vector3(p.x, p.y, -p.z);
            }
            HingeJoint2D[] allChildrenJoint = GetComponentsInChildren<HingeJoint2D>();
            foreach (HingeJoint2D child in allChildrenJoint)
            {
                Vector3 anc = child.anchor;
                child.anchor = new Vector2(-anc.x, anc.y);
                anc = child.connectedAnchor;
                child.connectedAnchor = new Vector2(-anc.x, anc.y);
            }
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
        Debug.Log(pos);
        if (hit.collider != null)
        {
            // Debug.Log("Nice");
            onGround = true;
        }
        else
        {
            // Debug.Log(":(");
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
