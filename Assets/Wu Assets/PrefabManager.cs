using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class PrefabManager : MonoBehaviour
{
    [SerializeField] MapGrid grid;
    //can read from file and save from file
    string test = 
        "1,2,-1,-1,-1\n" +
        "1,-1,-1,-1,-1\n" +
        "-1,-1,1,-1,-1\n";
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S)) {
            save();
        } else if (Input.GetKeyDown(KeyCode.L)) {
            Instantiate();
        }
    }
    void Instantiate() {
        StreamReader sr = null;
        if (File.Exists("text.txt")) {//update to byte stream where first 2 bytes are length and width and each other byte is a index
            try {
                test = "";
                string line;
                sr = new StreamReader("text.txt");
                while ((line = sr.ReadLine()) != null) {
                    test += (line + "\n");
                }
                Debug.Log("result");
                Debug.Log(test);
            }
            catch (System.Exception e) {
                Debug.Log("High score file exist, but something went wrong when reading file");
                Debug.Log(e);
            }
            finally {
                if (sr != null) {
                    sr.Close();
                }
            }
        }
        //string[] line = test.Split('\n');
        string[] lines = test.Split('\n');
        for (int i = 0; i < lines.Length; i++) {
            if(lines[i].Length <= 1) {
                continue;
            }
            string[] ids = lines[i].Split(',');
            for (int j = 0; j < ids.Length; j++) {
                int id = -1;
                if (int.TryParse(ids[j], out id)) {
                    if (id != -1) {
                        grid.addObject(j, i, id);
                    }
                }
                else {
                    id = -1;
                }
                grid.getGridUnit(j, i);
            }
        }
    }
    void save() {
        StreamWriter sw = null;
        try {
            sw = File.CreateText("text.txt");
            MapGridUnit[,] currGrid = grid.getGrid();
            for(int i = 0; i < currGrid.GetLength(0); i++) {
                string line = "";
                for(int j = 0; j < currGrid.GetLength(1) - 1; j++) {
                    line += (currGrid[i, j].getID() + ",");
                }
                if(currGrid.GetLength(1) > 1) {
                    line += (currGrid[i, currGrid.GetLength(1) - 1].getID());
                }

                sw.WriteLine(line);
            }
        }
        catch (System.Exception e) {
            Debug.Log("High score file exist, but something went wrong when reading file");
            Debug.Log(e);
        }
        finally {
            if (sw != null) {
                sw.Close();
            }
        }
    }
}
