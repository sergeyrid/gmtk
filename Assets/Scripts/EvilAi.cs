using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvilAi : MonoBehaviour
{
    public float walktime;
    public float waittime;
    public float speed;
    public int playerLayer = 2;
    public float attackReach;
    public float damage;

    Rigidbody2D body;

    int direction = -1;
    int state = 0;
    float previoustime;
    Animator anim;
    GameObject eye;
    // Start is called before the first frame update
    void Start()
    {
        eye = GameObject.Find("eye-bg");
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        body.velocity = new Vector2(-speed, body.velocity.y);
        previoustime = Time.time;
    }

    void flip()
    {
        eye.transform.localPosition = new Vector3(eye.transform.localPosition.x, eye.transform.localPosition.y, -eye.transform.localPosition.z);
        transform.rotation = Quaternion.Euler(new Vector3(0, 180 - transform.rotation.eulerAngles.y, 0));
    }

    void dowalkcycle()
    {
        if (state == 0)
        {   
            body.velocity = new Vector2(-speed, body.velocity.y);
            if (Time.time - previoustime >= walktime)
            {
                previoustime = Time.time;
                state = 1;
                anim.SetTrigger("ChangeState");
            }
        }
        if (state == 1)
        {
            body.velocity = new Vector2(0, body.velocity.y);
            if (Time.time - previoustime >= waittime)
            {
                previoustime = Time.time;
                state = 2;
                anim.SetTrigger("ChangeState");
                direction = 1;
                flip();
            }
            
        }
        if (state == 2)
        {
            body.velocity = new Vector2(speed, body.velocity.y);
            if (Time.time - previoustime >= walktime)
            {
                previoustime = Time.time;
                state = 3;
                anim.SetTrigger("ChangeState");
                
            }
        }
        if (state == 3)
        {
            body.velocity = new Vector2(0, body.velocity.y);
            if (Time.time - previoustime >= waittime)
            {
                previoustime = Time.time;
                state = 0;
                anim.SetTrigger("ChangeState");
                direction = -1;
                flip();
            }
        }
    }

    void Attack()
    {
        RaycastHit2D []playersHit = Physics2D.RaycastAll(transform.position, Vector2.right * direction, attackReach, 1<<playerLayer);
        foreach (RaycastHit2D player in playersHit)
        {
            Controls cont = player.transform.gameObject.GetComponent<Controls>();
            cont.TakeDamage(damage);
        }
    }

    // Update is called once per frame
    void Update()
    {
        dowalkcycle();
        //Debug.Log(previoustime.ToString()+' '+ Time.time.ToString());
    }
}
