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
    public int energy = 4;
    public int hunger = 0;
    public int prepardness = 0;
    public int stress = 0;

    // if player has below x energy, then both clock hands will move
    public int energyThreshold = 4; 

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
        energy = ResourceManager.Instance.Energy/10;
        hunger = ResourceManager.Instance.Hunger / 10;
        prepardness = ResourceManager.Instance.Preparedness / 10;
        stress = ResourceManager.Instance.Stress / 5;
        //the more tired, the faster the hands and zones move
        greenSpeed = 0.1f - (0.01f * stress);
        redSpeed = 0.1f - (0.01f * stress);
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
        FindAnyObjectByType<Animator>().SetTrigger("WakeUp");
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
        ResourceController.Instance.Tick = 1;
        ResourceController.Instance.PauseSpecific(2, false);
        GameManager.Instance.gridPos = new Vector2Int(56, 48);
        TimeController.Instance.TimeSpeed = TimeController.Instance.DefaultSpeed;
        SceneManager.LoadScene("Wu");

        if (ResourceManager.Instance.Date == 0)
        {
            ResourceController.Instance.Paused = true;
            TimeController.Instance.Paused = true;
            PopupManager.Instance.InitPopupSequence(
                "welcome",
                onEndOfSequence: () =>
                {
                    ResourceController.Instance.Paused = false;
                    TimeController.Instance.Paused = false;
                }
            );
        }
        else if (ResourceManager.Instance.Date == 2)
        {
            ResourceController.Instance.Paused = true;
            TimeController.Instance.Paused = true;
            PopupManager.Instance.InitPopupSequence(
                "halfway",
                onEndOfSequence: () =>
                {
                    ResourceController.Instance.Paused = false;
                    TimeController.Instance.Paused = false;
                }
            );
        }
        else if (ResourceManager.Instance.Date == 4)
        {
            ResourceController.Instance.Paused = true;
            TimeController.Instance.Paused = true;
            PopupManager.Instance.InitPopupSequence(
                "fridayWarning",
                onEndOfSequence: () =>
                {
                    ResourceController.Instance.Paused = false;
                    TimeController.Instance.Paused = false;
                }
            );
        }
        else
        {
            ResourceController.Instance.Paused = false;
            TimeController.Instance.Paused = false;
        }
    }
    
    void OnWin(){
        Debug.Log("You win!");
        audioSource.time = 0.5f;
        audioSource.PlayOneShot(alarmSound, 0.2f);
        Invoke("Done", 7.5f);
    }

    void OnLose(){
        Debug.Log("You lose!");
        audioSource.PlayOneShot(alarmSound, 0.2f);
        IEnumerator passTime() {
            ResourceController.Instance.Paused = true;
            int remaining = 120;
            float currTime = ResourceManager.Instance.Time;
            while(remaining > 0) {
                yield return new WaitForSeconds(.1f);
                ResourceManager.Instance.Time += 2;
                remaining-=2;
                if(remaining%10 == 0) {
                    ResourceManager.Instance.Stress += 1;
                    ResourceManager.Instance.Hunger += 1;
                }
            }
            ResourceController.Instance.Paused = false;
            yield return null;
        }
        StartCoroutine(passTime());
        //could play some animation before hand to smoothen out the thing
        Invoke("Done", 7.5f);
    }
}
