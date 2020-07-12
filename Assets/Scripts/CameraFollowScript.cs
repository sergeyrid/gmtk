using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowScript : MonoBehaviour
{
    // Start is called before the first frame update
    public bool doFollow;
    public float speed = 1;
    public GameObject objToTrack;

    // Update is called once per frame
    void FixedUpdate()
    {
        if(doFollow)
        {
            Vector3 pos = Vector2.Lerp(transform.position, objToTrack.transform.position, Time.deltaTime/speed);
            pos.z = -100;
            transform.position = pos;
        }
    }
}
