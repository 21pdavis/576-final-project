using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Defines and tracks the different types of player resources, propagating them to meters in the UI using <see cref="ResourceMeter"/>.
/// </summary>
public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance { get; private set; }
    
    [Header("Meters")]
    //[SerializeField] private int currentDay;
    //[SerializeField] private float currentTime;
    [SerializeField] private TimeDisplay currentTime;
    [SerializeField] private ResourceMeter stressMeter;
    [SerializeField] private ResourceMeter hungerMeter;
    [SerializeField] private ResourceMeter energyMeter;
    [SerializeField] private ResourceMeter preparednessMeter;

    [Header("Initial Values")]
    [SerializeField] private int dateInitial;
    [SerializeField] private float timeInitial;
    [SerializeField] private int stressInitial;
    [SerializeField] private int hungerInitial;
    [SerializeField] private int energyInitial;
    [SerializeField] private int preparednessInitial;

    /// <summary>
    /// Overall score is hidden from the player until the end, minigames can add to this upon concluding
    /// </summary>
    internal int Score;

    private class Resource
    {
        public int Amount;
        public ResourceMeter Meter;

        public Resource(int amount, ResourceMeter meter)
        {
            Amount = amount;
            Meter = meter;
        }
     }

    private Dictionary<string, Resource> resources;

    private void SetResourceValue(string key, int value)
    {
        resources[key].Amount = Mathf.Clamp(value, resources[key].Meter.MinAmount, resources[key].Meter.MaxAmount);
    }

    public int Stress
    {
        get => resources["stress"].Amount;
        set => SetResourceValue("stress", value);
    }

    public int Hunger
    {
        get => resources["hunger"].Amount;
        set => SetResourceValue("hunger", value);
    }

    public int Energy 
    {
        get => resources["energy"].Amount;
        set => SetResourceValue("energy", value);
    }

    public int Preparedness
    {
        get => resources["preparedness"].Amount;
        set => SetResourceValue("preparedness", value);
    }
    public int Date 
    {
        get => currentTime.Date;
        set => currentTime.Date = value;
    }
    public float Time {
        get => currentTime.Time;
        set => currentTime.Time = value;
    }

    private void Awake()
    {
        // Instantiate Singleton (https://en.wikipedia.org/wiki/Singleton_pattern)
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        resources = new Dictionary<string, Resource>
        {
            ["stress"] = new Resource(stressInitial, stressMeter),
            ["hunger"] = new Resource(hungerInitial, hungerMeter),
            ["energy"] = new Resource(energyInitial, energyMeter),
            ["preparedness"] = new Resource(preparednessInitial, preparednessMeter)
        };
        currentTime.MaxAmount = 1440;
        currentTime.MinAmount = 0;
        currentTime.Time = timeInitial;
        currentTime.Date = dateInitial;

        // init stress min/max
        resources["stress"].Meter.MinAmount = 0;
        resources["stress"].Meter.MaxAmount = 100;

        // init hunger min/max
        resources["hunger"].Meter.MinAmount = 0;
        resources["hunger"].Meter.MaxAmount = 100;

        // init energy min/max
        resources["energy"].Meter.MinAmount = 0;
        resources["energy"].Meter.MaxAmount = 100;

        // init preparedness min/max
        resources["preparedness"].Meter.MinAmount = 0;
        resources["preparedness"].Meter.MaxAmount = 100;

        // start overall score at 0
        Score = 0;
    }

    // Update is called once per frame
    void Update()
    {
        DebugUI();
        UIUpdate();
    }

    private void UIUpdate()
    {
        foreach (KeyValuePair<string, Resource> resourceEntry in resources)
        {
            int amount = resourceEntry.Value.Amount;
            ResourceMeter meter = resourceEntry.Value.Meter;

            meter.Amount = amount;
        }
    }


    /// <summary>
    /// Method that can be used to test the UI with key presses
    /// </summary>
    private void DebugUI()
    {
        int diff = 10;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            diff = -10;
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            Energy += Energy + diff >= 0 ? diff : 0;
            Debug.Log($"Exhaustion set to {Energy}");
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            Hunger += diff;
            Debug.Log($"Hunger set to {Hunger}");
        }
    }
}
