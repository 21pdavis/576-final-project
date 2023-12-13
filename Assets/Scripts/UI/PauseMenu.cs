using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameObject canvas;

    public GameObject LeftSelectorPrefab;
    public GameObject RightSelectorPrefab;
    public Vector3 LeftSelectorPos { private get; set; }
    public Vector3 RightSelectorPos { private get; set; }
    public RectTransform SelectedButton { private get; set; }

    [SerializeField]
    private Button continueButton;
    [SerializeField]
    private Button optionsButton;
    [SerializeField]
    private Button exitButton; 

    private GameObject leftSelector;
    private GameObject rightSelector;

    // Start is called before the first frame update
    void OnEnable()
    {
        continueButton.onClick.AddListener(() =>
        {
            TimeController.Instance.Paused = false;
        });

        optionsButton.onClick.AddListener(() =>
        {
            // TODO
            Debug.Log("Options Clicked");
        });

        exitButton.onClick.AddListener(() =>
        {
            #if UNITY_EDITOR
            // Editor play mode handling
            EditorApplication.ExitPlaymode();
            #else
            // Build application handling
            Application.Quit();
            #endif
        });
    }

    // Update is called once per frame
    void Update()
    {
        // place tree graphic selectors around option being hovered over
        if (SelectedButton != null)
        {
            // create selectors on either side of button (separate if statements to 100% ensure both will be created in case of some weird UI desync)
            if (leftSelector == null)
            {
                Vector2 leftPosition = new Vector2(SelectedButton.anchoredPosition.x - (SelectedButton.rect.width / 2f + 25f), SelectedButton.anchoredPosition.y);
                leftSelector = Instantiate(LeftSelectorPrefab, SelectedButton.transform.parent);
                leftSelector.GetComponent<RectTransform>().anchoredPosition = leftPosition;
            }

            if (rightSelector == null)
            {
                Vector2 rightPosition = new Vector2(SelectedButton.anchoredPosition.x + (SelectedButton.rect.width / 2f + 25f), SelectedButton.anchoredPosition.y);
                rightSelector = Instantiate(RightSelectorPrefab, SelectedButton.transform.parent);
                rightSelector.GetComponent<RectTransform>().anchoredPosition = rightPosition;
            }
        }
        else
        {
            if (leftSelector != null)
            {
                Destroy(leftSelector);
            }
            if (rightSelector != null)
            {
                Destroy(rightSelector);
            }
        }
    }
}
