using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO string pulling throw a ray to the next waypoint 
//Ignore, this file was just used to test movement scripts, look at player follow to see how the player is controlled
//Or compute average of is neighboring vertices and pull towards it
public class node {
    public int x;
    public int z;
    public int distanceFromStart;
    public node parent;
    public node(int x, int z) {
        this.x = x;
        this.z = z;
        distanceFromStart = 0;
        parent = null;
    }

    public node(int x, int z, int distance, node prev) {
        this.x = x;
        this.z = z;
        distanceFromStart = distance;
        parent = prev;
    }
    public void printCords() {
        Debug.Log("(" + x + "," + z + ")");
    }
}
public class PathFinding : MonoBehaviour
{
    [SerializeField] int length = 100;
    [SerializeField] int width = 100;
    [SerializeField] Cursor endGoal;
    (int, int) lastEndGoal = (-1, -1);
    [SerializeField] bool perframe = false;
    bool[,] usedLocation;
    [SerializeField] MapGrid grid;
    [SerializeField] Vector2Int startPos = new Vector2Int(0, 0);
    Queue<GameObject> markers;
    // Start is called before the first frame update
    void Start() {
        usedLocation = new bool[length, width];
        markers = new Queue<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || perframe) {
            
            (int, int) endGoalPos = endGoal.getGridPos();
            //if(lastEndGoal.Item1 == endGoalPos.Item1 && lastEndGoal.Item2 == endGoalPos.Item2) {
            //    return;
            //}
            while (markers.Count > 0) {//could probably just move them should don't need to create and destory(ie preinstantiate)
                Destroy(markers.Dequeue());
            }
            lastEndGoal = endGoalPos;
            node end = findPath((endGoalPos.Item1, endGoalPos.Item2));
            while(end != null) {
                
                GameObject temp3 = GameObject.CreatePrimitive(PrimitiveType.Cube);
                temp3.transform.position = grid.gridToWorldPoint(end.x, end.z);
                temp3.GetComponent<Renderer>().material.color = Color.yellow;
                temp3.transform.localScale = new Vector3(.5f, 2, .5f);
                end.printCords();
                end = end.parent;
                markers.Enqueue(temp3);
            }
        }
    }
    void clearHistory() {
        for(int i = 0; i < length; i++) {
            for(int j = 0; j < width; j++) {
                usedLocation[i, j] = false;
            }
        }
    }

    bool checkTile((int x, int z) next) {
        if(next.x < 0 || next.x >= width) {
            return false;
        } 
        if(next.z < 0 || next.z >= length) {
            return false;
        }
        if(usedLocation[next.z, next.x]) {
            return false;
        }
        if(grid.getGridUnit(next.x, next.z).getID() != -1) {
            return false;
        }
        usedLocation[next.z, next.x] = true;
        return true;
    }
    node findPath((int x, int z) goal) {
        Queue<node> temp = new Queue<node>();
        int count = 0;
        temp.Enqueue(new node(startPos.x, startPos.y));
        while(temp.Count > 0) {
            count++;
            node u = temp.Dequeue();
            usedLocation[u.z, u.x] = true;
            if(u.x == goal.x && u.z == goal.z) {
                clearHistory();
                return u;
            }
            //check surrounding
            if(checkTile((u.x+1, u.z))){
                temp.Enqueue(new node(u.x + 1, u.z, u.distanceFromStart + 1, u));
            }
            if (checkTile((u.x - 1, u.z))) {
                temp.Enqueue(new node(u.x - 1, u.z, u.distanceFromStart + 1, u));
            }
            if (checkTile((u.x, u.z + 1))) {
                temp.Enqueue(new node(u.x, u.z + 1, u.distanceFromStart + 1, u));
            }
            if (checkTile((u.x, u.z - 1))) {
                temp.Enqueue(new node(u.x, u.z - 1, u.distanceFromStart + 1, u));
            }
        }
        clearHistory();
        return null;
    }

}
