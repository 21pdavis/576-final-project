using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameObject LeftSelectorPrefab;
    public GameObject RightSelectorPrefab;
    public Vector3 LeftSelectorPos { private get; set; }
    public Vector3 RightSelectorPos { private get; set; }
    public RectTransform SelectedButton { private get; set; }

    [Header("Main Pause Menu")]
    [SerializeField]
    private Button continueButton;

    [SerializeField]
    private Button optionsButton;

    [SerializeField]
    private Button exitButton;

    [Header("Options Menu")]
    [SerializeField]
    private GameObject optionsMenu;

    [SerializeField]
    private Slider volumeSlider;

    [SerializeField]
    private Button optionsBackButton;

    private GameObject leftSelector;
    private GameObject rightSelector;

    // Start is called before the first frame update
    void OnEnable()
    {
        continueButton.onClick.AddListener(() =>
        {
            TimeController.Instance.Paused = false;
            ResourceController.Instance.Paused = false;
            gameObject.SetActive(false);
        });

        optionsButton.onClick.AddListener(() =>
        {
            optionsMenu.SetActive(true);
            continueButton.transform.parent.gameObject.SetActive(false);

            // attach listener to options menu back button
            optionsBackButton.onClick.AddListener(() =>
            {
                optionsMenu.SetActive(false);
                continueButton.transform.parent.gameObject.SetActive(true);
            });
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

    public void ResetMenu()
    {
        optionsMenu.SetActive(false);
        continueButton.transform.parent.gameObject.SetActive(true);
    }

    public void ClearSelectors()
    {
        Destroy(leftSelector);
        Destroy(rightSelector);
    }

    private void UpdateSelectors()
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
    }

    // Update is called once per frame
    private void Update()
    {
        UpdateSelectors();

        if (optionsMenu.activeSelf)
        {
            AudioListener.volume = volumeSlider.value;
        }
    }
}
