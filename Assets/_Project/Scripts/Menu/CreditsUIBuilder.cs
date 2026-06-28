using UnityEngine;
using UnityEngine.UI;


public class CreditsUIBuilder : MonoBehaviour
{
    [Tooltip("Righe dei crediti, una per elemento.")]
    [TextArea(3, 20)]
    [SerializeField]
    private string m_creditsText =
        "CREDITS\n\n\n" +
        "Game Design\nFilippo\nCalogero \n\n" +
        "Programming\nSilvia\nMatteo\nFilippo\nAntonio\nAlessandro\nCalogero\n\n" +
        "Art\nCalogero\nAntonio\n\n" +
        "Audio\nAlessandro\n\n\n" +
        "Thanks for playing!";

    [SerializeField] private float m_scrollSpeed = 60f;
    [SerializeField] private bool m_loop = false;
    [SerializeField] private int m_fontSize = 36;
    [SerializeField] private Color m_textColor = Color.white;
    [SerializeField] private Color m_backgroundColor = new Color(0f, 0f, 0f, 0.85f);

    [Tooltip("Main menu objects (buttons, title...) to hide while credits are open.")]
    [SerializeField] private GameObject[] m_objectsToHideWhileOpen;

    private GameObject m_root;

    private void Awake()
    {
        Build();
        m_root.SetActive(false); 
    }

    public void Open()
    {
        if (m_root != null) m_root.SetActive(true);
        SetMainUIVisible(false); // hide the main menu buttons behind the credits
    }

    public void Close()
    {
        if (m_root != null) m_root.SetActive(false);
        SetMainUIVisible(true); // bring the main menu buttons back
    }

    // Toggle the main menu objects assigned in the inspector.
    private void SetMainUIVisible(bool visible)
    {
        if (m_objectsToHideWhileOpen == null) return;
        foreach (GameObject go in m_objectsToHideWhileOpen)
        {
            if (go != null) go.SetActive(visible);
        }
    }

    private void Build()
    {
        Font builtinFont = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");

        // --- Canvas root ---
        m_root = new GameObject("CreditsCanvas",
            typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
        Canvas canvas = m_root.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 100;
        CanvasScaler scaler = m_root.GetComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);

        // --- Background ---
        GameObject background = CreateChild("Background", m_root.transform);
        RectTransform bgRect = (RectTransform)background.transform;
        Stretch(bgRect);
        Image bgImage = background.AddComponent<Image>();
        bgImage.color = m_backgroundColor;

        // --- Viewport masked ---
        GameObject viewport = CreateChild("Viewport", background.transform);
        RectTransform viewportRect = (RectTransform)viewport.transform;
        viewportRect.anchorMin = new Vector2(0.5f, 0.5f);
        viewportRect.anchorMax = new Vector2(0.5f, 0.5f);
        viewportRect.pivot = new Vector2(0.5f, 0.5f);
        viewportRect.sizeDelta = new Vector2(1200, 800);
        viewportRect.anchoredPosition = new Vector2(0, 40);
        viewport.AddComponent<RectMask2D>();

        // --- Content scorrevole ---
        GameObject content = CreateChild("Content", viewport.transform);
        RectTransform contentRect = (RectTransform)content.transform;
        contentRect.anchorMin = new Vector2(0.5f, 0.5f);
        contentRect.anchorMax = new Vector2(0.5f, 0.5f);
        contentRect.pivot = new Vector2(0.5f, 0.5f);
        contentRect.sizeDelta = new Vector2(1100, 0);

        Text text = content.AddComponent<Text>();
        text.font = builtinFont;
        text.text = m_creditsText;
        text.fontSize = m_fontSize;
        text.color = m_textColor;
        text.alignment = TextAnchor.UpperCenter;
        text.horizontalOverflow = HorizontalWrapMode.Wrap;
        text.verticalOverflow = VerticalWrapMode.Overflow;

        ContentSizeFitter fitter = content.AddComponent<ContentSizeFitter>();
        fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

        // --- scrolling ---
        CreditsScroll scroll = viewport.AddComponent<CreditsScroll>();
        scroll.Setup(contentRect, m_scrollSpeed, m_loop);

        // --- Btn Back ---
        GameObject backBtn = CreateChild("BackButton", background.transform);
        RectTransform backRect = (RectTransform)backBtn.transform;
        backRect.anchorMin = new Vector2(0.5f, 0f);
        backRect.anchorMax = new Vector2(0.5f, 0f);
        backRect.pivot = new Vector2(0.5f, 0f);
        backRect.sizeDelta = new Vector2(280, 80);
        backRect.anchoredPosition = new Vector2(0, 40);
        Image backImg = backBtn.AddComponent<Image>();
        backImg.color = new Color(0.2f, 0.2f, 0.2f, 1f);
        Button button = backBtn.AddComponent<Button>();
        button.onClick.AddListener(Close);

        GameObject backLabel = CreateChild("Label", backBtn.transform);
        RectTransform backLabelRect = (RectTransform)backLabel.transform;
        Stretch(backLabelRect);
        Text backText = backLabel.AddComponent<Text>();
        backText.font = builtinFont;
        backText.text = "Back";
        backText.fontSize = 32;
        backText.color = Color.white;
        backText.alignment = TextAnchor.MiddleCenter;
    }

    private static GameObject CreateChild(string name, Transform parent)
    {
        GameObject go = new GameObject(name, typeof(RectTransform));
        go.transform.SetParent(parent, false);
        return go;
    }

    private static void Stretch(RectTransform rect)
    {
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;
    }
}
