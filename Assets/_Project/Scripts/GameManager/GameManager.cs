using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private UnityEvent onWin;
    [SerializeField] private UnityEvent onLose;
    [SerializeField] private GameObject pauseMenu;

    public bool IsGameOver { get; private set; }
    public bool IsVictory { get; private set; }
    public bool IsPaused { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        Time.timeScale = 1f;
    }

    public void Win()
    {
        if (IsGameOver || IsVictory) return;

        IsVictory = true;
        onWin.Invoke();
        Time.timeScale = 0f;
        Debug.Log("VICTORY");
    }

    public void GameOver()
    {
        if (IsGameOver || IsVictory) return;

        IsGameOver = true;
        onLose.Invoke();
        Time.timeScale = 0f;
        Debug.Log("GAME OVER");
    }

    public void TogglePause()
    {
        if (IsGameOver || IsVictory) return;

        IsPaused = !IsPaused;
        Time.timeScale = IsPaused ? 0f : 1f;
        pauseMenu.SetActive(IsPaused);
        Debug.Log(IsPaused ? "PAUSED" : "RESUMED");
    }

    public void RestartScene()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadScene(string sceneName)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private void Update()
{
    if (Keyboard.current == null) return;

    if (Keyboard.current.escapeKey.wasPressedThisFrame)
    {
        TogglePause();
    }

    /*
    if (Keyboard.current.kKey.wasPressedThisFrame)
    {
        RespawnManager.Instance.Respawn();
    }*/
}
}
