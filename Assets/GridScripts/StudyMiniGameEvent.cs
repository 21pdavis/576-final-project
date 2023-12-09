using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StudyMiniGameEvent : MonoBehaviour
{
    Queue<GameObject> created;
    [SerializeField] GameObject studyPlayerPrefab;
    [SerializeField] GameObject studyPlayerUIPrefab;
    Transform playerPos;
    // Start is called before the first frame update
    void Start()
    {
        created = new Queue<GameObject>();
        playerPos = FindAnyObjectByType<PlayerFollow>().transform;
    }
    public void initGame() {
        GameObject newPlayerGame = Instantiate(studyPlayerPrefab);
        newPlayerGame.GetComponent<StudyGame>().eventScript = this;

        newPlayerGame.transform.position = playerPos.transform.position;
        playerPos.gameObject.SetActive(false);
        created.Enqueue(newPlayerGame);

        GameObject newUI = Instantiate(studyPlayerUIPrefab);
        newPlayerGame.GetComponent<StudyGame>().inputField = newUI.GetComponentInChildren<InputField>();
        foreach (Text text in newUI.GetComponentsInChildren<Text>()) {
            if(text.transform.parent == newUI.transform) {
                newPlayerGame.GetComponent<StudyGame>().displayText = text;
            }
        }
        created.Enqueue(newUI);
    }

    public void finishedGame() {
        playerPos.gameObject.SetActive(true);
        while(created.Count > 0) {
            Destroy(created.Dequeue());
        }
    }
}
