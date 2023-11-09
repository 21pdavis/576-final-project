using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGrid : MonoBehaviour 
{
    //TODO, split up code into sections,
    //Manhattan distance for A star
    //Since potentially rooms, some manager that will decide which MapGrid to use
    Vector3 mousePosition = new Vector3();
    [SerializeField] float gridSize = 1f;
    [SerializeField] Vector2 offset = new Vector2();
    [SerializeField] int length = 100;
    [SerializeField] int width = 100;
    public MapGridUnit[,] grid;
    Ray ray;

    public MapGridUnit getGridUnit(int x, int z) {
        return grid[z, x];
    }

    // Start is called before the first frame update
    void Awake() {
        grid = new MapGridUnit[length, width];
        Debug.Log(grid[0, 0]);
        for(int i = 0; i < length; i++) {
            for(int j = 0; j < width; j++) {
                grid[i, j] = new MapGridUnit();
            }
        }
    }

    // Update is called once per frame
    void Update() {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        LayerMask mask = LayerMask.GetMask("Ground");
        if (Physics.Raycast(ray, out hit, 100, mask)) {
            mousePosition = hit.point;
            //Debug.Log(mousePosition);
            Debug.DrawRay(ray.origin + ray.direction, ray.direction * 100, Color.red);
        }
    }
    public MapGridUnit[,] getGrid() {
        return grid;
    }

    public (int, int) worldToGridPoint(float x, float z) {
        //Mathf.FloorToInt((x - offset.x) + gridSize/2)/gridSize
        //Mathf.RoundToInt(x)
        return (Mathf.FloorToInt((x-offset.x+gridSize/2)/gridSize), Mathf.FloorToInt((z - offset.y + gridSize / 2) / gridSize));
        //return ((Mathf.RoundToInt(x) - offset.x) / gridSize, (Mathf.RoundToInt(z) - offset.y) / gridSize);

    }
    public (int, int) clampGridIndex(int x, int z) {
        return (Mathf.Clamp(x, 0, width), Mathf.Clamp(z, 0, length));
        //coords += new Vector3(offset.x, 0, offset.y);
        ////coords.
        //coords -= new Vector3(offset.x, 0, offset.y);
    }
    public Vector3 clampWorldToEquivalentGridPositions(float x, float z) {
        (int, int) gridPos = (worldToGridPoint(x, z));
        gridPos = clampGridIndex(gridPos.Item1, gridPos.Item2);
        return gridToWorldPoint(gridPos.Item1, gridPos.Item2);
    }
    public Vector3 gridToWorldPoint(int x, int z) {
        return new Vector3(x * gridSize + offset.x, 0, z * gridSize + offset.y);
    }

    public Vector3 getMousePos() {
        return mousePosition;
    }
    public static int manhattanDistance((int x, int z) start, (int x, int z) end) {

        return Mathf.Abs(start.x - end.x) + Mathf.Abs(start.z - end.z);
    }

    public bool addObject(int x, int z, int id) {
        Debug.Log((x, z));
        int existingID = grid[z, x].getID();
        if (existingID != -1)
            return false;

        grid[z, x].setID(id);
        GameObject temp3 = GameObject.CreatePrimitive(PrimitiveType.Cube);
        temp3.transform.position = gridToWorldPoint(x, z);
        temp3.GetComponent<Renderer>().material.color = Color.yellow;
        grid[z, x].setGameObject(temp3);
        return true;
    }

    public bool deleteObject(int x, int z) {
        int existingID = grid[z, x].getID();
        if (existingID == -1)
            return false;

        grid[z, x].setID(-1);
        grid[z, x].deleteGameObject();
        return true;
    }

}