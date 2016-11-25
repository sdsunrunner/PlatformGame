using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


[RequireComponent (typeof (CanvasGroup))]
public class CanvasGroupRevealer : MonoBehaviour
{
    public CanvasGroup m_CanvasGroup;

    public float m_RevealTime = 0.5f; // Animation duration for Show

    // Animation duration for Hide. If 0, then use the same as RevealTime.
    public float m_HideTime = 0.0f; 
    public Vector3  m_HideOffset = new Vector3(0, 0, 0); // Position offset when hidden.
	public float    m_HiddenAlpha = 0.0f; // Alpha when hidden.
    public Vector3  m_HiddenScale = new Vector3(1, 1, 1); // Scale when hidden.
    public float m_ScalePopPercent = 0.0f; // 0.0f = 0%,  1.0f = 100%


    public bool m_HideOnStart = true;

    // Parent Canavas that should be deactivated when this revealer is hidden. (Performance).
    public Canvas m_DeactivateCanvasOnHide;

    /*
     * These screen revealers will be triggered to show with a delay 
     * when this Screen Revealer is triggered to show .
     * These screen revealers will also hide with no delay
     * when this Screen Revealer is triggered to hide.
     */
    public CanvasGroupRevealer[] m_ChildrenRevealers;
    public float m_ChildrenRevealDelay = 0.25f;


    // These screen revealers will be tridded to hide with no delay when
    // this Screen Revealer is triggered to hide.
    public CanvasGroupRevealer[] m_ChildrenHiders;

    protected bool m_PanelHidden = false;
    protected bool m_ChildrenScreensHidden = false;

	protected float m_AnimStartTime = 0.0f;
	protected float m_AnimPercent = 0.0f;

    protected RectTransform m_ScreenRectTransform;

    protected Vector3 m_OriginalLocalPosition;
    protected Vector3 m_HiddenLocalPosition;
    protected bool m_EnableMoving = true;

    protected bool m_OriginalInteractable = true;
    protected bool m_OriginalBlocksRaycasts = true;

    protected Vector3 m_OriginalScale = new Vector3(1, 1, 1);
    protected bool m_EnableScaling = true;

    // Pop on scale constants.
    const float POP_AT_ANIM_PERCENT = 0.90f;
    const float POP_ANIM_TIME_DIV_2 = (1.0f - POP_AT_ANIM_PERCENT) / 2.0f;

    protected float m_DeactivateCanvasInSecs = -1.0f;

    protected bool m_Initialized = false;

    public delegate void OnOpenCloseEventHandler(CanvasGroupRevealer canvasGroupRevealer);
    public event OnOpenCloseEventHandler OnOpen;
    public event OnOpenCloseEventHandler OnOpenStart;
    public event OnOpenCloseEventHandler OnClose;
    public event OnOpenCloseEventHandler OnCloseStart;

    public RectTransform rectTransform
    {
        get { return m_ScreenRectTransform; }
    }

    public void Start()
    {
        Init();
    }

    public void Init(bool forceReInit = false)
    {
        if (m_Initialized && !forceReInit)
            return;
        else
            m_Initialized = true;

        // If unassigned, attempt to find one attached to this object.
        if (m_CanvasGroup == null)
            m_CanvasGroup = GetComponent<CanvasGroup>();

        if (m_CanvasGroup == null)
        {
            Debug.LogError("ScreenRevealer constructor requires a canvas group.");
        }
        else
        {
            m_ScreenRectTransform = m_CanvasGroup.GetComponent<RectTransform>();

			UpdateOriginalPosition();

			if (m_HiddenAlpha < 0.0f || m_HiddenAlpha > 1.0f)
			{
				Debug.LogError("Hidden Alpha out of bounds. Clamping to 0.0f, 1.0f");
				m_HiddenAlpha = Mathf.Clamp(m_HiddenAlpha, 0.0f, 1.0f);
			}

            m_OriginalInteractable = m_CanvasGroup.interactable;
            m_OriginalBlocksRaycasts = m_CanvasGroup.blocksRaycasts;

            if (m_HideTime <= 0.0f)
                m_HideTime = m_RevealTime;
        }

        if (m_ChildrenRevealers != null && m_ChildrenRevealers.Length > 0)
            foreach (CanvasGroupRevealer child in m_ChildrenRevealers)
                if (child != null)
                    child.Init();

        if (m_ChildrenHiders != null && m_ChildrenHiders.Length > 0)
            foreach (CanvasGroupRevealer child in m_ChildrenHiders)
                if (child != null)
                    child.Init();

        if (m_HideOnStart)
			HidePanel(true,true);
    }

