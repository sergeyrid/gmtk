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
    public float deltaMaxHp = 10;
    public float deltaStrength = 20;
    public float deltaDizziness = 10;
    public float deltaAttackReach = 1;
    public int deltaAttackCooldown = -1;
    Characteristics stats;
    Controls controls;

    void Start()
    {
        stats = GetComponent<Characteristics>();
        controls = GetComponent<Controls>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("q") && stats.potions > 0)
        {
            stats.speed += deltaSpeed;
            stats.acceleration += deltaAcceleration;
            if (stats.slipperiness < 0.98f)
            {
                stats.slipperiness += 0.01f;
            }
            else if (stats.slipperiness < 0.99f)
            {
                stats.slipperiness += 0.002f;
            }
            else
            {
                stats.slipperiness += 0.001f;
            }
            stats.jumpHeight += deltaJumpHeight;
            stats.airControl *= deltaAirControl;
            stats.maxHp += deltaMaxHp;
            stats.strength += deltaStrength;
            stats.dizziness += deltaDizziness;
            stats.attackReach += deltaAttackReach;
            stats.attackCooldown += deltaAttackCooldown;
            --stats.potions;
            controls.hp += deltaMaxHp;
        }
    }
}
