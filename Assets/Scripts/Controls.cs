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
    float movement;
    int attackCurrentCooldown = 0;
    int jumpCurrentCooldown = 0;
    public int jumpCooldown;
    public Animator animator;
    public float hp;
    public int enemyLayer = 8;
    public int playerLayer = 10;
    public int ignoreLayer = 2;

    void Start()
    {
        stats = GetComponent<Characteristics>();
        body = GetComponent<Rigidbody2D>();
        height = GetComponent<Collider2D>().bounds.size.y;
        width = GetComponent<Collider2D>().bounds.size.x;
        animator = gameObject.GetComponent<Animator>();
        hp = stats.maxHp;
    }

    void Update() {
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
        bool currentlyJumping = false;

        if (jumpCurrentCooldown > 0)
        {
            currentlyJumping = true;
            --jumpCurrentCooldown;
        }
        if (attackCurrentCooldown > 0)
        {
            --attackCurrentCooldown;
        }
        CheckIfOnGround();
        HorizontalMovement();

        if (jumpCurrentCooldown <= 0)
            currentlyJumping = false;

        if(!currentlyJumping)
        {
            animator.SetBool("jumping", false);
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
        }
        else
        {
            onGround = false;
        }
    }

    void HorizontalMovement()
    {
        bool isWalking = false;
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
            isWalking = true;
        }
        if (Mathf.Abs(body.velocity.x) > speed)
        {
            body.velocity = new Vector2(speed * Mathf.Sign(body.velocity.x), body.velocity.y);
        }

        if(isWalking && onGround)
        {
            animator.SetBool("running", true);
        }
        else
        {
            animator.SetBool("running", false);
        }
    }

    void Jump()
    {
        if (onGround && jumpCurrentCooldown == 0)
        {
            body.AddForce(new Vector2(0, jumpHeight));
            jumpCurrentCooldown = jumpCooldown;
            animator.SetBool("jumping", true);
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
            EvilAi cont = enemy.transform.gameObject.GetComponent<EvilAi>();
            if (cont != null)
            {
                cont.TakeDamage(strength);
            }
            else
            {
                Debug.Log(enemy);
                EvilFly cnt = enemy.transform.gameObject.GetComponent<EvilFly>();
                Debug.Log(cnt);
                cnt.TakeDamage(strength);
            }
        }
        attackCurrentCooldown = attackCooldown;
    }

    void Death()
    {
        // animation
        Debug.Log("I'm ded too :(");
        SceneManager.LoadScene("SampleScene");
    }
    
    public void TakeDamage(float damage)
    {
        // animation;
        hp -= damage;
    }
}
