using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// UI Wrapper storing references to Popup UI elements
/// </summary>
public class PopupContent : MonoBehaviour
{
    public TextMeshProUGUI Title;
    public TextMeshProUGUI Body;
    public Button BackButton;
    public TextMeshProUGUI ProgressCount;
    public TextMeshProUGUI ContinueText;
    public Button ContinueButton;

    public void InitText(string title, string body)
    {
        Title.text = title;
        Body.text = body;
    }
}
