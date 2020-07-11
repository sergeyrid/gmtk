using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetHealth : MonoBehaviour
{
    public GameObject player;
    Characteristics stats;
    Text textbox;
    // Start is called before the first frame update
    void Start()
    {
        stats = player.GetComponent<Characteristics>();
        textbox = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        textbox.text = "Health: " + stats.hp.ToString();
    }
}
