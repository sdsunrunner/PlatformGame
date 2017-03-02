using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// base class of Inu objects
/// this class define the Inu data, handles states change basically
/// </summary>
public class InuObject : MonoBehaviour
{
    #region variables
    public int mOrder = 0;
    protected Transform m_Transform;
    protected InuVector2 mInuPosition = new InuVector2();
    public InuVector2 pp
    {
        set
        {
            float y = transform.localPosition.y;
            mInuPosition = value;
            transform.localPosition = mInuPosition.ToUnityVector3(y);
        }
        get
        {
            return mInuPosition;
        }
    }

    // Inu type
    protected InuDefine.EInuType mType = InuDefine.EInuType.eNone;   

    public virtual InuDefine.EInuType GetInuType()
    {
        return mType;
    }

    // main data stored as string
    public string mInuData;
    // main data structure stored in memory
	public Dictionary<InuDefine.EInuState, List<Dictionary<string, object>>> mInuDicData;
	
	#endregion

    //-----------------------Audio---------------------------------
    #region Audio

    // defined audio source
    private AudioSource mAudioSource;
    public AudioSource audioSource
    {
        set
        {
            mAudioSource = value;
        }
        get
        {
            return mAudioSource;
        }
    }

    #endregion
    //-----------------------State---------------------------------

    #region state basically

    // default is idle state
    InuDefine.EInuState mState = InuDefine.EInuState.eCount;

    // store the previous state
    InuDefine.EInuState mPreState = InuDefine.EInuState.eCount;

    public InuDefine.EInuState state
    {
        get
        {
            return mState;
        }
        set
        {
            // this will call the OnStateChange() interface
            OnStateChange(value);
        }
    }

    public InuDefine.EInuState prevState
    {
        get
        {
            return mPreState;
        }
    }
    #endregion

    #region virual interfaces
    protected virtual void OnStateChange(InuDefine.EInuState _state)
    {
        OnPreStateChange(_state);
        mPreState = mState;
        mState = _state;
        OnPostStateChange();
    }

    protected virtual void OnPreStateChange(InuDefine.EInuState _state) { }

    protected virtual void OnPostStateChange() { }

    #endregion


    //-----------------------Monobehavior---------------------------------
    #region Monobehavior methods
    bool mStarted = false;
    bool mDestroyed = false;
    public bool InuDestroyed
    {
        get
        {
            return mDestroyed;
        }
        set
        {
            mDestroyed = value;
        }
    }
    public static bool operator !=(InuObject c1, InuObject c2)
    {
        if (object.ReferenceEquals(c1, null))
        {
            return !object.ReferenceEquals(c2, null) && !c2.InuDestroyed;
        }
        if (object.ReferenceEquals(c2, null))
        {
            return !object.ReferenceEquals(c1, null) && !c1.InuDestroyed;
        }
        return !object.ReferenceEquals(c1, c2);
    }
    public static bool operator ==(InuObject c1, InuObject c2)
    {
        if (object.ReferenceEquals(c1, null))
        {
            return object.ReferenceEquals(c2, null) || c2.InuDestroyed;
        }
        if (object.ReferenceEquals(c2, null))
        {
            return object.ReferenceEquals(c1, null) || c1.InuDestroyed;
        }
        return object.ReferenceEquals(c1, c2);
    }

    public static List<InuObject> sObjects = new List<InuObject>();
    protected void Awake()
    {
        sObjects.Add(this);
        InuAwake();
    }
    protected void Update()
    {        
        InuUpdate();
    }
    protected void OnDestroy()
    {
        mDestroyed = true;
        sObjects.Remove(this);
        InuOnDestroy();
    }

    protected virtual void InuAwake()
    {
        m_Transform = transform;

        Transform tAudio = m_Transform.FindChild("sfx");
        if (tAudio != null)
        {
            audioSource = tAudio.GetComponent<AudioSource>();
        }

        if (mInuData != null)
        {
            mInuDicData = InuData.ParseData(mInuData);
        }
    }

    protected virtual void InuStart()
    {
    }

    protected virtual void InuUpdate()
    {
        if (!mStarted)
        {
            mStarted = true;
            InuStart();
        }
    }
    protected virtual void InuOnDestroy()
    {
        mDestroyed = true;
    }
    #endregion



    //----------for---Editor------------
    #region for Editor

    public void ReloadAndPlay(Dictionary<InuDefine.EInuState, List<Dictionary<string, object>>> _InuDicData)
    {
        mInuDicData = _InuDicData;
        state = state;
    }

    private string mPrefabPath;

    public string prefabPath
    {
        set
        {
            mPrefabPath = value;
        }
        get
        {
            return mPrefabPath;
        }
    }


    #endregion
    
}
