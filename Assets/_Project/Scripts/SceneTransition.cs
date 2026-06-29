using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    [Header("Fade Settings")]
    [Tooltip("Drag your Black Fade Screen CanvasGroup here.")]
    [SerializeField] private CanvasGroup m_fadeScreen;

    [Tooltip("How long the fade to black takes in seconds.")]
    [SerializeField] private float m_fadeDuration = 0.5f;

    [Tooltip("The exact name of the scene you want to load.")]
    [SerializeField] private string m_sceneToLoad = "Level 1";

    // --- IMPORTANT: This must be public so the Animation Event can see it! ---
    public void StartFadeAndChangeScene()
    {
        if (m_fadeScreen != null)
        {
            // Block mouse clicks immediately so the player can't double-click things
            m_fadeScreen.blocksRaycasts = true;
            StartCoroutine(FadeOutRoutine());
        }
        else
        {
            Debug.LogError("No CanvasGroup assigned! Loading scene instantly.");
            SceneManager.LoadScene(m_sceneToLoad);
        }
    }

    private IEnumerator FadeOutRoutine()
    {
        float elapsedTime = 0f;

        // Smoothly fade the alpha from 0 (clear) to 1 (solid black)
        while (elapsedTime < m_fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            m_fadeScreen.alpha = elapsedTime / m_fadeDuration;
            yield return null;
        }

        // Snap to exactly 1 to be safe
        m_fadeScreen.alpha = 1f;

        // Change the scene!
        SceneManager.LoadScene(m_sceneToLoad);
    }
}