using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CouchIteraction : Interactable
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public override void onClick() {
        base.onClick();
        Debug.Log(x + " , " + z);
    }
}