    public void Update()
	{
        float currAlpha = m_CanvasGroup.alpha;
        Vector3 currPos = m_ScreenRectTransform.anchoredPosition3D;
        Vector3 currScale = m_ScreenRectTransform.localScale;

        float timeSinceAnimStart = Time.unscaledTime - m_AnimStartTime;
        float clampedTimeSinceAnimStart;

        if (m_PanelHidden) // HIDE PANEL
		{
            clampedTimeSinceAnimStart = Mathf.Clamp(timeSinceAnimStart, 0.0f, m_HideTime);

			if (m_AnimPercent < 1.0f) {
				m_AnimPercent = Mathf.Clamp(getQuadraticEaseIn(clampedTimeSinceAnimStart, 0.0f, 1.0f, m_HideTime), 0.0f, 1.0f);
                if (m_AnimPercent == 1.0f && OnClose != null)
                    OnClose(this);
			}

            // Are we done hiding alpha?
			if (currAlpha != m_HiddenAlpha)
            {
				float neoAlpha = 1.0f * (1.0f - m_AnimPercent) + m_HiddenAlpha * m_AnimPercent; // linear interpolation.
                SetHideablePanelAlpha(neoAlpha);
            }

            // Are we done hiding position?
            if (m_EnableMoving && currPos != m_HiddenLocalPosition)
            {
				Vector3 neoPos = m_OriginalLocalPosition * (1.0f - m_AnimPercent) + m_HiddenLocalPosition * m_AnimPercent; // linear interpolation.
                m_ScreenRectTransform.anchoredPosition3D = neoPos;
            }

            // Are we done hiding scale?
            if (m_EnableScaling && currScale != m_HiddenScale)
            {
                float scalePercent = GetPoppingScalePercent(1.0f - m_AnimPercent);
                Vector3 neoScale = m_OriginalScale * (scalePercent) + m_HiddenScale * (1.0f -scalePercent); // linear interpolation.
                m_ScreenRectTransform.localScale = neoScale;
            }

            // Time to trigger hiding of children revealers?
            if (!m_ChildrenScreensHidden)
            {
                HideChildrenPanels();
            }

            // Is it time to deactivate the canvas of this revealer? (Performance)
            CheckDeactivateCanvas();
        }
        else if (!m_PanelHidden) // SHOW PANEL
		{
            clampedTimeSinceAnimStart = Mathf.Clamp(timeSinceAnimStart, 0.0f, m_RevealTime);

			if (m_AnimPercent < 1.0f) {
                m_AnimPercent = Mathf.Clamp(getQuadraticEaseOut(clampedTimeSinceAnimStart, 0.0f, 1.0f, m_RevealTime), 0.0f, 1.0f);
                if (m_AnimPercent == 1.0f && OnOpen != null)
                    OnOpen(this);
			}

            // Are we done revealing alpha?
            if (currAlpha != 1.0f)
            {
				float neoAlpha = m_HiddenAlpha * (1.0f - m_AnimPercent) + 1.0f * m_AnimPercent; // linear interpolation.
                SetHideablePanelAlpha(neoAlpha);
            }

            // Are we done revealing position?
            if (m_EnableMoving && currPos != m_OriginalLocalPosition)
            {
				Vector3 neoPos = m_OriginalLocalPosition * m_AnimPercent + m_HiddenLocalPosition * (1.0f - m_AnimPercent); // linear interpolation.
                m_ScreenRectTransform.anchoredPosition3D = neoPos;
                //Debug.LogError("\nm_ScreenRectTransform.anchoredPosition = " + m_ScreenRectTransform.anchoredPosition);
                //Debug.LogError("m_ScreenRectTransform.localPosition = " + m_ScreenRectTransform.localPosition);
            }

            // Are we done revealing scale?
            if (m_EnableScaling && currScale != m_OriginalScale)
            {
                float scalePercent = GetPoppingScalePercent(m_AnimPercent);
                Vector3 neoScale = m_OriginalScale * scalePercent + m_HiddenScale * (1.0f - scalePercent); // linear interpolation.
                m_ScreenRectTransform.localScale = neoScale;
            }

            // Is it time to trigger children revealers?
            if (m_ChildrenScreensHidden)
            {
                if (timeSinceAnimStart > m_ChildrenRevealDelay)
                {
                    ShowChildrenPanels();
                }
            }
        }
    }

	// This is important for the hide offset and should be called anytime that this rect transform
	public void UpdateOriginalPosition()
	{
		m_OriginalLocalPosition = m_ScreenRectTransform.anchoredPosition;
		m_HiddenLocalPosition = m_OriginalLocalPosition + m_HideOffset;
	}

    public void UpdateOriginalPosition(Vector3 neoPosition)
    {
        m_OriginalLocalPosition = neoPosition;
        m_HiddenLocalPosition = m_OriginalLocalPosition + m_HideOffset;
    }

