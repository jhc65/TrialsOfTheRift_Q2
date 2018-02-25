using UnityEngine;
using System.Collections;


// I found this script here and made edits: http://newbquest.com/2014/06/the-art-of-screenshake-with-unity-2d-script/
public class Screenshake : MonoBehaviour 
{

    Vector3 originalCameraPosition;

    private float shakeAmt = 0;
    private RectTransform go_UI;

    public void StartShake()
    {   
        originalCameraPosition = go_UI.anchoredPosition;
        InvokeRepeating("CameraShake", 0, .008f);
        Invoke("StopShaking", 0.1666f * 2);
    }

    void CameraShake()
    {
        if(shakeAmt>0) 
        {
            float quakeAmt = Random.value*shakeAmt*2f - shakeAmt;
            Vector3 pp = go_UI.anchoredPosition;
            pp.y+= quakeAmt;
            pp.x+= quakeAmt; // can also add to x and/or z
            go_UI.anchoredPosition = pp;
        }
    }

    void StopShaking()
    {
        CancelInvoke("CameraShake");
        go_UI.anchoredPosition = originalCameraPosition;
        shakeAmt = 0;
    }

    public void SetShake(float shake_in, RectTransform rectIn) {
        shakeAmt = shake_in;
        go_UI = rectIn;
    }

}