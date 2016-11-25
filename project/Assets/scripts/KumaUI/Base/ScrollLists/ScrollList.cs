using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ScrollList : MonoBehaviour {

    public ScrollListItem sampleItem;

    public List<ScrollListItem> itemsList = new List<ScrollListItem>();

    public RectTransform scrollContentGroup;

    public float distanceBetweenItems;
    public bool isVerticalAlignment;

	// Use this for initialization
	void Start () {
        InitTheList();

    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void InitTheList()
    {
        if (sampleItem != null)
        {
            float _deltaPositioning = 0.0f;
            int _tempItemsCount = sampleItem.parentListCapacity;

            Vector2 _tempContentSize;
            _tempContentSize = scrollContentGroup.sizeDelta;
            if (!isVerticalAlignment)
            {
                _tempContentSize.x += (sampleItem.GetComponent<RectTransform>().rect.size.x * (_tempItemsCount - 1)) + (distanceBetweenItems * (_tempItemsCount - 1));
            }
            else
            {
                _tempContentSize.y += (sampleItem.GetComponent<RectTransform>().rect.size.y * (_tempItemsCount - 1)) + (distanceBetweenItems * (_tempItemsCount - 1));
            }
            scrollContentGroup.sizeDelta = _tempContentSize;

            for (int c=0; c< _tempItemsCount; c++)
            {
                GameObject _tempObject;
                _tempObject = (GameObject)Instantiate(sampleItem.gameObject, sampleItem.transform.position, sampleItem.transform.rotation);
                _tempObject.transform.SetParent(sampleItem.gameObject.transform.parent);
                _tempObject.name = "_" + ((c+1).ToString());

                RectTransform _tempRect;
                _tempRect = _tempObject.GetComponent<RectTransform>();
                _tempRect.localScale = new Vector3(1, 1, 1);
                _tempObject.GetComponent<ScrollListItem>().OnUpdateInfo(c);

                Vector3 _tempPosition;
                _tempPosition = _tempRect.localPosition;
                if ( c!= 0)
                {
                    _deltaPositioning += distanceBetweenItems;
                    if (!isVerticalAlignment)
                    {
                        _deltaPositioning += _tempRect.rect.size.x;
                    }
                    else
                    {
                        _deltaPositioning += _tempRect.rect.size.y;
                    }
                }
                if (!isVerticalAlignment)
                {
                    _tempPosition.x += _deltaPositioning;
                }
                else
                {
                    _tempPosition.y -= _deltaPositioning;
                }
                _tempRect.localPosition = _tempPosition;
            }
            sampleItem.gameObject.SetActive(false);
        }
    }
}
