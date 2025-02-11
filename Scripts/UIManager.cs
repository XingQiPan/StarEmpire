using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 3. UI管理类
public class UIManager : MonoBehaviour
{
    private static UIManager _instance;
    public static UIManager Instance => _instance;

    // 层级父节点
    private Dictionary<UILayer, Transform> layerParents = new Dictionary<UILayer, Transform>();
    // 面板缓存
    private Dictionary<string, BasePanel> panelCache = new Dictionary<string, BasePanel>();
    // 当前显示的面板栈
    private Stack<BasePanel> panelStack = new Stack<BasePanel>();

    [SerializeField] private Transform canvasTransform;
    [SerializeField] private List<UILayerConfig> layerConfigs;

    [System.Serializable]
    public class UILayerConfig
    {
        public UILayer layer;
        public Transform parent;
        public int order;
    }

    private void Awake()
    {
        _instance = this;
        InitializeLayers();
    }

    private void InitializeLayers()
    {
        foreach (var config in layerConfigs)
        {
            layerParents[config.layer] = config.parent;
        }
    }

    public void ShowPanel<T>(string panelPath, UILayer layer, object args = null) where T : BasePanel
    {
        if (panelCache.TryGetValue(panelPath, out BasePanel panel))
        {
            ShowExistingPanel(panel as T, args);
        }
        else
        {
            StartCoroutine(LoadPanelAsync<T>(panelPath, layer, args));
        }
    }

    private IEnumerator LoadPanelAsync<T>(string panelPath, UILayer layer, object args) where T : BasePanel
    {
        var request = Resources.LoadAsync<GameObject>(panelPath);
        yield return request;

        if (request.asset == null)
        {
            Debug.LogError($"Failed to load panel: {panelPath}");
            yield break;
        }

        GameObject panelObj = Instantiate(request.asset as GameObject);
        T panel = panelObj.GetComponent<T>();
        panel.Layer = layer;
        
        Transform parent = layerParents[layer];
        panelObj.transform.SetParent(parent, false);
        panelObj.transform.localPosition = Vector3.zero;
        panelObj.transform.localScale = Vector3.one;

        panelCache[panelPath] = panel;
        ShowExistingPanel(panel, args);
    }

    private void ShowExistingPanel<T>(T panel, object args) where T : BasePanel
    {
        if (panelStack.Count > 0)
        {
            var topPanel = panelStack.Peek();
            if (topPanel.Layer <= panel.Layer)
            {
                topPanel.OnPause();
            }
        }

        panelStack.Push(panel);
        panel.OnEnter(args);
    }

    public void HidePanel()
    {
        if (panelStack.Count == 0) return;

        var panel = panelStack.Pop();
        panel.OnExit();

        if (panelStack.Count > 0)
        {
            var previousPanel = panelStack.Peek();
            previousPanel.OnResume();
        }
    }
}