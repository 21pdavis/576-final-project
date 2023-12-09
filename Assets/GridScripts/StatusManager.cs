using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusManager : MonoBehaviour
{
    public float tiredness = 0;
    public float stress = 0;
    public int money = 100;
    public int ambiance = 0;
    public int dirtiness = 0;
    public int knowledge = 0;
    [Range(0, 1440)]
    public float currTime = 0;
    public Light sun;
    public float TimeSpeed = 2;
    public Text timeText;
    void Start() {

        sun.transform.eulerAngles = new Vector3(180 * ((currTime - 360) / 770), -30, 0);
    }
    void Update() {
        currTime += TimeSpeed * Time.deltaTime;
        
        sun.transform.eulerAngles = new Vector3(180 * ((currTime - 360) / 770) , -30, 0);
        string hour = "" + Mathf.RoundToInt(currTime) / 60 % 24;
        hour = hour.PadLeft(2, '0');

        string min = "" + Mathf.RoundToInt(currTime) % 60;
        min = min.PadLeft(2, '0');
        timeText.text = hour + ":" + min;
    }
}
