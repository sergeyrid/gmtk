using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionControl : MonoBehaviour
{
    public float deltaSpeed = 5;
    public float deltaAcceleration = 1;
    public float deltaSlipperiness = 0.002f;
    public float deltaJumpHeight = 200;
    public float deltaAirControl = -0.01f;
    public float deltaHp = 10;
    public float deltaStrength = 20;
    public float deltaDizziness = 10;
    public float deltaAttackReach = 1;
    public int deltaAttackCooldown = -1;
    Characteristics stats;
    // Start is called before the first frame update
    void Start()
    {
        stats = GetComponent<Characteristics>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("q") && stats.potions > 0)
        {
            stats.speed += deltaSpeed;
            stats.acceleration += deltaAcceleration;
            stats.slipperiness += deltaSlipperiness;
            stats.jumpHeight += deltaJumpHeight;
            stats.airControl += deltaAirControl;
            stats.hp += deltaHp;
            stats.strength += deltaStrength;
            stats.dizziness += deltaDizziness;
            stats.attackReach += deltaAttackReach;
            stats.attackCooldown += deltaAttackCooldown;
            --stats.potions;
        }
    }
}
