using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimeDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text clockDisplay;
    [SerializeField] private TMP_Text dayOfWeekDisplay;
    [SerializeField] private Image curtain;

    private float maxAmount;
    private float minAmount;
    [SerializeField] private float currTime;
    private int date;
    private string[] daysOfWeek;
    // Start is called before the first frame update
    private void Start()
    {
        daysOfWeek = new string[] { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday" };
    }

    private IEnumerator Hide(float amount) {
        amount = Mathf.Clamp(amount, 0, 1);
        while (curtain.color.a <= amount) {
            curtain.color = new Color(curtain.color.r, curtain.color.g, curtain.color.b, Mathf.Clamp(curtain.color.a + .1f, 0, 1));
            yield return new WaitForSeconds(.1f);
        }
    }

    private IEnumerator unHide() {
        while (curtain.color.a >= .001f) {
            curtain.color = new Color(curtain.color.r, curtain.color.g, curtain.color.b, Mathf.Clamp(curtain.color.a - .1f, 0, 1));
            yield return new WaitForSeconds(.1f);
        }
    }

    public void dimScreen(float amount) {
        StopAllCoroutines();
        StartCoroutine(Hide(amount));
    }
    public void unDimScreen() {
        StopAllCoroutines();
        StartCoroutine(unHide());
    }
    public float MaxAmount {
        get => maxAmount;
        set {
            maxAmount = value;
        }
    }

    public float MinAmount {
        get => minAmount;
        set {
            minAmount = value;
        }
    }

    public float Time {
        get => currTime;
        set {
            currTime = Mathf.Clamp(value, minAmount, maxAmount);
            string hour = "" + Mathf.RoundToInt(currTime) / 60 % 24;
            hour = hour.PadLeft(2, '0');

            string min = "" + Mathf.RoundToInt(currTime) % 60;
            min = min.PadLeft(2, '0');
            clockDisplay.text = hour + ":" + min;
        }
    }

    public int Date {
        get => date;
        set {
            date = value;
            dayOfWeekDisplay.text = daysOfWeek[value];
        }
    }
}
