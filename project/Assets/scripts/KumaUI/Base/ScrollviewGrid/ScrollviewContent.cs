using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public abstract class ScrollviewContent<T> : MonoBehaviour {

    public int m_ItemsPerLine = 1; // Number of items per row (or column).
    public int m_TrailingPixels = 0; // Number of extra pixels to add to rect size.
    public bool m_StartAtEnd = false;
    // public SpinnerController m_Spinner;

    protected ScrollRect m_ParentScrollRect;
    protected RectTransform m_ParentScrollRectTransform;
    protected RectTransform m_RectTransform;
    protected List<ScrollviewContentItem<T>> m_Items;
    protected bool m_RepositionRequired = false;
    protected bool m_ResetScrollOnReposition = false;
    protected float m_LastAnchoredPositionY = 0;

    protected Vector2 m_TargetSize;
    protected float m_resizeTime;

	// Use this for initialization
	void Awake () 
    {
		Init();
	}
	
	protected virtual void Init()
	{
        m_RectTransform = GetComponent<RectTransform>();
        if (m_RectTransform == null)
            Debug.LogError("ScrollviewContent requires a Rect Transform component.");
        m_TargetSize = m_RectTransform.sizeDelta;

        m_ParentScrollRect = GetComponentInParent<ScrollRect>();
        if (m_ParentScrollRect == null)
            Debug.LogError("ScrollviewContent should be the child of a ScrollRect.");

        m_ParentScrollRectTransform = m_ParentScrollRect.GetComponent<RectTransform>();

		m_Items = new List<ScrollviewContentItem<T>>();
	}

    public int ItemsPerRow
    {
        get { return m_ItemsPerLine; }
        set { m_ItemsPerLine = value; }
    }

	// Update is called once per frame
	void Update () {
        DoUpdate();
    }

    protected virtual void DoUpdate()
    {
        if (m_RepositionRequired)
            DoRepositionContentItems(m_ResetScrollOnReposition);

        if (m_RectTransform.sizeDelta.x != m_TargetSize.x)
        {
            float neoWidth = Mathf.Lerp(m_RectTransform.sizeDelta.x, m_TargetSize.x, Time.unscaledTime - m_resizeTime);
            m_RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, neoWidth);

            //if (neoWidth == m_TargetSize.x)
                LayoutContentItemsHorizontal();
        }
        if (m_RectTransform.sizeDelta.y != m_TargetSize.y)
        {
            float neoHeight = Mathf.Lerp(m_RectTransform.sizeDelta.y, m_TargetSize.y, Time.unscaledTime - m_resizeTime);
            m_RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, neoHeight);

            //if (neoHeight == m_TargetSize.y)
                LayoutContentItemsVertical();
        }
        //DeactivateOffscreenContent();
	}
	/*
    protected void DeactivateOffscreenContent()
    {
        // This seems to be quite expensive.
        return;
        
        if (m_RectTransform.anchoredPosition.y != m_LastAnchoredPositionY)
        {
            m_LastAnchoredPositionY = m_RectTransform.anchoredPosition.y;

            Vector2 contentRectHalfSize = new Vector2(m_RectTransform.rect.width / 2.0f, m_RectTransform.rect.height / 2.0f);
            foreach (ScrollviewContentItem<T> item in m_Items)
            {
                //Debug.LogError(
                //    " itemY = " + item.RectTransform.anchoredPosition.y
                //    + " m_LastAnchoredPositionY = " + m_LastAnchoredPositionY
                //    + " m_ParentScrollRectTransform.anchorMax.y = " + m_ParentScrollRectTransform.anchoredPosition.y
                //    );
				if ((item.RectTransform.anchoredPosition.y + m_LastAnchoredPositionY + contentRectHalfSize.y) < m_ParentScrollRectTransform.rect.y  ||
				    (item.RectTransform.anchoredPosition.y - item.RectTransform.rect.height + m_LastAnchoredPositionY + contentRectHalfSize.y) > m_ParentScrollRectTransform.rect.yMax)
                {
                    if (item.gameObject.activeSelf)
                        item.gameObject.SetActive(false);
                }
                else
                {
                    if (item.gameObject.activeSelf == false)
                        item.gameObject.SetActive(true);
                }
            }
        }
    }
*/
	public virtual void ClearContentItems()
	{
        m_Items.Clear();

        RepositionContentItems();
	}

    public void ShowSpinner()
    {
        // if (m_Spinner && m_Spinner.enabled == false)
        // {
        //     m_Spinner.enabled = true;
        //     m_Spinner.Show();
        // }
    }

    public void AddContentItem(ScrollviewContentItem<T> contentItem)
	{
		// if (m_Spinner && m_Spinner.enabled){
		// 	m_Spinner.enabled = false;
		// 	m_Spinner.Hide();
		// }
		
		contentItem.SetScrollviewContent(this);
        contentItem.RectTransform.SetParent(m_RectTransform, false);
        contentItem.RectTransform.anchoredPosition = new Vector3(0, 0, 0);
        contentItem.RectTransform.localScale = new Vector3(1, 1, 1);

        m_Items.Add(contentItem);
        RepositionContentItems();
	}

	public void RemoveContentItem(ScrollviewContentItem<T> contentItem)
	{
        m_Items.Remove(contentItem);
        RepositionContentItems();
	}

    public void RepositionContentItems(bool resetScroll = false)
    {
        m_RepositionRequired = true;
        m_ResetScrollOnReposition |= resetScroll;
    }

    public void DoRepositionContentItems(bool resetScroll)
	{
        if (m_ParentScrollRect.horizontal && m_ParentScrollRect.vertical)
            Debug.LogError("KumaUI's ScrollviewContent only supports one dimensional scrolling! You've been warned!");

        if (m_ParentScrollRect.horizontal)
            RepositionContentItemsHorizontal();
        else if (m_ParentScrollRect.vertical)
            RepositionContentItemsVertical();

        if (resetScroll)
            ResetScroll();

        m_RepositionRequired = false;
        m_ResetScrollOnReposition = false;
    }

    protected void RepositionContentItemsHorizontal()
    {
        float totalWidth = CalcTotalWidth();
        m_TargetSize.x = totalWidth;
        m_resizeTime = Time.unscaledTime;
        //m_RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, totalWidth); 
        LayoutContentItemsHorizontal();
    }

    protected void LayoutContentItemsHorizontal()
    {
        float currentX = 0;
        float currentY = 0;

        for (int i = 0; i < m_Items.Count; i++)
        {
            ScrollviewContentItem<T> item = m_Items[i];
            item.RectTransform.anchoredPosition = new Vector2(currentX, currentY);
			// Explicitly update position for revealer as well.
			if (item.canvasGroupRevealer != null) 
				item.canvasGroupRevealer.UpdateOriginalPosition();

            if (item.gameObject.activeSelf == false)
                item.gameObject.SetActive(true);

            currentY -= item.ContentItemSize.y;

            if ((i % m_ItemsPerLine) == (m_ItemsPerLine-1))
            {
                currentY = 0;
                currentX += CalcColWidthAtIndex(i);
            }
        }
    }

    protected void RepositionContentItemsVertical()
    {
        float totalHeight = CalcTotalHeight();
        m_TargetSize.y = totalHeight;
        m_resizeTime = Time.unscaledTime;
        //m_RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, totalHeight);
        LayoutContentItemsVertical();
    }

    protected void LayoutContentItemsVertical()
    {
		// Vertical ScrollviewContentItems should be anchored to their top.
		float currentY = 0;
        float currentX = 0;
        
        for (int i = 0; i < m_Items.Count; i++)
        {
            ScrollviewContentItem<T> item = m_Items[i];
			item.RectTransform.anchoredPosition = new Vector2(currentX, currentY);
			// Explicitly update position for revealer as well.
			if (item.canvasGroupRevealer != null) 
				item.canvasGroupRevealer.UpdateOriginalPosition();

            if (item.gameObject.activeSelf == false)
                item.gameObject.SetActive(true);

            currentX += item.ContentItemSize.x;

            if ((i % m_ItemsPerLine) == (m_ItemsPerLine-1))
            {
                currentX = 0;
                currentY -= CalcRowHeightAtIndex(i);
            }
        }
    }

    protected float CalcColWidthAtIndex(int index)
    {
        float colWidth = 0;
        int i = (index / m_ItemsPerLine) * m_ItemsPerLine; // column start index.

        for (int j = 0; j < m_ItemsPerLine; j++)
            if (i + j < m_Items.Count && m_Items[i + j].ContentItemSize.x > colWidth)
                colWidth = m_Items[i + j].ContentItemSize.x;

        return colWidth;
    }

    protected float CalcRowHeightAtIndex(int index)
    {
        float rowHeight = 0;
        int i = (index / m_ItemsPerLine) * m_ItemsPerLine; // row start index.

        for (int j = 0; j < m_ItemsPerLine; j++)
            if (i + j < m_Items.Count && m_Items[i + j].ContentItemSize.y > rowHeight)
                rowHeight = m_Items[i + j].ContentItemSize.y;

        return rowHeight;
    }

    protected float CalcTotalWidth()
    {
        float totalWidth = 0.0f;
        if (m_Items.Count > 0)
        {
            if (m_Items[0].ContentItemSize.x == 0)
                Debug.LogError("RepositionContentItemsHorizontal: m_Items[0].ContentItemSize.x == 0");

            // Calculate total width, determining each column's max width.
            for (int i = 0; i < m_Items.Count; i++)
                if (i % m_ItemsPerLine == 0)
                    totalWidth += CalcColWidthAtIndex(i);

            totalWidth += m_TrailingPixels;
        }

        return totalWidth;
    }

    protected float CalcTotalHeight()
    {
        float totalHeight = 0.0f;
        if (m_Items.Count > 0)
        {
            for (int i = 0; i < m_Items.Count; i++)
                if (i % m_ItemsPerLine == 0)
                    totalHeight += CalcRowHeightAtIndex(i);

            totalHeight += m_TrailingPixels;
        }
        return totalHeight;
    }

    // Move content to beginning (top-left corner of content)
	public void ResetScroll()
    {
        Vector2 neoPos = m_ParentScrollRect.normalizedPosition;
        if (m_StartAtEnd)
        {
            if (m_ParentScrollRect.horizontal)
                neoPos.x = 0.0f;

            if (m_ParentScrollRect.vertical)
                neoPos.y = 0.0f;
        }
        else
        {
            if (m_ParentScrollRect.horizontal)
                neoPos.x = 1.0f;

            if (m_ParentScrollRect.vertical)
                neoPos.y = 1.0f;
        }

        m_ParentScrollRect.normalizedPosition = neoPos;
    }

    public IList<ScrollviewContentItem<T>> Items
    {
        get { return m_Items.AsReadOnly(); }
    }
}
