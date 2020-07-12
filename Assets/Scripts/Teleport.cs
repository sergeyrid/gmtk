using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Teleport : MonoBehaviour
{
    public int playerLayer = 10;
    public string nextLevel;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.gameObject.layer == playerLayer)
        {
            SceneManager.LoadScene(nextLevel);
        }
    }
}
