using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public abstract class ScrollviewContentItem<T> : MonoBehaviour {

    protected bool m_Initialized = false;
    protected System.Action<string,T> m_OnClickCallback;
    public delegate void ScrollviewContentItemClickedHandler(string eventStr, ScrollviewContentItem<T> scrollviewContentItem);
    public event ScrollviewContentItemClickedHandler OnItemClicked;

    protected T m_ContentItem;
	protected CanvasGroupRevealer m_Revealer; // optional revealer
	protected RectTransform m_RectTransform;
	// Scrollview content - parent of this content item.
	protected ScrollviewContent<T> m_ScrollviewContent;

	// Derived, concrete classes should use Init to populate content prefab with display information (name, icon, size, etc.)
	public virtual void Init()
	{
		m_RectTransform = GetComponent<RectTransform>();
		if (m_RectTransform == null)
			Debug.LogError("ScrollviewContentItem requires a Rect Transform component.");

		m_Revealer = GetComponent<CanvasGroupRevealer>(); // this is optional
		if (m_Revealer)
            m_Revealer.Init();
	}

	public CanvasGroupRevealer canvasGroupRevealer { get { return m_Revealer; }}
    public RectTransform RectTransform { get { return m_RectTransform; }}
    public virtual Vector2 ContentItemSize { get { return m_RectTransform.rect.size;}}


	public void SetScrollviewContent(ScrollviewContent<T> scrollviewContent)
	{
		m_ScrollviewContent = scrollviewContent;
	}

    public T ContentItem { get { return m_ContentItem; } }
    public void SetItem(T contentItem)
    {
        m_ContentItem = contentItem;
        (m_ContentItem as RenderableData<T>).OnValueChanged -= OnItemValueChanged;
        (m_ContentItem as RenderableData<T>).OnValueChanged += OnItemValueChanged;
        UpdateItemUI(m_ContentItem);
    }

    public void ClearItem()
    {
        (m_ContentItem as RenderableData<T>).OnValueChanged -= OnItemValueChanged;
        m_ContentItem = default(T);
    }

    public void SetOnClickCallback(System.Action<string, T> onClickCallback)
    {
        m_OnClickCallback = onClickCallback;
    }

    /*
     * Each of the Item buttons in the prefab have their OnClick() set to trigger this method with their appropriate index.
     */
    public void OnClickItem()
    {
        if (OnItemClicked != null)
            OnItemClicked("click", this);
    }

    protected void OnItemValueChanged(T item)
    {
        UpdateItemUI(item);
    }

    protected virtual void UpdateItemUI(T item)
    {
        if (m_Revealer && m_Revealer.IsHidden)
            m_Revealer.ShowPanel();
    }

    protected virtual void ClearItemUI()
    {
        if (m_Revealer && !m_Revealer.IsHidden)
            m_Revealer.HidePanel(true);
    }

}

public abstract class DraggableScrollviewContentItem<T> : ScrollviewContentItem<T>, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public delegate void ScrollviewContentItem_MonsterDraggedHandler(DraggableScrollviewContentItem<T> contentItem, PointerEventData eventData);
    public event ScrollviewContentItem_MonsterDraggedHandler OnDragBegin;
    public event ScrollviewContentItem_MonsterDraggedHandler OnDragEnd;
    public event ScrollviewContentItem_MonsterDraggedHandler OnDragging;

    public abstract void OnBeginDrag(PointerEventData eventData);
    public abstract void OnDrag(PointerEventData eventData);
    public abstract void OnEndDrag(PointerEventData eventData);

    protected void TriggerOnDragBegin(DraggableScrollviewContentItem<T> contentItem, PointerEventData eventData)
    {
        if (OnDragBegin != null)
            OnDragBegin(contentItem, eventData);
    }
    protected void TriggerOnDragEnd(DraggableScrollviewContentItem<T> contentItem, PointerEventData eventData)
    {
        if (OnDragEnd != null)
            OnDragEnd(contentItem, eventData);
    }
    protected void TriggerOnDragging(DraggableScrollviewContentItem<T> contentItem, PointerEventData eventData)
    {
        if (OnDragging != null)
            OnDragging(contentItem, eventData);
    }
}
