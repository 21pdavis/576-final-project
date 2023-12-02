using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlarmClock : MonoBehaviour
{

    public Camera cam;
    public GameObject clockHand;
    public GameObject hoursHand;

    public GameObject greenZone;
    public GameObject redZone;

    private bool clockStopped = false;
    private bool greenCounterClockwise = true;
    private bool redCounterClockwise = false;

    public float greenSpeed = 0.1f;
    public float redSpeed = 0.1f;

    // temporary, hook up with globals later
    public int energy = 6;
    public int hunger = 0;
    public int prepardness = 0;
    public int stress = 0;

    // if player has below x energy, then both clock hands will move
    public int energyThreshold = 5; 

    //if player is tired, then clock will move faster
    // if the player is well reseted, the red zone won't exist
    // Start is called before the first frame update
    void Start()
    {
        //activate the camera
        cam.enabled = true;

        //the more tired, the faster the hands and zones move
        greenSpeed = 0.1f - (0.01f * energy);
        redSpeed = 0.1f - (0.01f * energy);
    }

    // Update is called once per frame
    void Update()
    {
        //
        // if clockStopped is true, check for intersection
        if (clockStopped){
            

        }
        else {
            // continually rotate the clockhand
            clockHand.transform.Rotate(0, 0, 1);
            //rotate the hour hand
            hoursHand.transform.Rotate(0, 0, 0.1f);

            if (greenZone.transform.rotation.z > 0.5f)
            {
                greenCounterClockwise = false;
            }
            else if (greenZone.transform.rotation.z < -0.5f)
            {
                greenCounterClockwise = true;
            }
            //rotate green zone
            if (greenCounterClockwise)
                greenZone.transform.Rotate(0, 0, greenSpeed);
            else
                greenZone.transform.Rotate(0, 0, -greenSpeed);

            if (energy < energyThreshold)
            {
                //rotate red zone
                if (redCounterClockwise)
                {
                    redZone.transform.Rotate(0, 0, redSpeed);
                }
                else
                {
                    redZone.transform.Rotate(0, 0, -redSpeed);
                }
            }
            else
            {
                // if energy is above threshold, then red zone doesn't exist
                redZone.SetActive(false);
                // big hand too
                clockHand.SetActive(false);

            }
            if (redZone.transform.rotation.z > 0.5f)
            {
                redCounterClockwise = false;
            }
            else if (redZone.transform.rotation.z < -0.5f)
            {
                redCounterClockwise = true;
            }

            //rotate red zone
            if (redCounterClockwise)
            {
                redZone.transform.Rotate(0, 0, redSpeed);
            }
            else
            {
                redZone.transform.Rotate(0, 0, -redSpeed);
            }

            // if user clicks the mouse, stop the clock
            if (Input.GetMouseButtonDown(0))
            {
                // stop the clock
                StopClock(); 
            }
        }
       
    }

    void StopClock(){
        // stop the clock
        clockStopped = true;
        //check for collision between clock hand and green
        bool greenCorrect = hoursHand.transform.GetChild(0).GetComponent<Collider>().bounds.Intersects(greenZone.transform.GetChild(0).GetComponent<Collider>().bounds);
        bool redCorrect = clockHand.transform.GetChild(0).GetComponent<Collider>().bounds.Intersects(redZone.transform.GetChild(0).GetComponent<Collider>().bounds);
        Debug.Log("Green: " + greenCorrect);
        Debug.Log("Red: " + redCorrect);

        //if red is active
        if (redZone.activeSelf){
            //if both are correct
            if (greenCorrect && redCorrect){
                //win
                Debug.Log("You win!");
            }
            else{
                //lose
                Debug.Log("You lose!");
            }
        }
        else {
            //if only green is correct
            if (greenCorrect){
                //win
                Debug.Log("You win!");
            }
            else{
                //lose
                Debug.Log("You lose!");
            }
        }
    }
}
