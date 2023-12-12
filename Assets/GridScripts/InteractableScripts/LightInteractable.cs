using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightInteractable : Interactable
{
    // Start is called before the first frame update
    bool turnOnLight = true;
    Light lampLight;
    // Start is called before the first frame update
    void Start() {
        lampLight = transform.parent.GetComponentInChildren<Light>();
        lampLight.enabled = turnOnLight;
    }

    // Update is called once per frame
    void Update() {

    }
    public override void onClick() {
        base.onClick();
        turnOnLight = !turnOnLight;
        lampLight.enabled = turnOnLight;
    }
}
