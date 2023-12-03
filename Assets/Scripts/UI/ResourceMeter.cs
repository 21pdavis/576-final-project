using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Wrapper class for updating UI resource meters.
/// </summary>
public class ResourceMeter : MonoBehaviour
{
    private Slider slider;

    private int maxAmount;
    private int minAmount;
    private int amount;

    private void Awake()
    {
        slider = GetComponent<Slider>();
    }

    public int MaxAmount
    {
        get => maxAmount;
        set
        {
            maxAmount = value;
            slider.maxValue = value;
        }
    }

    public int MinAmount
    {
        get => minAmount;
        set
        {
            minAmount = value;
            slider.minValue = value;
        }
    }

    public int Amount
    {
        get => amount;
        set 
        {
            amount = value;
            slider.value = value;
        }
    }
}
