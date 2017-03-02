using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// base class of all units, heroes, boss,trigger,ammos etc.
/// this class handles the presentation according to the data,
/// such as handle the presentation of some states, like movement, upgrade, destroy, be attacked etc.
/// also manages the model instances and animations
/// </summary>
public class InuStructure : InuObject
{
    #region variables

    // model instance currently
    protected GameObject mCurrentModel;

    // model instance name 
    protected SecureString mCurrentModelName;

    // only for battle logic
    private int mUniqueId;

    // hit point
    public InuFloat mMaxHp = new InuFloat(10000);
    public InuFloat mHp = new InuFloat(10000);

    // shield point
    public InuFloat mMaxShield = new InuFloat(0);
    public InuFloat mShield = new InuFloat(0);
    
    #endregion
  
    protected override void InuAwake()
    {
        base.InuAwake();
    }

    protected override void InuStart()
    {
        base.InuStart();
        mHp = mMaxHp;
        mShield = mMaxShield;

        if (state == InuDefine.EInuState.eCount)
        {
            state = InuDefine.EInuState.eIdle;
        }
    }

    protected override void InuUpdate()
    {
        base.InuUpdate();

        if (InuAIManager.instance.PauseGame)
            return;

        mInuTimer += InuTime.deltaTime;
        _InuUpdate();
    }

    protected override void InuOnDestroy()
    {
        //RemoveAllEnventListener();

        //if (audioSource)
        //{
        //    InuSFXManager.instance.Destroy(audioSource);
        //}
    }


    #region Inu Logic

    // timing controlling the state updating
    protected InuFloat mInuTimer;
    public InuFloat InuTimer { get { return mInuTimer; } }

    // step in inu timing
    protected short mInuStep;

    // if doing the Inu Logic currently
    protected bool mInuUpdating = false;

    // next state if changing state in Inu Update Logic
    protected InuDefine.EInuState mInuNextState = InuDefine.EInuState.eCount;

    // play customized logic or not
    protected bool mInuCustom;

    // customized data
    protected List<Dictionary<string, object>> mInuCustomData;
    // The most important function to parse the data and update the logic
    private void _InuUpdate()
    {
        mInuUpdating = true;

        List<Dictionary<string, object>> stateData = GetStateData();

        if (stateData != null)
        {
            while (true)
            {
                // reach the end of state logic
                if (mInuStep >= stateData.Count || mInuStep < 0)
                {
                    break;
                }
                else
                {
                    Dictionary<string, object> stepData = stateData[mInuStep];
                    if (stepData != null)
                    {
                        bool exit = InuHandleStep(stateData, stepData);
                        if (exit)
                        {
                            break;
                        }
                    }
                }
            }
        }

        mInuUpdating = false;

        // When State Logic Updating, change the state is very dangerous
        // Change state is put here after logic done
        if (mInuNextState != InuDefine.EInuState.eCount)
        {
            state = mInuNextState;
            mInuNextState = InuDefine.EInuState.eCount;
        }

        Inu_OnUpdate();

    }

    List<Dictionary<string, object>> GetStateData()
    {
        List<Dictionary<string, object>> stateData = null;

        if (mInuCustom)
        {
            stateData = mInuCustomData;
        }
        else
        {
            InuDefine.EInuState _state = state;
            if (null != mInuDicData && mInuDicData.ContainsKey(_state))
            {
                stateData = mInuDicData[_state];
            }
        }

        return stateData;
    }

