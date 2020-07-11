using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    float strength;
    float attackSpeed;
    Characteristics characteristics;
    Rigidbody2D body;
    float height;
    bool onGround = true;
    float movement;
    int jumpCurrentCooldown = 0;
    public int jumpCooldown;

    void Start()
    {
        characteristics = GetComponent<Characteristics>();
        body = GetComponent<Rigidbody2D>();
    }

    void Update() {
        strength = characteristics.strength;
        attackSpeed = characteristics.attackSpeed;
    }
}
