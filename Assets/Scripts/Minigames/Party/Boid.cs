//Based off of unity starter code on flocking

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour {

    float speed;
    bool turning = false;

    void Start() {

        speed = Random.Range(PartSceneManager.PM.minSpeed, PartSceneManager.PM.maxSpeed);
    }


    void Update() {

        Bounds b = new Bounds(PartSceneManager.PM.transform.position, PartSceneManager.PM.boidLimits * 2.0f);

        if (!b.Contains(transform.position)) {

            turning = true;
        } else {

            turning = false;
        }

        if (turning) {

            Vector3 direction = PartSceneManager.PM.transform.position - transform.position;
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.LookRotation(direction),
                PartSceneManager.PM.rotationSpeed * Time.deltaTime);
        } else {


            if (Random.Range(0, 100) < 10) {

                speed = Random.Range(PartSceneManager.PM.minSpeed, PartSceneManager.PM.maxSpeed);
            }


            if (Random.Range(0, 100) < 10) {
                ApplyRules();
            }
        }

        this.transform.Translate(0.0f, 0.0f, speed * Time.deltaTime);
    }

    private void ApplyRules() {

        GameObject[] gos;
        gos = PartSceneManager.PM.allBoids;

        Vector3 vCentre = Vector3.zero;
        Vector3 vAvoid = Vector3.zero;

        float gSpeed = 0.01f;
        float mDistance;
        int groupSize = 0;

        foreach (GameObject go in gos) {

            if (go != this.gameObject) {

                mDistance = Vector3.Distance(go.transform.position, this.transform.position);
                if (mDistance <= PartSceneManager.PM.neighbourDistance) {

                    vCentre += go.transform.position;
                    groupSize++;

                    if (mDistance < 1.0f) {

                        vAvoid = vAvoid + (this.transform.position - go.transform.position);
                    }

                    Boid otherBoid = go.GetComponent<Boid>();
                    gSpeed = gSpeed + otherBoid.speed;
                }
            }
        }

        if (groupSize > 0) {

            vCentre = vCentre / groupSize + (PartSceneManager.PM.goalPos - this.transform.position);
            speed = gSpeed / groupSize;

            if (speed > PartSceneManager.PM.maxSpeed) {

                speed = PartSceneManager.PM.maxSpeed;
            }

            Vector3 direction = (vCentre + vAvoid) - transform.position;
            //make direction have 0 y
            direction.y = 0;
            if (direction != Vector3.zero) {

                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    Quaternion.LookRotation(direction),
                    PartSceneManager.PM.rotationSpeed * Time.deltaTime);
            }
        }
    }
}