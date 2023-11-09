using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//TODO string pulling
public class Follow : MonoBehaviour
{
    [SerializeField] MapGrid mousePoint;
    public (int, int) gridPos;
    // Start is called before the first frame update
    void Start()
    {
        gridPos = (0, 0);

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 temp = mousePoint.getMousePos();
        gridPos = mousePoint.worldToGridPoint(temp.x, temp.z);
        //Debug.Log(gridPos);
        temp = mousePoint.gridToWorldPoint(gridPos.Item1, gridPos.Item2);
        //Debug.Log(temp);
        transform.position = (temp);
        if (Input.GetKeyDown(KeyCode.Mouse1)) {
            bool success = mousePoint.addObject(gridPos.Item1, gridPos.Item2, 1);
        } else if (Input.GetKeyDown(KeyCode.Mouse0)) {
            bool success = mousePoint.deleteObject(gridPos.Item1, gridPos.Item2);
        }
        //Debug.Log(MapGrid.manhattanDistance(gridPos, (50, 50)));
    }

    public (int, int) getGridPos() {
        return gridPos;
    }
}
