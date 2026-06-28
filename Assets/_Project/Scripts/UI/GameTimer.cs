using TMPro;
using UnityEngine;


public class GameTimer : MonoBehaviour
{
    [Tooltip("The TextMeshPro label that displays the timer.")]
    [SerializeField] private TMP_Text m_label;

    [Tooltip("Show as MM:SS when true, otherwise show raw seconds.")]
    [SerializeField] private bool m_useMinutesFormat = true;

    [Tooltip("Start counting automatically on Start.")]
    [SerializeField] private bool m_autoStart = true;

    private float m_elapsed;
    private int m_lastShownSecond = -1;
    private bool m_running;

    private void Start()
    {
        if (m_autoStart) StartTimer();
    }

    public void StartTimer()
    {
        m_elapsed = 0f;
        m_lastShownSecond = -1;
        m_running = true;
        UpdateLabel(0); // show 0 immediately
    }

    public void StopTimer()
    {
        m_running = false;
    }

    /// <summary>Elapsed time in whole seconds (useful for scoring).</summary>
    public int ElapsedSeconds => Mathf.FloorToInt(m_elapsed);

    private void Update()
    {
        if (!m_running) return;

        m_elapsed += Time.deltaTime;

        int currentSecond = Mathf.FloorToInt(m_elapsed);
        if (currentSecond != m_lastShownSecond)
        {
            m_lastShownSecond = currentSecond;
            UpdateLabel(currentSecond);
        }
    }

    private void UpdateLabel(int totalSeconds)
    {
        if (m_label == null) return;

        if (m_useMinutesFormat)
        {
            int minutes = totalSeconds / 60;
            int seconds = totalSeconds % 60;
            m_label.text = $"{minutes:00}:{seconds:00}";
        }
        else
        {
            m_label.text = totalSeconds.ToString();
        }
    }
}
