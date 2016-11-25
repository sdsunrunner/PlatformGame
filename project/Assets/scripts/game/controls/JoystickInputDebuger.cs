using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using InControl;

public class JoystickInputDebuger : MonoBehaviour
{
	// Use this for initialization   

    GUIStyle style = new GUIStyle();
    List<LogMessage> logMessages = new List<LogMessage>();
    bool isPaused;


    void OnEnable()
    {
        isPaused = false;
        Time.timeScale = 1.0f;
     
        InputManager.OnDeviceAttached += inputDevice => Debug.Log("Attached: " + inputDevice.Name);
        InputManager.OnDeviceDetached += inputDevice => Debug.Log("Detached: " + inputDevice.Name);
        InputManager.OnActiveDeviceChanged += inputDevice => Debug.Log("Active device changed to: " + inputDevice.Name);

        InputManager.OnUpdate += HandleInputUpdate;      
    }


    void HandleInputUpdate(ulong updateTick, float deltaTime)
    {
        CheckForPauseButton();       
    }


    void Start()
    {
        var unityDeviceManager = InputManager.GetDeviceManager<UnityInputDeviceManager>();
        unityDeviceManager.ReloadDevices();
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Application.LoadLevel("TestInputManager");
        }

#if UNITY_ANDROID && INCONTROL_OUYA && !UNITY_EDITOR
			var inputDevice = InputManager.ActiveDevice;
			Debug.Log( "[InControl] " + inputDevice.LeftStick.Vector );
			var anyButton = inputDevice.AnyButton;
			if (anyButton)
			{
				Debug.Log( "[InControl] AnyButton = " + anyButton.Handle );
			}
#endif
    }


    void CheckForPauseButton()
    {
        if (Input.GetKeyDown(KeyCode.P) || InputManager.MenuWasPressed)
        {
            Time.timeScale = isPaused ? 1.0f : 0.0f;
            isPaused = !isPaused;
        }
    }


    void SetColor(Color color)
    {
        style.normal.textColor = color;
    }


    void OnGUI()
    {
        if (!Constants.JOYSTICK_INPUT_DEBUF)
            return;
        var w = 500;
        var x = 10;
        var y = 10;
        var lineHeight = 15;      
        SetColor(Color.white);

        string info = "Devices:";
        info += " (Platform: " + InputManager.Platform + ")";
       
        info += " " + InputManager.ActiveDevice.Direction.Vector;
        
        if (isPaused)
        {
            SetColor(Color.red);
            info = "+++ PAUSED +++";
        }

        GUI.Label(new Rect(x, y, x + w, y + 10), info, style);

        SetColor(Color.white);

        foreach (var inputDevice in InputManager.Devices)
        {
            bool active = InputManager.ActiveDevice == inputDevice;
            Color color = active ? Color.yellow : Color.white;

            y = 35;

            SetColor(color);

            GUI.Label(new Rect(x, y, x + w, y + 10), inputDevice.Name, style);
            y += lineHeight;

            GUI.Label(new Rect(x, y, x + w, y + 10), "SortOrder: " + inputDevice.SortOrder, style);
            y += lineHeight;

            GUI.Label(new Rect(x, y, x + w, y + 10), "LastChangeTick: " + inputDevice.LastChangeTick, style);
            y += lineHeight;

            foreach (var control in inputDevice.Controls)
            {
                if (control != null)
                {
                    string controlName;

                    if (inputDevice.IsKnown)
                    {
                        controlName = string.Format("{0} ({1})", control.Target, control.Handle);
                    }
                    else
                    {
                        controlName = control.Handle;
                    }
                    //Debug.LogError("controlName:" + controlName);

                    SetColor(control.State ? Color.green : color);
                    var label = string.Format("{0} {1}", controlName, control.State ? "= " + control.Value : "");
                    GUI.Label(new Rect(x, y, x + w, y + 10), label, style);
                    y += lineHeight;
                }
            }

            y += lineHeight;

            color = active ? new Color(1.0f, 0.7f, 0.2f) : Color.white;
            if (inputDevice.IsKnown)
            {
                var control = inputDevice.LeftStickX;
                SetColor(control.State ? Color.green : color);
                var label = string.Format("{0} {1}", "Left Stick X", control.State ? "= " + control.Value : "");
                GUI.Label(new Rect(x, y, x + w, y + 10), label, style);
                y += lineHeight;

                control = inputDevice.LeftStickY;
                SetColor(control.State ? Color.green : color);
                label = string.Format("{0} {1}", "Left Stick Y", control.State ? "= " + control.Value : "");
                GUI.Label(new Rect(x, y, x + w, y + 10), label, style);
                y += lineHeight;

                control = inputDevice.RightStickX;
                SetColor(control.State ? Color.green : color);
                label = string.Format("{0} {1}", "Right Stick X", control.State ? "= " + control.Value : "");
                GUI.Label(new Rect(x, y, x + w, y + 10), label, style);
                y += lineHeight;

                control = inputDevice.RightStickY;
                SetColor(control.State ? Color.green : color);
                label = string.Format("{0} {1}", "Right Stick Y", control.State ? "= " + control.Value : "");
                GUI.Label(new Rect(x, y, x + w, y + 10), label, style);
                y += lineHeight;

                control = inputDevice.DPadX;
                SetColor(control.State ? Color.green : color);
                label = string.Format("{0} {1}", "DPad X", control.State ? "= " + control.Value : "");
                GUI.Label(new Rect(x, y, x + w, y + 10), label, style);
                y += lineHeight;

                control = inputDevice.DPadY;
                SetColor(control.State ? Color.green : color);
                label = string.Format("{0} {1}", "DPad Y", control.State ? "= " + control.Value : "");
                GUI.Label(new Rect(x, y, x + w, y + 10), label, style);
                y += lineHeight;
            }

            SetColor(Color.cyan);
            var anyButton = inputDevice.AnyButton;
            if (anyButton)
            {
                GUI.Label(new Rect(x, y, x + w, y + 10), "AnyButton = " + anyButton.Handle, style);
            }

            x += 200;
        }


        Color[] logColors = { Color.gray, Color.yellow, Color.white };
        SetColor(Color.white);
        x = 10;
        y = Screen.height - (10 + lineHeight);
        for (int i = logMessages.Count - 1; i >= 0; i--)
        {
            var logMessage = logMessages[i];
            SetColor(logColors[(int)logMessage.type]);
            foreach (var line in logMessage.text.Split('\n'))
            {
                GUI.Label(new Rect(x, y, Screen.width, y + 10), line, style);
                y -= lineHeight;
            }
        }
    }
}
