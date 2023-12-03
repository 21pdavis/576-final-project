using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGridUnit
{
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

    public void setGameObject(GameObject go) {
        this.go = go;
    }

    public void reset() {
        deleteGameObject();
        this.id = -1;
    }


}