    bool InuHandleStep(List<Dictionary<string, object>> stateData, Dictionary<string, object> _data)
    {
        Dictionary<string, object> _tempData = new Dictionary<string, object>(_data);
        // exitStep = true: update next step's logic in next frame
		// exitStep = false: if next step's time arrives, update next logic in this frame
		bool exitStep = true;
        InuFloat stepTime = new InuFloat((float)_tempData[InuParam.time]);

		// if the step time arrives
        if (mInuTimer >= stepTime)
        {
            InuCmd inucmd = (InuCmd)_tempData[InuParam.cmd];
            exitStep = false;

            // following is step logic
            switch (inucmd)
            {
                // do nothing cmd: just do nothing in this step	
                case InuCmd.DoNothing:
                    break;
                // exit cmd: do nothing any more in this state
                case InuCmd.Exit:
                    mInuStep = short.MaxValue;
                    exitStep = true;
                    OnStateExit(state);
                    return exitStep;

                // go to cmd: move to a specific step, can jump over several steps, or jump back	
                case InuCmd.GoTo:                   
                    mInuStep = (short)_tempData[InuParam.step];
                    if (stateData != null)
                    {
                        // reach the end of state logic
                        if (mInuStep >= stateData.Count || mInuStep < 0)
                        {
                            if (mInuStep >= stateData.Count)
                            {
                                OnStateComplete(state);
                            }
                        }
                        // if not, set the timer
                        else
                        {
                            Dictionary<string, object> stepData = stateData[mInuStep];
                            mInuTimer = new InuFloat((float)stepData[InuParam.time]);
                        }
                    }                    
                    exitStep = true;
                    return exitStep;

                case InuCmd.UsePrefab:
                    InuUpdatePrefab(_tempData);
                    break;

                // play animation cmd: play specific animation for which models
                case InuCmd.PlayAnimation:
                    InuPlayAnimation(_tempData, false);
                    break;

                // crossfade animation cmd: crossfade to another animation
                case InuCmd.CrossFadeAnimation:
                    InuPlayAnimation(_tempData, true);
                    break;

                // stop current animation cmd: stop immediately
                case InuCmd.StopAnimation:
                    InuStopAnimation(_tempData);
                    break;

                // play a particle effect cmd: play an effect on a dummy position
                case InuCmd.PlayEffect:
                    InuPlayEffect(_tempData);
                    break;

                // stop a particle effect cmd: stop a specific effect with name
                case InuCmd.StopEffect:
                    InuStopEffect(_tempData);
                    break;

                // play sound cmd: play a sfx	
                case InuCmd.PlaySfx:
                    InuPlaySfx(_tempData);
                    break;

                // Shake position cmd: iTween Shake utility
                case InuCmd.ShakePosition:
                    Inu_ShakePosition(_tempData);
                    break;

                // Orient to target cmd: face to an object within a second, the object(target) is assigned by its own AI
                case InuCmd.OrientToTarget:
                    Inu_OrientToTarget(_tempData);
                    break;

                // Destroy cmd: destroy this gameObject, be careful!	
                case InuCmd.Destroy:
                    Inu_Destroy(_tempData);
                    mInuStep = short.MaxValue;
                    exitStep = true;
                    OnStateDestroy(state);
                    return exitStep;
                // Shake camera cmd	
                case InuCmd.ShakeCamera:
                    Inu_ShakeCamera(_tempData);
                    break;

                // Goto Another state cmd
                case InuCmd.GoToState:
                    Inu_GoToState(_tempData);
                    exitStep = true;
                    OnStateExit(state);
                    return exitStep;

                // Wait customized time: the time is given by outside
                case InuCmd.WaitCustomTime:
                    Inu_WaitCustomTime(_tempData);
                    exitStep = true;
                    break;

                // Handle extra cmd in derived classes	
                default:
                    exitStep = Inu_ExtraFunction(_tempData);
                    break;

            }

            // When State Logic Updating, change the state is very dangerous
            // if state changed, leave current state immediately
            if (mInuNextState != InuDefine.EInuState.eCount)
            {
                return true;
            }

            mInuTimer -= stepTime;
            // step increment
            if (mInuStep < short.MaxValue)
                mInuStep++;
            if (mInuStep >= stateData.Count)           
                OnStateComplete(state);           
        }
        else
        {
            exitStep = true;
        }

        return exitStep;
    }
    protected virtual void InuUpdatePrefab(Dictionary<string, object> _data) { }
    protected virtual void InuPlayAnimation(Dictionary<string, object> _data, bool _crossfade)
    {
        string prefabName = (string)_data[InuParam.name];
        string animationName = (string)_data[InuParam.animname];
        Transform tModel = transform.Find(prefabName);
        if (tModel != null)
        {
            GameObject model = tModel.gameObject;

            if (model.GetComponent<Animation>())
            {
                if (model.GetComponent<Animation>()[animationName] != null)
                {
                    if (_crossfade)
                    {
                        model.GetComponent<Animation>().CrossFade(animationName);
                    }
                    else
                        model.GetComponent<Animation>().Play(animationName);
                }
            }
        }
    }
    void InuStopAnimation(Dictionary<string, object> _data)
    {
        string prefabName = (string)_data[InuParam.name];
        Transform tModel = transform.Find(prefabName);
        if (tModel != null)
        {
            GameObject model = tModel.gameObject;
            if (model.GetComponent<Animation>())
            {
                model.GetComponent<Animation>().Stop();
            }
        }
    }
    protected virtual void InuPlayEffect(Dictionary<string, object> _data)
    {
        string effectName = (string)_data[InuParam.name];
        object tmp;
        int effectSkip = 0;
        if (_data.TryGetValue(InuParam.effectskip, out tmp))
        {
            effectSkip = (int)tmp;
        }       

        string dummyName = (string)_data[InuParam.dummy];
        bool multiple = false;
        if (_data.ContainsKey(InuParam.destroy))
        {
            multiple = (bool)_data[InuParam.destroy];
        }

        Transform tModel;
        bool worldrotation = false;
        if (dummyName == "root")
            tModel = transform;
        else if (dummyName == "rootWorld")
        {
            tModel = transform;
            worldrotation = true;
        }
        else
        {
            tModel = transform.Find(dummyName);
        }
        if (tModel != null)
        {
            if (!multiple)
            {
                // if effect exist in dummy position, set it active
                if (tModel.Find(effectName))
                {
                    Transform tEffect = tModel.Find(effectName);
                    tEffect.gameObject.SetActive(true);
                    return;
                }
            }

            // create an instance of effect
            GameObject effect = InuResources.GetEffectInstance(effectName, tModel.position, worldrotation ? Quaternion.identity : tModel.rotation);
            if (effect != null)
            {
                //Vector3 effectPos = effect.transform.position;
                //Quaternion rot = effect.transform.rotation;
                effect.name = effectName;
                effect.transform.parent = tModel;
                effect.transform.localPosition = Vector3.zero;
                effect.transform.localRotation = Quaternion.identity;               
            }
        }
    }
    void InuStopEffect(Dictionary<string, object> _data)
    { 
        string effectName = (string)_data[InuParam.name];
        bool destroy = (bool)_data[InuParam.destroy];

        Transform[] listChilds = gameObject.GetComponentsInChildren<Transform>();
        Transform child;
        for (int i = 0; i < listChilds.Length; i++)
        {
            child = listChilds[i];
            if (child.gameObject.name == effectName)
            {
                if (destroy)
                    Destroy(child.gameObject);
                else
                    child.gameObject.SetActive(false);
            }
        }
    }