    protected float GetPoppingScalePercent(float animPercent)
    {
        float scalePercent = animPercent;

        if (animPercent > POP_AT_ANIM_PERCENT + POP_ANIM_TIME_DIV_2)
        {
            float localAnim = (animPercent - (POP_AT_ANIM_PERCENT + POP_ANIM_TIME_DIV_2)) / POP_ANIM_TIME_DIV_2;
            scalePercent = animPercent + (animPercent * m_ScalePopPercent) * (1.0f - localAnim) + (animPercent * 0.0f) * localAnim;
        }
        else if (animPercent > POP_AT_ANIM_PERCENT)
        {
            float localAnim = (animPercent - POP_AT_ANIM_PERCENT) / POP_ANIM_TIME_DIV_2;
            scalePercent = animPercent + (animPercent * m_ScalePopPercent) * localAnim + (animPercent * 0.0f) * (1.0f - localAnim);
        }

        //if (scalePercent != animPercent)
        //    Debug.LogError("PoppingScalePercent = " + scalePercent + " orig = " + animPercent);

        return scalePercent;
    }

    public bool IsHidden
    {
        get { return m_PanelHidden; }
    }

    public bool IsFullyHidden
    {
        get { return m_PanelHidden && m_AnimPercent == 1.0f; }
    }

    public bool IsFullyShown
    {
        get { return !m_PanelHidden && m_AnimPercent == 1.0f; }
    }

	public void TogglePanel()
	{
		if (m_PanelHidden)
			ShowPanel();
		else
			HidePanel();
	}

    public void HidePanel(bool immediately = false , bool no_close = false)
    {
        if (!m_Initialized)
            Init();

		if (m_PanelHidden) 
			return;

        m_PanelHidden = true;
        m_ChildrenScreensHidden = false;

        if (immediately)
        {
            SetHideablePanelAlpha(0.0f);
            if (m_EnableMoving)
                m_ScreenRectTransform.anchoredPosition3D = m_HiddenLocalPosition;
            if (m_EnableScaling)
                m_ScreenRectTransform.localScale = m_HiddenScale;
            m_AnimStartTime = Time.unscaledTime - m_HideTime;
            if(!no_close && this.OnClose != null)
                this.OnClose(this);
            //HideChildrenPanels(true);
        }
        else
        {
			if (m_AnimPercent > 0.0f && m_AnimPercent < 1.0f) // are we mid-animation?
			{
				m_AnimPercent = 1.0f - m_AnimPercent; // we are switching directions, so animPercent should be flipped as well.
				m_AnimStartTime = Time.unscaledTime - getQuadraticEaseInTimeStarted(m_AnimPercent, 0.0f, 1.0f, m_HideTime); 
			}
			else // no, so fresh anim start.
			{
				m_AnimStartTime = Time.unscaledTime;
				m_AnimPercent = 0.0f;
			}
        }

        // Non-interactable and doesn't block raycasts when "hidden", but only if it is truly hidden.
        if (m_HiddenAlpha < 1.0f)
        {
            if (m_CanvasGroup == null)
                Debug.LogError("m_CanvasGroup unassigned in menu object = " + name);
            m_CanvasGroup.interactable = false;
            m_CanvasGroup.blocksRaycasts = false;
        }

        if (m_DeactivateCanvasOnHide != null)
            m_DeactivateCanvasInSecs = m_HideTime + 0.5f;

        if (OnCloseStart != null)
            OnCloseStart(this);
    }

    public void ShowPanel(bool immediately = false)
    {
        if (!m_Initialized)
            Init();

		if (m_PanelHidden == false)
			return;

        m_PanelHidden = false;
        m_ChildrenScreensHidden = true;

        CheckActivateCanvas();

        if (immediately)
        {
            SetHideablePanelAlpha(1.0f);
            if (m_EnableMoving)
                m_ScreenRectTransform.anchoredPosition3D = m_OriginalLocalPosition;
            if (m_EnableScaling)
                m_ScreenRectTransform.localScale = m_OriginalScale;
            m_AnimStartTime = Time.unscaledTime - m_RevealTime;
            //ShowChildrenPanels(true);
        }
        else
		{
			if (m_AnimPercent > 0.0f && m_AnimPercent < 1.0f) // are we mid-animation?
			{
				m_AnimPercent = 1.0f - m_AnimPercent; // we are switching directions, so animPercent should be flipped as well.
				m_AnimStartTime = Time.unscaledTime - getQuadraticEaseOutTime(m_AnimPercent, 0.0f, 1.0f, m_RevealTime);
			}
			else //no, so fresh anim start.
			{
				m_AnimStartTime = Time.unscaledTime;
				m_AnimPercent = 0.0f;
			}
        }

        // Restore original interactable and blocksRaycasts behavior on reveal.
        m_CanvasGroup.interactable = m_OriginalInteractable;
        m_CanvasGroup.blocksRaycasts = m_OriginalBlocksRaycasts;

        if (OnOpenStart != null)
            OnOpenStart(this);
    }

