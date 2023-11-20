using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ScriptableObject for defining the key and text content of a popup sequence.
/// </summary>
[CreateAssetMenu(fileName = "Popup Sequence", menuName = "Custom Assets/Popup Sequence")]
public class PopupSequence : ScriptableObject
{
    [System.Serializable]
    public struct Popup
    {
        [TextArea(1, 1)]
        public string Title;
        [TextArea(3, 1)]
        public string Body;
    }

    public string Key;
    public List<Popup> popups = new();
}
