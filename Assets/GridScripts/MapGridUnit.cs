using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Represents 1 grid unit
public class MapGridUnit
{
    //consists of an id and a game object, with the id representing the type of game object
    public int id;
    GameObject go;
    public bool isWalkable;
    public MapGridUnit(int id) {
        this.id = id;
        isWalkable = false;
        
    }
    public MapGridUnit() {
        this.id = -1;
        go = null;
        isWalkable = true;
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
        isWalkable = true;
    }
    public GameObject removeGameObject() {
        GameObject removed = go;
        if (go != null) {
            go = null;
        }
        isWalkable = true;
        return removed;
    }

    public void setGameObject(GameObject go) {
        this.go = go;
    }
    public void click() {
        if(go == null) {
            return;
        }
        Interactable clickable = go.GetComponentInChildren<Interactable>();
        if(clickable != null) {
            clickable.onClick();
        }
    }

    public void reset() {
        deleteGameObject();
        this.id = -1;
        isWalkable = true;
    }


}
