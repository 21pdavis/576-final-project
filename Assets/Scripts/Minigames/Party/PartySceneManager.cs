using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartySceneManager : MonoBehaviour {

    public static PartySceneManager PM;
    public GameObject boidPrefab;
    public int numBoids = 20;
    public GameObject[] allBoids;
    public Vector3 boidLimits = new Vector3(5.0f, 5.0f, 5.0f);
    public Vector3 goalPos = Vector3.zero;

    [Header("Boid Settings")]
    [Range(0.0f, 5.0f)] public float minSpeed;
    [Range(0.0f, 5.0f)] public float maxSpeed;
    [Range(1.0f, 10.0f)] public float neighbourDistance;
    [Range(1.0f, 5.0f)] public float rotationSpeed;

    void Start() {
        PopupManager.Instance.InitPopupSequence("PartyMinigame",
            onEndOfSequence: MinigameManager.Instance.MinigameInitFunctions["Party"]
        );
    }

    public void InitBoids() {       
        allBoids = new GameObject[numBoids];

        for (int i = 0; i < numBoids; ++i) {

            Vector3 pos = this.transform.position + new Vector3(
                Random.Range(0, boidLimits.x),
                Random.Range(-boidLimits.y, boidLimits.y),
                Random.Range(-boidLimits.z, boidLimits.z));

            allBoids[i] = Instantiate(boidPrefab, pos, Quaternion.identity);
        }

        PM = this;
        goalPos = this.transform.position;
    }

    void Update() {

        if (Random.Range(0, 100) < 10) {

            goalPos = this.transform.position + new Vector3(
                Random.Range(-boidLimits.x, boidLimits.x),
                Random.Range(-boidLimits.y, boidLimits.y),
                Random.Range(-boidLimits.z, boidLimits.z));
        } else {
            //set goal position to be players position
            goalPos = GameObject.FindGameObjectWithTag("Player").transform.position;
        }
    }
}