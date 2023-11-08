using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Defines and tracks the different types of player resources, propagating them to meters in the UI using <see cref="ResourceMeter"/>.
/// </summary>
public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance { get; private set; }

    // TODO: Design question: do we want to keep meters low (exhaustion/hunger) or high (energy/nourishment), i.e., positive or negative?
    [SerializeField] private ResourceMeter exhaustionMeter;
    [SerializeField] private ResourceMeter hungerMeter;
    // more meters here...

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
        int max = resources[key].Meter.MaxAmount;
        if (value <= max)
            resources[key].Amount = value;
        else
            Debug.LogWarning($"Trying to set value of {key.FirstCharacterToUpper()} to {value}, which is greater than its max of {max}");
    }

    public int Exhaustion 
    {
        get => resources["exhaustion"].Amount;
        set => SetResourceValue("exhaustion", value);
    }

    public int Hunger
    {
        get => resources["hunger"].Amount;
        set => SetResourceValue("hunger", value);
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
            ["exhaustion"] = new Resource(0, exhaustionMeter),
            ["hunger"] = new Resource(-290, hungerMeter)
        };

        // init exhaustion min/max
        resources["exhaustion"].Meter.MinAmount = 0;
        resources["exhaustion"].Meter.MaxAmount = 100;

        // init hunger min/max
        resources["hunger"].Meter.MinAmount = 0;
        resources["hunger"].Meter.MaxAmount = 100;
    }

    // Update is called once per frame
    void Update()
    {
        //DebugUI();
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
            Exhaustion += Exhaustion + diff >= 0 ? diff : 0;
            Debug.Log($"Exhaustion set to {Exhaustion}");
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            Hunger += diff;
            Debug.Log($"Hunger set to {Hunger}");
        }
    }
}
