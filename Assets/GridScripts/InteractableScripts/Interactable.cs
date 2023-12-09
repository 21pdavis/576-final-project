using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public int x;
    public int z;
    public int id;
    public virtual void onClick() {
        Debug.Log("Clicked");
    }
}
