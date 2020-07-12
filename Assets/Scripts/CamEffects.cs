using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamEffects : MonoBehaviour
{
    // Start is called before the first frame update
    Camera cam;
    GameObject camObj;
    public GameObject player;
    Characteristics stats;

    float constShakeAmt = 1;
    float constShakeRotAmt = 3;
    float constShakeSmooth = 1;

    float shakeAmt = 1;
    float shakeRotAmt = 3;
    float shakeSmooth = 1;

    void Start()
    {
        stats = player.GetComponent<Characteristics>();
        cam = Camera.main;
        camObj = cam.gameObject;
        //Camera.main.SetReplacementShader(Shader.Find("Custom/Shader"),"Opaque");
        // StartShake(shakeAmt, shakeSmooth, 0.1f);
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
        InvokeRepeating("Shake", delay, delay);
    }

    void StopShake()
    {
        CancelInvoke("Shake");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        shakeAmt = constShakeAmt * stats.dizziness;
        shakeRotAmt = constShakeRotAmt * stats.dizziness;
        shakeSmooth = constShakeSmooth * stats.dizziness;
        Shake();
    }
}
