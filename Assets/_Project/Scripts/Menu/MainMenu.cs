using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour
{
    [Header("Menu Connections")]
    [SerializeField] private CreditsUIBuilder m_creditsBuilder;
    [SerializeField] private GameObject m_firstSelectedButton;

    [Header("Entry Animation Settings")]
    [Tooltip("Drag the Canvas Groups here in the order you want them to appear.")]
    [SerializeField] private CanvasGroup[] m_uiElementsToAnimate;
    [SerializeField] private float m_animationDuration = 0.5f;
    [SerializeField] private float m_staggerDelay = 0.2f;


    private void Awake()
    {

        if (m_firstSelectedButton != null)
        {
            EventSystem.current.SetSelectedGameObject(m_firstSelectedButton);
        }
    }

    private void Start()
    {
        foreach (var element in m_uiElementsToAnimate)
        {
            if (element != null) element.alpha = 0f;
        }

        StartCoroutine(AnimateMenuIn());
    }

    private IEnumerator AnimateMenuIn()
    {
        // Wait a split second before starting so it doesn't happen during a loading lag spike
        yield return new WaitForSeconds(0.1f);

        // Animate each element one by one
        foreach (var element in m_uiElementsToAnimate)
        {
            if (element != null)
            {
                StartCoroutine(FadeIn(element));
                yield return new WaitForSeconds(m_staggerDelay); // Wait before starting the next one
            }
        }

        // Once the buttons are visible, give keyboard/controller focus to the first button
        if (m_firstSelectedButton != null)
        {
            EventSystem.current.SetSelectedGameObject(m_firstSelectedButton);
        }
    }

    private IEnumerator FadeIn(CanvasGroup element)
    {
        float elapsedTime = 0f;

        while (elapsedTime < m_animationDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / m_animationDuration;

            // Smooth "Ease Out" effect so the fade feels natural instead of robotic
            element.alpha = 1f - Mathf.Pow(1f - t, 3f);

            yield return null; // Wait for the next frame
        }

        // Snap to exactly 1 (fully visible) just to be safe
        element.alpha = 1f;
    }

    public void PlayGame()
    {
        Debug.Log("Starting game...");
        SceneManager.LoadScene("GameStart");
    }

    public void OpenCredits()
    {
        if (m_creditsBuilder != null) m_creditsBuilder.Open();
    }

    public void QuitGame()
    {
        Debug.Log("Quitting the game...");
        Application.Quit();
    }
}