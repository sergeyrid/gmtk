using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controls : MonoBehaviour
{
    float speed = characteristics.speed;
    float jumpHeight = characteristics.jumpHeight;
    float airControl = characteristics.airControl;
    float hp = characteristics.hp;
    float strength = characteristics.strength;
    float attackSpeed = characteristics.attackSpeed;
    float dizziness = characteristics.dizziness;

    public Rigidbody2D body;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        Debug.Log(characteristics.speed);
    }

    // Update is called once per frame
    void Update()
    {
        float movement = Input.GetAxis("Horizontal") * speed;
        body.velocity = new Vector2(movement, body.velocity.y);
    }
}