    void InuPlaySfx(Dictionary<string, object> _data)
    {
        string sfxName = (string)_data[InuParam.name];
        if (audioSource)
        {
            InuSFXManager.instance.Play(audioSource, sfxName);
        }
        else
        {
            Debug.LogError("No audio source find in this object: " + name);
        }
    }

    void Inu_ShakePosition(Dictionary<string, object> _data)
    {
        string prefabName = (string)_data[InuParam.name];
        Transform tModel = transform.Find(prefabName);
        if (tModel != null)
        {
            GameObject model = tModel.gameObject;
            // stop and destroy "move" iTween animation
            // maybe problems when stop and launch new iTween animation continuously(Destroy Components and Add new Components)
            //iTween.StopByName(model, "move");

            Dictionary<string, object> optionsHash = new Dictionary<string, object>();
            optionsHash.Add("name", "shake");
            optionsHash.Add("time", (float)_data[InuParam.during]);
            //only "loop" is allowed with shakes
            if ((iTween.LoopType)_data[InuParam.looptype] == iTween.LoopType.pingPong)
                optionsHash.Add("looptype", iTween.LoopType.loop);
            else
                optionsHash.Add("looptype", (iTween.LoopType)_data[InuParam.looptype]);
            optionsHash.Add("amount", (Vector3)_data[InuParam.amount3]);
            optionsHash.Add("islocal", true);
            iTween.ShakePosition(model, optionsHash);           
        }
    }

    void Inu_OrientToTarget(Dictionary<string, object> _data)
    {
    }
    public virtual void Inu_Destroy(Dictionary<string, object> _data)
    {
        InuDestroyed = true;
        Destroy(gameObject);
    }

    void Inu_ShakeCamera(Dictionary<string, object> _data)
    {
        Camera cam = Camera.main;
        if (cam != null)
        {
            GameObject model = cam.gameObject;
            if (model.GetComponent<iTween>())
            {
                return;
            }

            Dictionary<string, object> optionsHash = new Dictionary<string, object>();
            optionsHash.Add("name", "shake");
            optionsHash.Add("time", (float)_data[InuParam.during]);
            //only "loop" is allowed with shakes
            if ((iTween.LoopType)_data[InuParam.looptype] == iTween.LoopType.pingPong)
                optionsHash.Add("looptype", iTween.LoopType.loop);
            else
                optionsHash.Add("looptype", (iTween.LoopType)_data[InuParam.looptype]);
            optionsHash.Add("amount", (Vector3)_data[InuParam.amount3]);
            optionsHash.Add("islocal", true);
            iTween.ShakePosition(model, optionsHash);
        }
    }
    void Inu_GoToState(Dictionary<string, object> _data)
    {
        InuDefine.EInuState nstate = (InuDefine.EInuState)((short)_data[InuParam.step]);
        state = nstate;
    }
    protected virtual void Inu_WaitCustomTime(Dictionary<string, object> _data)
    {
        //mInuTimer -= mCustomDelay;
    }

    // Usual Update
    protected virtual void Inu_OnUpdate() { }

    // Handle extra cmd in derived classes
    protected virtual bool Inu_ExtraFunction(Dictionary<string, object> _data)
    {
        return false;
    }
    // when a state finish its process
    protected virtual void OnStateComplete(InuDefine.EInuState _state) { }

    // a special callback for a state finish its process at any time, then do something after Hero's casting skill or Building's working
    protected virtual void OnStateExit(InuDefine.EInuState _state) { }

    // when destroy the gameObject
    protected virtual void OnStateDestroy(InuDefine.EInuState _state) { }

    // when Select the Object
    protected virtual void OnSelect() { }

    // when Unselect the Object
    protected virtual void OnUnSelect() { }

    public virtual void EnableAI() { }

    public virtual void DisableAI() { }

    public virtual void ResetPathState() { }

   
    #endregion
}

