using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/PrefabListScriptableObject", order = 1)]
public class PrefabList : ScriptableObject {

    public GameObject[] idMap;
    public bool started = false;
    private void Awake() {
        started = true;
    }
}
