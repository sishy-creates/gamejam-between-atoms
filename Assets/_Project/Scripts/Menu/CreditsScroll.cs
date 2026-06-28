using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


public class CreditsScroll : MonoBehaviour
{
    [SerializeField] private RectTransform m_content;

    [SerializeField] private float m_scrollSpeed = 60f;

    [SerializeField] private bool m_loop = false;

    public UnityEvent onFinished;

    private float m_startY;
    private float m_endY;
    private bool m_finished;

    public void Setup(RectTransform content, float scrollSpeed, bool loop)
    {
        m_content = content;
        m_scrollSpeed = scrollSpeed;
        m_loop = loop;
        if (isActiveAndEnabled) ResetScroll();
    }

    private void OnEnable()
    {
        ResetScroll();
    }

    private void ResetScroll()
    {
        if (m_content == null) return;

        LayoutRebuilder.ForceRebuildLayoutImmediate(m_content);

        RectTransform viewport = (RectTransform)transform;
        float contentHeight = m_content.rect.height;
        float viewportHeight = viewport.rect.height;

        m_startY = -(viewportHeight * 0.5f) - (contentHeight * 0.5f);
        m_endY = (viewportHeight * 0.5f) + (contentHeight * 0.5f);

        m_content.anchoredPosition = new Vector2(m_content.anchoredPosition.x, m_startY);
        m_finished = false;
    }

    private void Update()
    {
        if (m_finished || m_content == null) return;

        Vector2 pos = m_content.anchoredPosition;
        pos.y += m_scrollSpeed * Time.deltaTime;
        m_content.anchoredPosition = pos;

        if (pos.y >= m_endY)
        {
            if (m_loop)
            {
                pos.y = m_startY;
                m_content.anchoredPosition = pos;
            }
            else
            {
                m_finished = true;
                onFinished?.Invoke();
            }
        }
    }
}
