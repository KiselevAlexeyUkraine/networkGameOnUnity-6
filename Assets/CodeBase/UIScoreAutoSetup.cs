using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class UIScoreAutoSetup : NetworkBehaviour
{
    public GameObject scoreRowPrefab;
    private Transform scorePanel;

    void Awake()
    {
        // Setup Canvas
        GameObject canvasObj = new GameObject("Canvas", typeof(Canvas));
        var canvas = canvasObj.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasObj.AddComponent<CanvasScaler>();
        canvasObj.AddComponent<GraphicRaycaster>();

        // Create ScorePanel
        GameObject panelObj = new GameObject("ScorePanel", typeof(RectTransform), typeof(VerticalLayoutGroup), typeof(ContentSizeFitter));
        panelObj.transform.SetParent(canvasObj.transform);

        var layout = panelObj.GetComponent<VerticalLayoutGroup>();
        layout.childControlHeight = true;
        layout.childControlWidth = true;
        layout.childForceExpandHeight = false;
        layout.childForceExpandWidth = false;

        var fitter = panelObj.GetComponent<ContentSizeFitter>();
        fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

        var rt = panelObj.GetComponent<RectTransform>();
        rt.anchorMin = new Vector2(1, 1);
        rt.anchorMax = new Vector2(1, 1);
        rt.pivot = new Vector2(1, 1);
        rt.anchoredPosition = new Vector2(-20, -20);
        rt.sizeDelta = new Vector2(300, 400);

        scorePanel = panelObj.transform;

        // Create default prefab if not assigned
        if (scoreRowPrefab == null)
            scoreRowPrefab = CreateRowPrefab();

        GameManager.Instance.scorePanel = scorePanel;
        GameManager.Instance.scoreRowPrefab = scoreRowPrefab;
    }

    GameObject CreateRowPrefab()
    {
        var row = new GameObject("ScoreRow", typeof(RectTransform), typeof(HorizontalLayoutGroup));
        var rowRt = row.GetComponent<RectTransform>();
        rowRt.sizeDelta = new Vector2(300, 30);

        string[] names = { "Player", "Kills", "Deaths" };
        foreach (var n in names)
        {
            var go = new GameObject(n + "Text", typeof(RectTransform), typeof(TextMeshProUGUI));
            go.transform.SetParent(row.transform);
            var txt = go.GetComponent<TextMeshProUGUI>();
            txt.fontSize = 16;
            txt.alignment = TextAlignmentOptions.Center;
            txt.text = n;

            var rt = go.GetComponent<RectTransform>();
            rt.sizeDelta = new Vector2(100, 30);
        }

        row.transform.SetParent(null);
        return row;
    }
}
