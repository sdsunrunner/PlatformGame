using UnityEngine;
using UnityEngine.UI;
using System.Collections;




[RequireComponent (typeof(UnityEngine.UI.Text))]
public class CardChestCountDisplayer : CountDisplayer
{
	public Slider mSlider;
	public float m_MaxCount = 1;
	bool mImageChanged;
	public Image mImageSlider;
	public Sprite mNormal;
	public Sprite mFinish;
    void Awake()
    {
        //
    }

    protected override void DoUpdate()
    {
    	base.DoUpdate();
    	if ( !mImageChanged && m_CurrCount == m_FinalCount && m_CurrCount >= m_MaxCount)
    	{
    		mImageChanged = true;
    		mImageSlider.sprite = mFinish;
    	}
    }

    protected override void UpdateDisplayText()
    {
    	base.UpdateDisplayText();
    	if(mSlider != null)
        {
            float rate = Mathf.Clamp(m_CurrCount/m_MaxCount,0f,1f);
            mSlider.value = rate;
        }
    }

    public override void OnValueChanged(int oldValue, int neoValue)
    {
    	base.OnValueChanged(oldValue,neoValue);
    	mImageChanged = false;
        if (oldValue >= m_MaxCount)
        {
            mImageSlider.sprite = mFinish;
        }
        else
        {
            mImageSlider.sprite = mNormal;
        }
    }
}
