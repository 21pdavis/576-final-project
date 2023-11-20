using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Represents 1 grid unit
public class MapGridUnit
{
    //consists of an id and a game object, with the id representing the type of game object
    public int id;
    GameObject go;
    public MapGridUnit(int id) {
        this.id = id;
        
    }
    public MapGridUnit() {
        this.id = -1;
        go = null;
    }

    public int getID() {
        return id;
    }

    public void setID(int id) {
        this.id = id;
    }

    public void deleteGameObject() {
        if (go != null) {
            Object.Destroy(go);
            go = null;
        }
    }
    public GameObject removeGameObject() {
        GameObject removed = go;
        if (go != null) {
            go = null;
        }
        return removed;
    }

    public void setGameObject(GameObject go) {
        this.go = go;
    }
    public void click() {
        Interactable clickable = go.GetComponent<Interactable>();
        if(clickable != null) {
            clickable.onClick();
        }
    }

    public void reset() {
        deleteGameObject();
        this.id = -1;
    }


}
