using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO string pulling
public class Cursor : MonoBehaviour
{
    [SerializeField] MapGrid mousePoint;
    public (int, int) gridPos;
    [SerializeField] GameObject heldObject;
    // Start is called before the first frame update
    void Start()
    {
        gridPos = (0, 0);

    }

    // Update is called once per frame
    void Update()
    {
        //iteracts with an iteractible on grid
        Vector3 worldPos = mousePoint.getMousePos();
        gridPos = mousePoint.worldToGridPoint(worldPos.x, worldPos.z);
        worldPos = mousePoint.gridToWorldPoint(gridPos.Item1, gridPos.Item2);
        worldPos.y = .5f;
        transform.position = (worldPos);
        if (Input.GetKeyDown(KeyCode.Mouse1)) {
            mousePoint.interact(gridPos.Item1, gridPos.Item2);
        }
        //Old Code that would have enabled buying and moving furniture, decided to scrap
        //if (Input.GetKeyDown(KeyCode.Escape)){
        //    currId = 4;
        //} else if (Input.GetKeyDown(KeyCode.Alpha0)) {
        //    currId = 5;
        //} else if (Input.GetKeyDown(KeyCode.Alpha1)) {
        //    currId = 0;
        //} else if (Input.GetKeyDown(KeyCode.Alpha2)) {
        //    currId = 1;
        //} else if (Input.GetKeyDown(KeyCode.Alpha3)) {
        //    currId = 2;
        //} else if (Input.GetKeyDown(KeyCode.Alpha4)) {
        //    currId = 3;
        //} else if (Input.GetKeyDown(KeyCode.Alpha5)) {
        //    currId = 4;
        //}
        //Vector3 worldPos = mousePoint.getMousePos();
        //gridPos = mousePoint.worldToGridPoint(worldPos.x, worldPos.z);
        ////Debug.Log(gridPos);
        //worldPos = mousePoint.gridToWorldPoint(gridPos.Item1, gridPos.Item2);
        ////Debug.Log(temp);
        //worldPos.y = .5f;
        //transform.position = (worldPos);
        //if(heldObject != null) {
        //    heldObject.transform.position = new Vector3(worldPos.x, heldObject.transform.position.y, worldPos.z);
        //}
        //if (Input.GetKeyDown(KeyCode.Mouse1)) {

        //    bool success = true;
        //    if(heldObject != null) {
        //        success = mousePoint.addObject(gridPos.Item1, gridPos.Item2, 5, heldObject);
        //        if(success)
        //            heldObject = null;
        //    } else {
        //        success = mousePoint.addObject(gridPos.Item1, gridPos.Item2, currId);
        //    }
        //} else if (Input.GetKeyDown(KeyCode.Mouse0)) {
        //    Debug.DrawRay(worldPos, 100 * transform.up, Color.red, 10f);
        //    bool success = mousePoint.deleteObject(gridPos.Item1, gridPos.Item2);
        //} else if(Input.GetKeyDown(KeyCode.Mouse2)) {
        //    Debug.Log("Pickup");
        //    if (heldObject == null) {
        //        Debug.Log("Pickup");
        //        heldObject = mousePoint.pickupObject(gridPos.Item1, gridPos.Item2);
        //    } else {
        //        if (mousePoint.addObject(gridPos.Item1, gridPos.Item2, 5, heldObject))
        //            heldObject = null;
        //    }
        //}
    }

    public (int, int) getGridPos() {
        return gridPos;
    }
}
