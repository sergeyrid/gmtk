using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionControl : MonoBehaviour
{
    Characteristics stats;
    // Start is called before the first frame update
    void Start()
    {
        stats = GetComponent<Characteristics>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("q"))
        {
            stats.speed += 30;
            stats.acceleration += 10;
            stats.slipperiness += 0.95f;
            stats.jumpHeight += 2000;
            stats.airControl += 0.3f;
            stats.hp += 10;
            stats.strength += 20;
            stats.dizziness += 10;
            stats.attackReach += 50;
        }
    }
}
