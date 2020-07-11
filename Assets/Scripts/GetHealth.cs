using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetHealth : MonoBehaviour
{
    public GameObject player;
    Characteristics stats;
    Controls controls;
    Text textbox;

    void Start()
    {
        stats = player.GetComponent<Characteristics>();
        controls = player.GetComponent<Controls>();
        textbox = GetComponent<Text>();
    }

    void Update()
    {
        Vector2 healthBar = transform.GetChild(0).GetComponent<RectTransform>().sizeDelta;
        transform.GetChild(0).GetComponent<RectTransform>().sizeDelta =
                              new Vector2(stats.maxHp, healthBar.y);
        Vector2 remainingHealth = transform.GetChild(1).GetComponent<RectTransform>().sizeDelta;
        transform.GetChild(1).GetComponent<RectTransform>().sizeDelta =
                              new Vector2(controls.hp, remainingHealth.y);
        textbox.text = "Health: " + controls.hp.ToString();
    }
}
