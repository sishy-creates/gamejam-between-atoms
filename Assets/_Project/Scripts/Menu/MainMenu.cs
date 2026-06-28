using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject m_creditsPanel;

    [SerializeField] private GameObject m_mainPanel;

    public void PlayGame()
    {
        SceneManager.LoadScene("Level 1");
    }

    public void OpenCredits()
    {
        if (m_mainPanel != null) m_mainPanel.SetActive(false);
        if (m_creditsPanel != null) m_creditsPanel.SetActive(true);
    }

    public void CloseCredits()
    {
        if (m_creditsPanel != null) m_creditsPanel.SetActive(false);
        if (m_mainPanel != null) m_mainPanel.SetActive(true);
    }
}
