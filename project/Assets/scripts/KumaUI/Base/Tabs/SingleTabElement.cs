using UnityEngine;
using System.Collections;

public class SingleTabElement : MonoBehaviour {

    public GameObject displayContent;
    public bool hasDisplayContent;

    void Awake()
    {
        if (displayContent != null)
        {
            hasDisplayContent = true;
        }
        else
        {
            hasDisplayContent = false;
        }
    }
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
