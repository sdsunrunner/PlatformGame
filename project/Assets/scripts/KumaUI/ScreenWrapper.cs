using UnityEngine;
using System.Collections;

public class ScreenWrapper {
	protected static int overrideWidth = 0;
	protected static int overrideHeight = 0;

	public static int width
	{
		get { return overrideWidth > 0 ? overrideWidth : Screen.width; }
	}

	public static int height
	{
		get { return overrideHeight > 0 ? overrideHeight : Screen.height; }
	}

	public static void overrideResolution(int width, int height)
	{
		Debug.Log("Overriding resolution to " + width + " x " + height);

		overrideWidth = width;
		overrideHeight = height;

        Screen.SetResolution(width, height, true);
	}
}
