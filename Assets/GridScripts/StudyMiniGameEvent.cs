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
    Vector3 oldCameraPos;
    Quaternion oldCameraRotation;
    // Start is called before the first frame update
    void Start()
    {
        created = new Queue<GameObject>();
        playerPos = FindAnyObjectByType<PlayerFollow>().transform;
    }
    public void initGame() {
        oldCameraPos = GameManager.Instance.currentCamera.transform.position;
        oldCameraRotation = GameManager.Instance.currentCamera.transform.rotation;

        GameObject newPlayerGame = Instantiate(studyPlayerPrefab);
        newPlayerGame.GetComponent<StudyGame>().eventScript = this;

        newPlayerGame.transform.position = playerPos.transform.position;
        playerPos.gameObject.SetActive(false);
        created.Enqueue(newPlayerGame);

        GameObject newUI = Instantiate(studyPlayerUIPrefab);
        GameManager.Instance.currentCamera.transform.position = new Vector3(-0.13f, 5.25f, -4f);
        GameManager.Instance.currentCamera.transform.eulerAngles = new Vector3(60, 90, 0);
        GameManager.Instance.currentCamera.orthographic = false;
        ResourceController.Instance.Paused = true;
        FindAnyObjectByType<WorkStationInteractable>().GetComponentInParent<Animator>().SetBool("isStudying", true);
        created.Enqueue(newUI);
    }

    public void finishedGame() {
        playerPos.gameObject.SetActive(true);
        FindAnyObjectByType<WorkStationInteractable>().GetComponentInParent<Animator>().SetBool("isStudying", false);
        GameManager.Instance.currentCamera.transform.position = oldCameraPos;
        GameManager.Instance.currentCamera.transform.rotation = oldCameraRotation;
        GameManager.Instance.currentCamera.orthographic = true;
        ResourceController.Instance.Paused = false;
        while (created.Count > 0) {
            Destroy(created.Dequeue());
        }
    }
}
