using UnityEngine;
using InControl;


public class IntButtonManager : MonoBehaviour 
{
    public enum EBtnPageState
    {
        eNone = 0,
        eStandby, 
        eGo,
        eBack,
        eEnd,
    }
    private EBtnPageState mCurState = EBtnPageState.eNone;
    private EBtnPageState mPreState = EBtnPageState.eNone;

    public IntButton focusedButton;

    TwoAxisInputControl filteredDirection;
    void Awake()
    {
        filteredDirection = new TwoAxisInputControl();
        filteredDirection.StateThreshold = 0.5f;
        state = EBtnPageState.eStandby;
    }
   
    void Update()
    {
        if (state == EBtnPageState.eStandby)
        {
            // Use last device which provided input.
            var inputDevice = InputManager.ActiveDevice;
            filteredDirection.Filter(inputDevice.Direction, Time.deltaTime);

            if (filteredDirection.Left.WasRepeated)
            {
                Debug.Log("!!!");
            }

            // Move focus with directional inputs.
            if (filteredDirection.Up.WasPressed)
            {
                MoveFocusTo(focusedButton.up);
            }

            if (filteredDirection.Down.WasPressed)
            {
                MoveFocusTo(focusedButton.down);
            }

            if (filteredDirection.Left.WasPressed)
            {
                MoveFocusTo(focusedButton.left);
            }

            if (filteredDirection.Right.WasPressed)
            {
                MoveFocusTo(focusedButton.right);
            }

            if (inputDevice.Start)
            {                
                state = EBtnPageState.eGo;
            }
            if (inputDevice.Back)
            {
                state = EBtnPageState.eBack;
            }
        }       
    }

    public EBtnPageState state
    {
        get { return mCurState; }
        set { _OnStateChange(value); }
    }
    private void _OnStateChange(EBtnPageState _state)
    {
        if (_state != mCurState)
        {           
            mPreState = mCurState;
            mCurState = _state;
            _StateEnter(mPreState);
        }
    }

    private void _StateEnter(EBtnPageState _prevState)
    {
        switch (mCurState)
        {
            case EBtnPageState.eGo:
                Debug.LogError("-----gogogo!-----------------");
                break;
            case EBtnPageState.eBack:
                Debug.LogError("-----back!-----------------");
                break;   
        }
    }
    void MoveFocusTo(IntButton newFocusedButton)
    {
        if (newFocusedButton != null)
        {
            focusedButton = newFocusedButton;
        }
    }
}
