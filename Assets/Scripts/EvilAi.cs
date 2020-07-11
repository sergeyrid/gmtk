using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvilAi : MonoBehaviour
{
    public float walktime;
    public float waittime;
    public float speed;
    Rigidbody2D body;


    int state = 0;
    float previoustime;
    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        body.velocity = new Vector2(-speed, 0);
        previoustime = Time.time;
    }

    void dowalkcycle()
    {
        if (state == 0)
        {   
            body.velocity = new Vector2(-speed, 0);
            if (Time.time - previoustime >= walktime)
            {
                previoustime = Time.time;
                state = 1;
            }
        }
        if (state == 1)
        {
            body.velocity = new Vector2(0, 0);
            if (Time.time - previoustime >= waittime)
            {
                previoustime = Time.time;
                state = 2;
            }
            
        }
        if (state == 2)
        {
            body.velocity = new Vector2(speed, 0);
            if (Time.time - previoustime >= walktime)
            {
                previoustime = Time.time;
                state = 3;
            }
        }
        if (state == 3)
        {
            body.velocity = new Vector2(0, 0);
            if (Time.time - previoustime >= waittime)
            {
                previoustime = Time.time;
                state = 0;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        dowalkcycle();
        Debug.Log(previoustime.ToString()+' '+ Time.time.ToString());
    }
}
