using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

    [SerializeField] private SimpleAudioController simpleAudioController;
    [SerializeField] private SceneTransition sceneTransition;

    [SerializeField] private GameObject m_firstSelectedButton;

    [SerializeField] private GameObject buttonContainer;

    private void Awake()
    {
        if (m_firstSelectedButton != null)
        {
            EventSystem.current.SetSelectedGameObject(m_firstSelectedButton);
        }
    }

    public void Resume()
    {
        GameManager.Instance.TogglePause();
    }

    public void MainMenu()
    {
        simpleAudioController.StopMainInstance();
        Time.timeScale = 1f;
        buttonContainer.SetActive(false);
        sceneTransition.StartFadeAndChangeScene();
    }

    public void QuitGame()
    {
        Debug.Log("Quitting the game...");
        Application.Quit();
    }
}
