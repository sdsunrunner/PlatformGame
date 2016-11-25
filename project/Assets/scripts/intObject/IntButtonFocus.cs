using UnityEngine;
using System.Collections;

public class IntButtonFocus : MonoBehaviour
{

	// Use this for initialization
    void Update()
    {
        // Get focused button.
        var focusedButton = transform.parent.GetComponent<IntButtonManager>().focusedButton;

        // Move toward same position as focused button.
        transform.position = Vector3.MoveTowards(transform.position, focusedButton.transform.position, Time.deltaTime * 10.0f);
    }
}
