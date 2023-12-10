using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AlarmClock : MonoBehaviour
{

    public Camera cam;
    public GameObject clockHand;
    public GameObject hoursHand;

    public GameObject greenZone;
    public GameObject redZone;

    public bool clockStopped = true;
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

    public AudioClip alarmSound;
    public AudioClip clockTick;

    private AudioSource audioSource;

    //if player is tired, then clock will move faster
    // if the player is well reseted, the red zone won't exist
    // Start is called before the first frame update
    void Start()
    {
        PopupManager.Instance.InitPopupSequence(
            "AlarmMinigameIntro",
            onEndOfSequence: MinigameManager.Instance.MinigameInitFunctions["Alarm"]
        );

        //the more tired, the faster the hands and zones move
        greenSpeed = 0.1f - (0.01f * energy);
        redSpeed = 0.1f - (0.01f * energy);
        audioSource = GetComponent<AudioSource>();
        
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("Clock stopped: " + clockStopped);
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
            if (Input.GetMouseButtonDown(0) && !clockStopped)
            {
                // stop the clock
                StopClock(); 
            }
        }

        //play clock tick if it is not already playing
        if (!audioSource.isPlaying && !clockStopped){
            audioSource.PlayOneShot(clockTick, 0.2f);
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
                OnWin();
            }
            else{
                //lose
                OnLose();
            }
        }
        else {
            //if only green is correct
            if (greenCorrect){
                //win
                OnWin();
            }
            else{
                //lose
                OnLose();
            }
        }
    }

    void Done() {
        GameManager.Instance.gridPos = new Vector2Int(47, 56);
        SceneManager.LoadScene("Wu");
    }
    
    void OnWin(){
        Debug.Log("You win!");
        audioSource.time = 0.5f;
        audioSource.PlayOneShot(alarmSound, 0.2f);
        Invoke("Done", 1f);
    }

    void OnLose(){
        Debug.Log("You lose!");
        audioSource.PlayOneShot(alarmSound, 0.2f);
        //could play some animation before hand to smoothen out the thing
        Invoke("Done", 1f);
    }
}
