using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public Button tutorialButton;
    public GameObject backgroundImage;
    public GameObject mainButtonsPanel;
    public Text[] tutorialTexts;
    private int currentIndex = -1;
    private bool tutorialStarted = false;

    void Start()
    {
        // Ensure the background image GameObject is initially inactive
        backgroundImage.SetActive(false);

        // Disable all tutorial texts initially
        foreach (Text text in tutorialTexts)
        {
            text.gameObject.SetActive(false);
        }

        // Add an onClick listener to the tutorialButton
        tutorialButton.onClick.AddListener(StartTutorial);
    }

    void StartTutorial()
    {
        tutorialButton.gameObject.SetActive(false); // Hide the tutorial button
        tutorialStarted = true;
        ShowNextTutorial();
    }

    void Update()
    {
        if (tutorialStarted && currentIndex >= 0 && Input.GetMouseButtonDown(0))
        {
            ShowNextTutorial();
        }
    }

    void ShowNextTutorial()
    {
        currentIndex++;

        if (currentIndex < tutorialTexts.Length)
        {
            backgroundImage.SetActive(true);

            foreach (Text text in tutorialTexts)
            {
                text.gameObject.SetActive(false);
            }

            tutorialTexts[currentIndex].gameObject.SetActive(true);
        }
        else
        {
            backgroundImage.SetActive(false);
            tutorialTexts[2].gameObject.SetActive(false);
            mainButtonsPanel.SetActive(true);
            // Implement the action when the tutorial ends (e.g., load main menu)
            Debug.Log("Tutorial Ended. Perform necessary actions...");
            // Add your code here to load the main menu scene or perform any other action
        }
    }
}
