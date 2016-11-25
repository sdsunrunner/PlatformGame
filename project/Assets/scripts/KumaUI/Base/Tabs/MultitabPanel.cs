using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MultitabPanel : MonoBehaviour {

    public RectTransform[] AvaliableTabs;
    public RectTransform DefaultEnabledTab;

    public Sprite ActiveTabSprite;
    public Sprite InactiveTabSprite;

    public bool canOffset;
    public float ActivityPositioningOffset;
    public float InactiveTabsDefaultPosition;

    private RectTransform currentActiveTab;

    void Awake()
    {

    }

    
    void Start ()
    {
        OnSetActiveTab(AvaliableTabs[0]);
    }
	
	
	void Update ()
    {
	
	}

    void OnResetAllTabs()
    {
        /*
        if (currentActiveTab != null && currentActiveTab.gameObject.GetComponent<SingleTabElement>().hasDisplayContent)
        {
            currentActiveTab.gameObject.GetComponent<SingleTabElement>().displayContent.gameObject.SetActive(false);
        }
        */

        for (int c=0; c<AvaliableTabs.Length; c++)
        {
            AvaliableTabs[c].GetComponent<Image>().sprite = InactiveTabSprite;
            Vector3 _tempPosition;
            _tempPosition = AvaliableTabs[c].localPosition;
            _tempPosition.y = InactiveTabsDefaultPosition;
            AvaliableTabs[c].localPosition = _tempPosition;

            GameObject _tempDisplayContent;
            if (AvaliableTabs[c].gameObject.GetComponent<SingleTabElement>().hasDisplayContent)
            {
                _tempDisplayContent = AvaliableTabs[c].gameObject.GetComponent<SingleTabElement>().displayContent;
                _tempDisplayContent.gameObject.SetActive(false);
            }
        }
    }

    public void OnSetActiveTab(RectTransform SelectedTab)
    {
        OnResetAllTabs();
        currentActiveTab = SelectedTab;
        SelectedTab.GetComponent<Image>().sprite = ActiveTabSprite;
        Vector3 _tempPosition;
        _tempPosition = SelectedTab.localPosition;
        if (canOffset)
        {
            _tempPosition.y -= ActivityPositioningOffset;
        }
        SelectedTab.localPosition = _tempPosition;

        GameObject _tempDisplayContent;
        if (SelectedTab.gameObject.GetComponent<SingleTabElement>().hasDisplayContent)
        {
            _tempDisplayContent = SelectedTab.gameObject.GetComponent<SingleTabElement>().displayContent;
            _tempDisplayContent.gameObject.SetActive(true);
        }
    }
}
