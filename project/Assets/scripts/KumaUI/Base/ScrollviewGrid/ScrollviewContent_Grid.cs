using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public abstract class ScrollviewContent_Grid<T> : ScrollviewContent<T>
{
    protected List<T> m_AddedItems; // added items.
    protected List<T> m_QueuedItems; // queue to add to items at a throttled rate (performance).

    protected GameObject m_ItemPrefab; // prefab from which to generate items.
    protected List<ScrollviewContentItem<T>> m_ItemPrefabInstances; //generated item rows prefabs.

    protected float ADD_ITEMS_PER_SEC = 75; // rate at which items will be added from m_AddItemQueue to m_StorableItems.
    protected float timeSinceLastAdd = 0.0f; //used for add item throttling (performance).

    public delegate void ScrollviewContentItemClickedHandler(string eventStr, ScrollviewContentItem<T> contentItem);
    public event ScrollviewContentItemClickedHandler OnItemClicked;
    public delegate void ScrollviewContentGridItemDraggedHandler(DraggableScrollviewContentItem<T> contentItem, PointerEventData eventData);
    public event ScrollviewContentGridItemDraggedHandler OnItemDragBegin;
    public event ScrollviewContentGridItemDraggedHandler OnItemDragEnd;
    public event ScrollviewContentGridItemDraggedHandler OnItemDragging;

	void Start()
	{
		Init ();
	}

    protected override void Init()
    {
        base.Init();

        m_QueuedItems = new List<T>();
        m_AddedItems = new List<T>();
        m_ItemPrefabInstances = new List<ScrollviewContentItem<T>>();
    }

    protected void CreateEnoughItems(int targetCount)
    {
        while (m_ItemPrefabInstances.Count < targetCount)
        {
            GameObject neoObj = GameObject.Instantiate(m_ItemPrefab as UnityEngine.GameObject);
            ScrollviewContentItem<T> neoItem = neoObj.GetComponent<ScrollviewContentItem<T>>();

            neoItem.Init();
            neoItem.OnItemClicked += ItemClicked;

            if (neoItem is DraggableScrollviewContentItem<T>)
            {
                DraggableScrollviewContentItem<T> draggableItem = neoItem as DraggableScrollviewContentItem<T>;
                if (draggableItem != null)
                {
                    draggableItem.OnDragBegin += ItemDragBegin;
                    draggableItem.OnDragEnd += ItemDragEnd;
                    draggableItem.OnDragging += ItemDragging;
                }
            }
                
            neoItem.gameObject.SetActive(false);
            base.AddContentItem(neoItem);
            m_ItemPrefabInstances.Add(neoItem);
        }
    }

    public override void ClearContentItems()
    {
        base.ClearContentItems();

        foreach (ScrollviewContentItem<T> item in m_ItemPrefabInstances)
            Destroy(item.gameObject);

        m_ItemPrefabInstances.Clear();
        m_AddedItems.Clear();
        m_QueuedItems.Clear();
    }
    public void AddItem(T item)
    {
        m_QueuedItems.Add(item);
    }

    protected void AddItemFromQueue(int itemCnt)
    {
        int itemsAdded = 0;
        while (m_QueuedItems.Count > 0 && itemsAdded < itemCnt)
        {
            itemsAdded++;

            T item = m_QueuedItems[0];
            m_QueuedItems.RemoveAt(0);
            DoAddItem(item);
        }
    }

    protected void DoAddItem(T item)
    {
        int neoCount = m_AddedItems.Count + 1;

        m_AddedItems.Add(item);

        // Create enough item rows to add the new item.
        CreateEnoughItems(neoCount);

        // Set the item
        m_ItemPrefabInstances[neoCount - 1].SetItem(item);
    }

    protected void ItemClicked(string eventStr, ScrollviewContentItem<T> item)
    {
        if (OnItemClicked != null)
            OnItemClicked(eventStr, item);
    }

    protected void ItemDragBegin(DraggableScrollviewContentItem<T> contentItem, PointerEventData eventData)
    {
        if (OnItemDragBegin != null)
            OnItemDragBegin(contentItem, eventData);
    }

    protected void ItemDragEnd(DraggableScrollviewContentItem<T> contentItem, PointerEventData eventData)
    {
        if (OnItemDragEnd != null)
            OnItemDragEnd(contentItem, eventData);
    }

    protected void ItemDragging(DraggableScrollviewContentItem<T> contentItem, PointerEventData eventData)
    {
        if (OnItemDragging != null)
            OnItemDragging(contentItem, eventData);
    }

    protected void CheckAddItemFromQueue()
    {
        if (m_QueuedItems.Count > 0)
        {
            timeSinceLastAdd += Time.unscaledDeltaTime;
            float itemCnt = ADD_ITEMS_PER_SEC * timeSinceLastAdd;
            if (itemCnt > 1.0f)
            {
                timeSinceLastAdd -= ((int)itemCnt) / ADD_ITEMS_PER_SEC;
                AddItemFromQueue((int)itemCnt);
            }
        }
    }

    protected override void DoUpdate()
    {
        base.DoUpdate();
        CheckAddItemFromQueue();
    }
}
