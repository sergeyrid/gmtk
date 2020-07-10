using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controls : MonoBehaviour
{
    float speed;
    float jumpHeight;
    float airControl;
    float hp;
    float strength;
    float attackSpeed;
    float dizziness;

    Rigidbody2D body;
    Characteristics characteristics;

    // Start is called before the first frame update
    void Start()
    {
        characteristics = GetComponent<Characteristics>();
        speed = characteristics.speed;
        jumpHeight = characteristics.jumpHeight;
        airControl = characteristics.airControl;
        hp = characteristics.hp;
        strength = characteristics.strength;
        attackSpeed = characteristics.attackSpeed;
        dizziness = characteristics.dizziness;
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
