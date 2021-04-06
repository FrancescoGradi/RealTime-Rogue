using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class ShowFPS : MonoBehaviour
{
    public float timer, refresh, avgFramerate;
    public string display = "{0} FPS";
    public TMP_Text fpsText;
    
    void Update() {
        float timelapse = Time.smoothDeltaTime;
        timer = timer <= 0 ? refresh : timer -= timelapse;

        if (timer <= 0) avgFramerate = (int) (1f / timelapse);

        if (avgFramerate < 1)
            avgFramerate = 0;

        fpsText.text = string.Format(display, avgFramerate.ToString());
    }
    
}
