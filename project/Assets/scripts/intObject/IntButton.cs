using UnityEngine;
using System.Collections;

public class IntButton : MonoBehaviour 
{
	// Use this for initialization
    public IntButton up = null;
    public IntButton down = null;
    public IntButton left = null;
    public IntButton right = null;

    void Update()
    {
        // Find out if we're the focused button.
        bool hasFocus = transform.parent.GetComponent<IntButtonManager>().focusedButton == this;

        // Fade alpha in and out depending on focus.
        var color = GetComponent<Renderer>().material.color;
        color.a = Mathf.MoveTowards(color.a, hasFocus ? 1.0f : 0.5f, Time.deltaTime * 3.0f);
        GetComponent<Renderer>().material.color = color;
    }
}
