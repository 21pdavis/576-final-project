using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO string pulling throw a ray to the next waypoint 
//Or compute average of is neighboring vertices and pull towards it

//contains info on each grid unit, used for path finding algorithm
public class pathFindingNode {
    public int x;
    public int z;
    public int distanceFromStart;
    public pathFindingNode parent;//used to reconstruct path
    public pathFindingNode(int x, int z) {
        this.x = x;
        this.z = z;
        distanceFromStart = 0;
        parent = null;
    }

    public pathFindingNode(int x, int z, int distance, pathFindingNode prev) {
        this.x = x;
        this.z = z;
        distanceFromStart = distance;
        parent = prev;
    }
    public void printCords() {
        Debug.Log("(" + x + "," + z + ")");
    }
}

//handles moving the player from its current position to a destination cordinate
public class PlayerFollow : MonoBehaviour {

    [SerializeField] int length;        //the length and width of the map
    [SerializeField] int width;
    [SerializeField] MapGrid grid;      //the current grid
    bool[,] usedLocation;               //contains which locations have been checked
    [SerializeField] PlayerTasks pt;    //handles giving PlayerFollow the locations to go to
    (int, int) lastEndGoal = (-1, -1);  //the current grid coordinate it is trying to reach
    int eventId = -1;                   //the id of the event we are traveling to 
    bool paused = false;

    bool newGoal = false;               //informs if a new goal coordinate was just given
    [SerializeField] Vector2Int currPos = new Vector2Int(0, 0);
    private Coroutine pathCouroutine;

    // Start is called before the first frame update
    void Start() {
        (int length , int width) dimensions = grid.getGridDimensions();
        length = dimensions.length;
        width = dimensions.width;
        usedLocation = new bool[length, width];
        GetComponent<Renderer>().material.color = Color.red;
        pt = GetComponent<PlayerTasks>();
    }

    // Update is called once per frame
    void Update() {
        transform.position = Vector3.MoveTowards(transform.position, grid.gridToWorldPoint(currPos.x, currPos.y), 5f*Time.deltaTime);//moves to the current position(smoothly)
        if (paused) {
            return;
        }
        if (newGoal) {//if we have a new goal, we find the new path
            newGoal = false;
            if (pathCouroutine != null)
                StopCoroutine(pathCouroutine);
            pathFindingNode end = findPath((lastEndGoal.Item1, lastEndGoal.Item2));//finds shortest path using Djkstra's algorithm
            LinkedList<pathFindingNode> wayPoints = new LinkedList<pathFindingNode>();
            while (end != null) {
                wayPoints.AddFirst(end);
                end = end.parent;
            }
            pathCouroutine = StartCoroutine(Travel(wayPoints));//and start traveling to it
        } 
    }
    void clearHistory() {
        for (int i = 0; i < length; i++) {
            for (int j = 0; j < width; j++) {
                usedLocation[i, j] = false;
            }
        }
    }

    //checks the tile to make sure that it is a valid tile
    bool checkTile((int x, int z) next) {
        if (next.x < 0 || next.x >= width) {
            return false;
        }
        if (next.z < 0 || next.z >= length) {
            return false;
        }
        if (usedLocation[next.z, next.x]) {
            return false;
        }
        if (grid.getGridUnit(next.x, next.z).getID() != -1) {
            return false;
        }
        usedLocation[next.z, next.x] = true;
        return true;
    }

    //finds the path from the current position to a new goal position
    pathFindingNode findPath((int x, int z) goal) {
        Queue<pathFindingNode> temp = new Queue<pathFindingNode>();
        int count = 0;
        temp.Enqueue(new pathFindingNode(currPos.x, currPos.y));
        while (temp.Count > 0) {
            count++;
            pathFindingNode u = temp.Dequeue();
            usedLocation[u.z, u.x] = true;
            if (u.x == goal.x && u.z == goal.z) {
                clearHistory();
                return u;
            }
            //check surrounding
            if (checkTile((u.x + 1, u.z))) {
                temp.Enqueue(new pathFindingNode(u.x + 1, u.z, u.distanceFromStart + 1, u));
            }
            if (checkTile((u.x - 1, u.z))) {
                temp.Enqueue(new pathFindingNode(u.x - 1, u.z, u.distanceFromStart + 1, u));
            }
            if (checkTile((u.x, u.z + 1))) {
                temp.Enqueue(new pathFindingNode(u.x, u.z + 1, u.distanceFromStart + 1, u));
            }
            if (checkTile((u.x, u.z - 1))) {
                temp.Enqueue(new pathFindingNode(u.x, u.z - 1, u.distanceFromStart + 1, u));
            }
        }
        clearHistory();
        return null;
    }

    IEnumerator Travel(LinkedList<pathFindingNode> travelPoints) {
        foreach(pathFindingNode points in travelPoints) {
            //debug blocks showing the path we have taken
            GameObject temp3 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            temp3.transform.position = grid.gridToWorldPoint(points.x, points.z);
            temp3.GetComponent<Renderer>().material.color = Color.yellow;
            temp3.transform.localScale = new Vector3(.5f, 2, .5f);
            Destroy(temp3, 4f);
            //makes sure we are in the correct position(old position)
            transform.position = grid.gridToWorldPoint(currPos.x, currPos.y);
            Vector2Int oldPos = currPos;
            currPos = (new Vector2Int(points.x, points.z));
            //waits depending on the distance traveled
            yield return new WaitForSeconds(.2f * (currPos - oldPos).magnitude);
            
        }
        pt.finTask(eventId);//gets next task aka next travel coordinates
        pt.getNextTask();
        yield return null;
    }
    public void pause() {
        paused = true;
    }

    public void pause(float seconds) {
        StartCoroutine(delayPause(seconds));
    }
    public void unpause() {
        paused = false;
    }

    IEnumerator delayPause(float seconds) {
        pause();
        yield return new WaitForSeconds(seconds);
        unpause();
        yield return null;
    }
    public void setGoal(int x, int y) {//called to set the new travel coordinates
        newGoal = true;
        lastEndGoal = (x, y);
        eventId = -1;
    }
    public void setGoal(int x, int y, int id) {//called to set the new travel coordinates
        newGoal = true;
        lastEndGoal = (x, y);
        eventId = id;
    }

}

