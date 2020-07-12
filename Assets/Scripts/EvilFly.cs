using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvilFly : MonoBehaviour
{
    public float walktime;
    public float waittime;
    public float speed;
    public int playerLayer = 10;
    public float attackReach;
    public float damage;
    public float losedistance;
    public float sightdistance;
    public float hp = 100;

    Rigidbody2D body;

    int direction = -1;
    int state = 0;
    bool sightedenemy = false;
    bool attacking = false;
    float attacktimestart;
    float previoustime;
    float height;
    Animator anim;
    GameObject eye;
    GameObject eyelid;
    GameObject enemy;
    // Start is called before the first frame update
    void Start()
    {
        eye = transform.Find("body/eye").gameObject;
        eyelid = transform.Find("body/eyelid").gameObject;
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        body.velocity = new Vector2(-speed, body.velocity.y);
        previoustime = Time.time;
        height = GetComponent<Collider2D>().bounds.size.y;
    }

    void flip()
    {
        eye.transform.localPosition = new Vector3(eye.transform.localPosition.x, eye.transform.localPosition.y, -eye.transform.localPosition.z);
        anim.SetBool("flip", !anim.GetBool("flip"));
        transform.rotation = Quaternion.Euler(new Vector3(0, 180 - transform.rotation.eulerAngles.y, 0));
    }

    void DoFlightCycle()
    {
        if (state == 0)
        {   
            body.velocity = new Vector2(-speed, body.velocity.y);
            if (Time.time - previoustime >= walktime)
            {
                anim.SetBool("Moving",true);
                previoustime = Time.time;
                state = 1;
            }
        }
        if (state == 1)
        {
            body.velocity = new Vector2(0, body.velocity.y);
            if (Time.time - previoustime >= waittime)
            {
                anim.SetBool("Moving",false);
                previoustime = Time.time;
                state = 2;
                direction = 1;
                flip();
            }
            
        }
        if (state == 2)
        {
            body.velocity = new Vector2(speed, body.velocity.y);
            if (Time.time - previoustime >= walktime)
            {
                anim.SetBool("Moving",true);
                previoustime = Time.time;
                state = 3;                
            }
        }
        if (state == 3)
        {
            body.velocity = new Vector2(0, body.velocity.y);
            if (Time.time - previoustime >= waittime)
            {
                anim.SetBool("Moving",false);
                previoustime = Time.time;
                state = 0;
                direction = -1;
                flip();
            }
        }
    }

    void Attack()
    {
        Debug.Log("Die human");
        RaycastHit2D []playersHit = Physics2D.BoxCastAll(transform.position, new Vector2(height / 2, attackReach / 2),
                                                         0, Vector2.right * direction, attackReach, 1<<playerLayer);
        anim.SetBool("Attacking",true);
        foreach (RaycastHit2D player in playersHit)
        {
            Controls cont = player.transform.gameObject.GetComponent<Controls>();
            cont.TakeDamage(damage);
            Debug.Log("TookDamage");
        }
        Invoke("StopAttack",anim.GetCurrentAnimatorClipInfo(0)[0].clip.length);
    }

    void StopAttack()
    {
        anim.SetBool("Attacking",false);
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.gameObject.layer != playerLayer)
        {
            return ;
        }
        enemy = other.gameObject;
        sightedenemy = true;
        state = 0;
        Debug.Log("Collided");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (hp <= 0)
        {
            Death();
        }
        if (attacking)
        {
            // Debug.Log("JUST A PUSSY");
            float delta = Time.time - attacktimestart;
            if (delta <= 0.32 && delta >= 0.28)
            {
                Attack();
                attacking = false;
                anim.SetTrigger("ChangeState");
            }
        }
        else if (sightedenemy)
        {
            // Debug.Log("FUCKING PUSSY");
            float dist = Vector3.Distance(enemy.transform.position, transform.position);
            if ((enemy.transform.position.x - transform.position.x) * direction < 0)
            {
                direction *= -1;
                flip();
            }

            if (dist > losedistance)
            {
                sightedenemy = false;
                previoustime = Time.time;
                if (direction == 1)
                {
                    flip();
                }
                direction = -1;
            }
            else if (dist > attackReach) 
            {
                body.velocity = new Vector2(speed * direction, body.velocity.y);
            }
            else
            {
                attacking = true;
                anim.SetTrigger("Attack");
                attacktimestart = Time.time;
            }
        }
        else
        {
            // Debug.Log("WALKING MENACE");
            DoFlightCycle();
        }
        //Debug.Log(previoustime.ToString()+' '+ Time.time.ToString());
    }

    void Death()
    {
        // anim;
        Destroy(gameObject);
    }

    public void TakeDamage(float damage)
    {
        // anim;
        hp -= damage;
    }
}
