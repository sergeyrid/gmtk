using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathRay : MonoBehaviour
{
    public int playerLayer = 10;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.gameObject.layer == playerLayer)
        {
            Scene current = SceneManager.GetActiveScene();
            SceneManager.LoadScene(current.name);
        }
    }
}