    protected void HideChildrenPanels(bool immediately = false)
    {
        m_ChildrenScreensHidden = true;

        if (m_ChildrenRevealers != null)
            foreach (CanvasGroupRevealer child in m_ChildrenRevealers)
				if (child != null)
               		child.HidePanel(immediately); 
            
        if (m_ChildrenHiders != null)
            foreach (CanvasGroupRevealer child in m_ChildrenHiders)
				if (child != null)
               		child.HidePanel(immediately);
    }

    protected void ShowChildrenPanels(bool immediately = false)
    {
        m_ChildrenScreensHidden = false;

        if (m_ChildrenRevealers != null)
            foreach (CanvasGroupRevealer child in m_ChildrenRevealers)
				if (child != null)
                	child.ShowPanel(immediately);
    }

    protected virtual void SetHideablePanelAlpha(float alpha)
    {
        m_CanvasGroup.alpha = alpha;
    }

	public static float getQuadraticEaseIn(float t, float b, float c, float d)
	{
		t /= d;
		return c*t*t + b;
	}

    public static float getQuadraticEaseInOut(float t, float b, float c, float d)
    {
        t /= d / 2;
        if (t < 1) return c / 2 * t * t + b;
        t--;
        return -c / 2 * (t * (t - 2) - 1) + b;
    }

    protected float getQuadraticEaseInTimeStarted(float p, float b, float c, float d)
	{
		// solve for t, using the Quadratic Ease In equation from getQuadraticEaseIn() method.
		/*
		 * p = c * (t/d)^2 + b
		 * (p - b) / c = (t/d)^2
		 * sqrt((p-b)/c) * d = t
		 */
		float t = Mathf.Sqrt((p-b)/c) * d;
		return t;
	}

    public static float getQuadraticEaseOut(float t, float b, float c, float d)
	{
		t /= d;
		return -c * t*(t-2) + b;
    } 

	protected float getQuadraticEaseOutTime(float p, float b, float c, float d)
	{
		// solve for t, using the Quadratic Ease Out equation from getQuadraticEaseOut() method.
		/*
		 * p = -c * (t/d)*(t/d - 2) + b
		 * (p - b)/(-c) = (t/d)*(t/d - 2)
		 * ((p - b) * d)/(-ct) = t/d - 2
		 * ((p - b) *d)/(-c) = t^2/d - 2t
		 * substitute left side with x since they are all known values
		 * x = t^2/d - 2t
		 * 0 = (1/d)t^2 - 2t - x
		 * divide by coefficient of t^2
		 * 0 = t^2 - 2dt - xd
		 * xd = t^2 - 2dt
		 * complete the square: (-2d) divide by 2 and squared = d^2
		 * xd + d^2 = t^2 - 2dt + d^2
		 * xd + d^2 = (t-d)^2
		 * sqrt(xd + d^2) = t - d
		 * sqrt(xd + d^2) + d = t
		 */
		float x = (p - b) / (-c);
		float sqrtVal = Mathf.Sqrt(x*d*d + d*d);
		// sqrtVal can have two answers, positive or negative, try both, 
		// choosing the one that is between 0.0f and d. As long as  0 <= p <= 1.0f,
		// the correct time should be between 0.0f and d (duration of animation).
		float time = sqrtVal + d;
		if (time > d || time < 0) 
			time = -sqrtVal + d;
		if (time > d || time < 0)
			Debug.LogError("unable to find solution in getQuadraticEaseOutTime for " +
				"(p,b,c,d) = ("+p+","+b+","+c+","+d+")");
		return time;
	}

    protected void CheckDeactivateCanvas()
    {
        if (m_DeactivateCanvasInSecs > 0.0f)
        {
            m_DeactivateCanvasInSecs -= Time.unscaledDeltaTime;
            if (m_DeactivateCanvasInSecs <= 0.0f)
            {
                m_DeactivateCanvasInSecs = -1.0f;
                if (m_DeactivateCanvasOnHide != null)
                    m_DeactivateCanvasOnHide.gameObject.SetActive(false);
            }
        }
    }

    protected void CheckActivateCanvas()
    {
        if (m_DeactivateCanvasOnHide != null &&
            m_DeactivateCanvasOnHide.gameObject.activeSelf == false)
            m_DeactivateCanvasOnHide.gameObject.SetActive(true);
    }

    public Vector3 OriginalPosition { get { return m_OriginalLocalPosition; } }

    public void DisableScaling() { m_EnableScaling = false; }
    public void EnableScaling() { m_EnableScaling = true; }

    public void DisableMoving() { m_EnableMoving = false; }
    public void EnableMoving() {m_EnableMoving = true; }

}