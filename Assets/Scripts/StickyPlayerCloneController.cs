using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ScrollRect))]
public class StickyPlayerCloneController : MonoBehaviour
{
    [Header("Required References")]
    [SerializeField] private RectTransform viewport;
    [SerializeField] private RectTransform content;
    [SerializeField] private RectTransform stickyContainer;

    [Header("My Player Reference")]
    [SerializeField] private LeaderboardItem myPlayerItem;

    private ScrollRect _scrollRect;
    private RectTransform _myPlayerRect;
    private GameObject _cloneInstance;

    private RectTransform MyPlayerRect
    {
        get
        {
            if (_myPlayerRect == null && myPlayerItem != null)
                _myPlayerRect = myPlayerItem.GetComponent<RectTransform>();
            return _myPlayerRect;
        }
    }

    private void Awake()
    {
        _scrollRect = GetComponent<ScrollRect>();
    }

    private void OnEnable()
    {
        _scrollRect.onValueChanged.AddListener(OnScroll);
        LeaderboardItem.OnMyPlayerChanged += HandleMyPlayerChanged;
        Canvas.willRenderCanvases += UpdateStickyStateOnceAfterRender;
    }

    private void OnDisable()
    {
        _scrollRect.onValueChanged.RemoveListener(OnScroll);
        LeaderboardItem.OnMyPlayerChanged -= HandleMyPlayerChanged;
        Canvas.willRenderCanvases -= UpdateStickyStateOnceAfterRender;
        DestroyStickyClone();
    }

    private void UpdateStickyStateOnceAfterRender()
    {
        UpdateStickyState();
        Canvas.willRenderCanvases -= UpdateStickyStateOnceAfterRender;
    }

    private void HandleMyPlayerChanged(LeaderboardItem newPlayer)
    {
        myPlayerItem = newPlayer;
        _myPlayerRect = null;
        DestroyStickyClone();
        UpdateStickyState();
    }

    private void OnScroll(Vector2 _) => UpdateStickyState();

    private void UpdateStickyState()
    {
        if (MyPlayerRect == null) return;

        var visibility = GetVisibilityState(MyPlayerRect, viewport);

        if (visibility == VisibilityState.Visible)
        {
            DestroyStickyClone();
        }
        else if (_cloneInstance == null)
        {
            CreateStickyClone(visibility);
        }
    }

    private enum VisibilityState { Visible, Above, Below }

    private VisibilityState GetVisibilityState(RectTransform item, RectTransform viewport)
    {
        var itemCorners = new Vector3[4];
        var viewportCorners = new Vector3[4];

        item.GetWorldCorners(itemCorners);
        viewport.GetWorldCorners(viewportCorners);

        float itemTop = itemCorners[1].y;
        float itemBottom = itemCorners[0].y;
        float viewportTop = viewportCorners[1].y;
        float viewportBottom = viewportCorners[0].y;

        return itemTop > viewportTop ? VisibilityState.Above
             : itemBottom < viewportBottom ? VisibilityState.Below
             : VisibilityState.Visible;
    }

    private void CreateStickyClone(VisibilityState side)
    {
        _cloneInstance = Instantiate(MyPlayerRect.gameObject, stickyContainer);
        var rt = _cloneInstance.GetComponent<RectTransform>();

        ConfigureCloneAnchors(rt, side);
        DisableCloneInteractions();
    }

    private void ConfigureCloneAnchors(RectTransform rt, VisibilityState side)
    {
        rt.anchorMin = new Vector2(0, side == VisibilityState.Above ? 1 : 0);
        rt.anchorMax = new Vector2(1, side == VisibilityState.Above ? 1 : 0);
        rt.pivot = new Vector2(0.5f, side == VisibilityState.Above ? 1 : 0);
        rt.anchoredPosition = Vector2.zero;
        rt.sizeDelta = new Vector2(0, rt.sizeDelta.y);

        if (_cloneInstance.TryGetComponent<LayoutElement>(out var layout))
            layout.ignoreLayout = true;
    }

    private void DisableCloneInteractions()
    {
        foreach (var graphic in _cloneInstance.GetComponentsInChildren<Graphic>())
        {
            graphic.raycastTarget = false;
        }
    }

    private void DestroyStickyClone()
    {
        if (_cloneInstance == null) return;

        Destroy(_cloneInstance);
        _cloneInstance = null;
    }
}