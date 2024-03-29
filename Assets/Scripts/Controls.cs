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
    float maxHp;
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
    bool dead = false;
    float movement;
    int attackCurrentCooldown = 0;
    int jumpCurrentCooldown = 0;
    public int jumpCooldown;
    public Animator animator;
    public float hp;
    public float movementAudio = 0.5f;
    public int enemyLayer = 8;
    public int playerLayer = 10;
    public int defaultLayer = 0;
    public int physicsLayer = 9;
    public int ignoreLayer = 2;
    public GameObject audioManager;
    AudioSource source;

    void Start()
    {
        source = GetComponent<AudioSource>();
        stats = GetComponent<Characteristics>();
        body = GetComponent<Rigidbody2D>();
        height = GetComponent<Collider2D>().bounds.size.y;
        width = GetComponent<Collider2D>().bounds.size.x;
        animator = gameObject.GetComponentInChildren<Animator>();
        hp = stats.maxHp;
    }

    void Update() {
        if (dead)
        {
            body.velocity = new Vector2(0, 0);
            return ;
        }
        speed = stats.speed;
        acceleration = stats.acceleration;
        jumpHeight = stats.jumpHeight;
        slipperiness = stats.slipperiness;
        airControl = stats.airControl;
        strength = stats.strength;
        maxHp = stats.maxHp;
        dizziness = stats.dizziness;
        attackReach = stats.attackReach;
        attackCooldown = stats.attackCooldown;
        if (body.velocity.x > 0.1 && direction == -1)
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
            int i = 0;
            foreach (HingeJoint2D child in allChildrenJoint)
            {
                ++i;
                if (i % 2 == 0)
                {
                    continue;
                }
                Vector3 anc = child.anchor;
                child.anchor = new Vector2(-anc.x, anc.y);
                anc = child.connectedAnchor;
                child.connectedAnchor = new Vector2(-anc.x, anc.y);
            }
        }
        else if (body.velocity.x < -0.1 && direction >= 0)
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
            int i = 0;
            foreach (HingeJoint2D child in allChildrenJoint)
            {
                ++i;
                if (i % 2 == 0)
                {
                    continue;
                }
                Vector3 anc = child.anchor;
                child.anchor = new Vector2(-anc.x, anc.y);
                anc = child.connectedAnchor;
                child.connectedAnchor = new Vector2(-anc.x, anc.y);
            }
        }
        ProcessInput();
        if (hp <= 0)
        {
            Death();
        }
    }

    void FixedUpdate()
    {
        if (dead)
        {
            return ;
        }
        CheckIfOnGround();
        HorizontalMovement();
        if (jumpCurrentCooldown > 0)
        {
            --jumpCurrentCooldown;
        }
        if (attackCurrentCooldown > 0)
        {
            --attackCurrentCooldown;
        }
        if(Mathf.Abs(body.velocity.x) > 1 && onGround) //! magic constant
        {
            animator.SetBool("IsRunning", true);
        }
        else
        {
            animator.SetBool("IsRunning", false);
        }
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
        if (Input.GetButton("Reset"))
        {
            Death();
        }
    }

    void CheckIfOnGround()
    {
        float delta = 2;
        Vector3 pos = transform.position;
        RaycastHit2D hit = Physics2D.BoxCast(new Vector2(pos.x, pos.y - height / 2),
                                             new Vector2(width / 2, delta),
                                             0, -Vector2.up, delta,
                                             ~((1 << playerLayer) | (1 << enemyLayer) | (1 << ignoreLayer)));
        if (hit.collider != null)
        {
            onGround = true;
            if (hit.collider.transform.gameObject.layer == physicsLayer &&
                Mathf.Abs(body.velocity.x) > movementAudio &&
                !source.isPlaying)
            {
                AudioClip audio = audioManager.GetComponent<GetAudio>().woodStep;
                source.PlayOneShot(audio);
            }
            else if (hit.collider.transform.gameObject.layer == defaultLayer &&
                     Mathf.Abs(body.velocity.x) > movementAudio &&
                     !source.isPlaying)
            {
                AudioClip audio = audioManager.GetComponent<GetAudio>().dirtStep;
                source.PlayOneShot(audio);
            }
            animator.SetBool("IsInAir", false);
        }
        else
        {
            onGround = false;
            animator.SetBool("IsInAir", true);
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
            animator.SetTrigger("Jump");
        }
    }

    void Attack()
    {
        if (attackCurrentCooldown > 0)
        {
            return ;
        }
        RaycastHit2D []enemiesHit = Physics2D.RaycastAll(transform.position, Vector2.right * direction,
                                                         attackReach, 1<<enemyLayer);
        AudioClip audio = audioManager.GetComponent<GetAudio>().playerAttack;
        source.PlayOneShot(audio);
        animator.SetTrigger("Hit");
        foreach (RaycastHit2D enemy in enemiesHit)
        {
            EvilAi cont = enemy.transform.gameObject.GetComponent<EvilAi>();
            if (cont != null)
            {
                cont.TakeDamage(strength);
            }
            else
            {
                EvilFly cnt = enemy.transform.gameObject.GetComponent<EvilFly>();
                cnt.TakeDamage(strength);
            }
        }
        attackCurrentCooldown = attackCooldown;
    }

    void LoadLevel()
    {
        Scene activeScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(activeScene.name);
    }

    void Death()
    {
        AudioClip audio = audioManager.GetComponent<GetAudio>().playerDeath;
        source.PlayOneShot(audio);
        dead = true;
        animator.SetTrigger("Death");
        Invoke("LoadLevel", 1.4f);
    }
    
    public void TakeDamage(float damage)
    {
        if (!dead)
        {
            animator.SetTrigger("TakeDamage");
            AudioClip audio = audioManager.GetComponent<GetAudio>().playerDamage;
            source.PlayOneShot(audio);
        }
        hp -= damage;
        if (hp < 0)
        {
            hp = 0;
        }
    }
}
