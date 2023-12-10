using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StudyGame : MonoBehaviour
{
    [SerializeField] GameObject Player;
    [SerializeField] float progress = 0;
    [SerializeField] int timeTaken = 0;
    public float timer = 0;
    public bool fallingAsleep;
    public bool isAsleep;
    public bool isMath;
    private int goal = 0;
    public StudyMiniGameEvent eventScript;
    public InputField inputField;
    public Text displayText;
    
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update() {
        if (progress > 30) {
            eventScript.finishedGame();
        }
        if (isMath) {
            //Debug.Log(inputField.isFocused);
            if(Input.GetKeyDown(KeyCode.Return)) {
                Debug.Log("entered");
                int result;
                if(int.TryParse(inputField.text, out result)) {
                    if(result == goal) {
                        progress += 5;
                    }
                }
                displayText.text = "What is?\n";
                isMath = false;
            }
            return;
        }
        timer += Time.deltaTime;
        if (isAsleep) {
            if(timer > 5) {
                timer = 0;
                GetComponent<Renderer>().material.color = Color.white;
                timeTaken += 10;
                isAsleep = false;
            }
            return;
        }
        if (fallingAsleep) {
            if (Input.GetKeyDown(KeyCode.Mouse0)){
                Debug.Log("pressed");
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                LayerMask mask = LayerMask.GetMask("Player");
                if (Physics.Raycast(ray, out hit, 100, mask)) {
                    Debug.Log(hit);
                    if (hit.transform.gameObject.name.Contains("Player")) {
                        timeTaken += 2;
                        fallingAsleep = false;
                        GetComponent<Renderer>().material.color = Color.white;
                    }
                }
            } else if(timer > 2) {
                GetComponent<Renderer>().material.color = Color.black;
                timer = 0;
                timeTaken += 2;
                isAsleep = true;
                fallingAsleep = false;
            }
            return;
        }
        progress += Time.deltaTime;
        
        if (timer > 5) {
            if (ResourceManager.Instance.Stress >= 50) {
                timeTaken += 10;
            } else {
                timeTaken += 5;
            }
            timer = 0;
            if (Random.Range(0f, 1f) * ResourceManager.Instance.Energy <= 30) {
                GetComponent<Renderer>().material.color = Color.red;
                fallingAsleep = true;
            } else {
                int first = Random.Range(0, 10);
                int second = Random.Range(0, 10);
                displayText.text = "What is?\n" + first + "+" + second;
                goal = first + second;
                isMath = true;
            }
            
        }
    }
}
