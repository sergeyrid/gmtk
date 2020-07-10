using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamEffects : MonoBehaviour
{
    // Start is called before the first frame update
    Camera cam;
    GameObject camObj;
    float shakeAmt = 1;
    float shakeRotAmt = 3;
    float shakeSmooth = 1;
    void Start()
    {
        cam = Camera.main;
        camObj = cam.gameObject;
    }

    void Shake()
    {
        camObj.transform.localPosition = Vector2.Lerp(camObj.transform.localPosition, new Vector2(Random.Range(shakeAmt, -shakeAmt), Random.Range(shakeAmt, -shakeAmt)), Time.deltaTime/shakeSmooth);
        camObj.transform.rotation = Quaternion.Euler(0,0, Random.Range(-shakeRotAmt,shakeRotAmt));
    }
    void StartShake(float amt, float smooth, float delay) //recomended shakeAmt = {0,25}
    {
        shakeAmt = amt;
        CancelInvoke("Shake");
        InvokeRepeating("Shake", delay,delay);
    }

    void StopShake()
    {
        CancelInvoke("Shake");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
