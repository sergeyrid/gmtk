using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetPotions : MonoBehaviour
{
    public GameObject player;
    Characteristics stats;
    Text textbox;

    void Start()
    {
        stats = player.GetComponent<Characteristics>();
        textbox = GetComponent<Text>();
    }

    void Update()
    {
        textbox.text = stats.potions.ToString();
    }
}
