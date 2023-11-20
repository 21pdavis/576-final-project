using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MapGrid : MonoBehaviour 
{
    //TODO, split up code into sections,
    //Manhattan distance for A star
    //Since potentially rooms, some manager that will decide which MapGrid to use

    
    [SerializeField] float gridSize = 1f;               //The size of the grid
    [SerializeField] Vector2 offset = new Vector2();    //the offset that the origin of the grid is from (0,0)
    [SerializeField] int length = 100;                  //the number of grid units per column
    [SerializeField] int width = 100;                   //the number of grid units per row
    [SerializeField] PrefabList idList;                 //the list of prefabs
    [SerializeField] MapGridUnit[,] grid;               //the grid representing the map

    Vector3 mousePosition = new Vector3();
    Ray ray;

    public MapGridUnit getGridUnit(int x, int z) {      //gets the grid unit for a given x, z
        return grid[z, x];
    }
    public MapGridUnit[,] getGrid() {                   //gets the grid
        return grid;
    }

    public (int length, int width) getGridDimensions() {
        return (length, width);
    }

    // Start is called before the first frame update
    void Awake() {                                      //initializes the grid
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

    //converts a world cordinates x and z into a grid position
    public (int, int) worldToGridPoint(float x, float z) {
        return (Mathf.FloorToInt((x-offset.x+gridSize/2)/gridSize), Mathf.FloorToInt((z - offset.y + gridSize / 2) / gridSize));
    }

    //clamps a grid coordinate to make sure it does not go outside the bounds of the grid
    public (int, int) clampGridIndex(int x, int z) {
        return (Mathf.Clamp(x, 0, width), Mathf.Clamp(z, 0, length));
    }

    //the world coordinates clamped to fall on a single grid unit
    public Vector3 clampWorldToEquivalentGridPositions(float x, float z) {
        (int, int) gridPos = (worldToGridPoint(x, z));
        gridPos = clampGridIndex(gridPos.Item1, gridPos.Item2);
        return gridToWorldPoint(gridPos.Item1, gridPos.Item2);
    }

    //converts a grid point to a world vector3
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
        if (existingID != -1) {//clicks object, should probably move out
            grid[z, x].click();
            return false;
        }

        GameObject createdObject;
        grid[z, x].setID(id);
        if (idList.idMap.Length > id && idList.idMap[id] != null) {
            createdObject = idList.idMap[id];
            Vector3 position = gridToWorldPoint(x, z);
            createdObject = Instantiate(createdObject, position ,new Quaternion());
        } else {
            createdObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
            createdObject.transform.position = gridToWorldPoint(x, z);
            createdObject.GetComponent<Renderer>().material.color = Color.yellow;
        }
        grid[z, x].setGameObject(createdObject);
        return true;
    }

    public bool addObject(int x, int z, int id, GameObject go) {
        int existingID = grid[z, x].getID();
        if (existingID != -1) {//clicks object, should probably move out
            return false;
        }
        grid[z, x].setGameObject(go);
        grid[z, x].setID(id);
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

    public GameObject pickupObject(int x, int z) {
        int existingID = grid[z, x].getID();
        if (existingID == -1)
            return null;

        grid[z, x].setID(-1);
        return grid[z, x].removeGameObject();
    }

}