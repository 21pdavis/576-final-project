using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Question {
    public string questionText;
    public string index1Ans;
    public string index2Ans;
    public string index3Ans;
    public string index4Ans;
    int correctIndex;
    public Question(string questionText, string index1, string index2, string index3, string index4, int index) {
        this.questionText = questionText;
        index1Ans = index1;
        index2Ans = index2;
        index3Ans = index3;
        index4Ans = index4;
        correctIndex = index;
    }

    public bool isCorrect(int index) {
        return index == correctIndex;
    }

    public string getAns(int i) {
        switch (i) {
            case 1:
                return index1Ans;
            case 2:
                return index2Ans;
            case 3:
                return index3Ans;
            case 4:
                return index4Ans;
            default:
                return "";
        }
    }
}
public class StudyGame : MonoBehaviour {
    public enum Operators {
        plus = 0,
        minus = 1,
        multiply = 2,
    }
    public float progress = 0;
    [SerializeField] int timeTaken = 0;
    public StudyMiniGameEvent eventScript;
    public Question currQuestion = null;
    [SerializeField] private Animator playerStateAni;
    private float timer = 0;
    private bool fallingAsleep;
    private bool isAsleep;
    private bool isMath;

    void Start() {
        playerStateAni = GetComponent<Animator>();
    }
    Question newQuestion() {
        //Random operation and random constants
        Operators operation = (Operators)Random.Range(0, 3);
        int constant1 = Random.Range(1, 20);
        int constant2 = Random.Range(1, 20);
        int correctAns = 0;
        string questionText = "";
        List<int> possibleAns = new List<int>();
        //gets the correct answer for the random operation and constants
        switch (operation) {
            case Operators.plus:
                questionText = constant1+"+"+constant2;
                correctAns = constant1 + constant2;
                break;
            case Operators.minus:
                questionText = constant1 + "-" + constant2;
                correctAns = constant1 - constant2;
                break;
            case Operators.multiply:
                questionText = constant1 + "*" + constant2;
                correctAns = constant1 * constant2;
                break;
            default:
                break;
        }
        //randomizes a few options in order to generate a few possible answers by slightly modifying the constants for modifying the answer by a few integers, until we have 3 incorrect answers
        while (possibleAns.Count < 3) {
            int randomizedAns = correctAns;
            int modifiedConstant1 = constant1;
            int modifiedConstant2 = constant2;
            switch (Random.Range(0, 2)) {
                case 0:
                    modifiedConstant1 += Random.Range(-1, 2);
                    modifiedConstant2 += Random.Range(-1, 2);
                    switch (operation) {
                        case Operators.plus:
                            randomizedAns = modifiedConstant1 + modifiedConstant2;
                            break;
                        case Operators.minus:
                            randomizedAns = modifiedConstant1 - modifiedConstant2;
                            break;
                        case Operators.multiply:
                            randomizedAns = modifiedConstant1 * modifiedConstant2;
                            break;
                        default:
                            break;
                    }
                    break;
                case 1:
                    randomizedAns += Random.Range(-2, 3);
                    break;
                default:
                    break;
            }
            if(!possibleAns.Contains(randomizedAns) && randomizedAns != correctAns)
                possibleAns.Insert(Random.Range(0, possibleAns.Count+1), randomizedAns);
        }
        //adds the correct answer in a random location
        possibleAns.Insert(Random.Range(0, possibleAns.Count + 1), correctAns);
        //generates the new question
        return new Question(questionText, ""+possibleAns[0], "" + possibleAns[1], "" + possibleAns[2], "" + possibleAns[3], possibleAns.IndexOf(correctAns) + 1);
    }

    // Update is called once per frame
    void Update() {
        if (progress > 30) {
            eventScript.finishedGame();
        }
        if (isMath) {
            TimeController.Instance.TimeSpeed = 1;
            ResourceController.Instance.Tick = 40;
            return;
        }
        if (ResourceManager.Instance.Stress >= 50) {
            TimeController.Instance.TimeSpeed = 10;
            ResourceController.Instance.Tick = 2;
        }
        else {
            TimeController.Instance.TimeSpeed = 5;
            ResourceController.Instance.Tick = 4;
        }
        timer += Time.deltaTime;
        if (isAsleep) {
            if (timer > 5) {
                timer = 0;
                timeTaken += 10;
                isAsleep = false;
            } else {
                TimeController.Instance.TimeSpeed = 20;
                ResourceController.Instance.Tick = 1;
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
                        playerStateAni.SetBool("fallingAsleep", false);
                        timeTaken += 3;
                        fallingAsleep = false;
                    }
                }
            } else if(timer > 3) {
                playerStateAni.SetTrigger("fellAsleep");
                playerStateAni.SetBool("fallingAsleep", false);
                timer = 0;
                timeTaken += 3;
                isAsleep = true;
                fallingAsleep = false;
            }
            return;
        }
        progress += Time.deltaTime;
        
        if (timer > 5) {
            timer = 0;
            ResourceManager.Instance.Preparedness += 1;
            if (Random.Range(0f, 1f) * ResourceManager.Instance.Stress >= 30) {//falling asleep
                playerStateAni.SetBool("fallingAsleep", true);
                fallingAsleep = true;
            } else {
                TimeController.Instance.TimeSpeed = 1;
                ResourceController.Instance.Tick = 40;
                currQuestion = newQuestion();
                isMath = true;
            }
        }
    }
    public bool answer(int index) {
        if(currQuestion != null) {
            bool result = currQuestion.isCorrect(index);
            if (isMath) {
                if (result) {
                    progress += 5;
                    ResourceManager.Instance.Preparedness += 10;
                } else {
                    ResourceManager.Instance.Stress += 10;
                }
                currQuestion = null;
            }
            isMath = false;
            return result;
        }
        return false;

    }
}
