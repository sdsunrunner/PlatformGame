/* Copyright (c) 2014 Advanced Platformer 2D */

using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

[AddComponentMenu("Advanced Platformer 2D/Samples/APSampleGUI")]

// Sample GUI handler
public class APSampleGUI : APCharacterEventListener 
{
	////////////////////////////////////////////////////////
	// PUBLIC/HIGH LEVEL
	public bool m_showLife = true;
	public bool m_showAmmo = true;
	public bool m_showScore = true;

	public GUIStyle m_text;
	public GUIStyle m_icon;
	public Texture m_textureLife;
	public Texture m_textureAmmo;
	public Texture m_textureScore;
	public Texture2D m_textureFade;
	public float m_fadeDuration = 1.5f; 		// time for fade in/out


	public void SetAmmoCount(int ammo) { m_ammoCount = ammo; }
	public void SetLife(int life) { m_lifeCount = life; }
	public void SetScore(int score) { m_scoreCount = score; }

	////////////////////////////////////////////////////////
	// PRIVATE/LOW LEVEL
	int m_ammoCount = 0;
	int m_lifeCount = 0;
	int m_scoreCount = 0;
	bool m_fadeToBlack;
	float m_fadeAlpha;
	string m_levelToLoad;
	float m_timeToLoad = 0f;
	APCharacterController m_character;

	// Use this for initialization
	void Awake()
	{
		m_character = GameObject.FindObjectOfType<APCharacterController>();
	}

	// Use this for initialization
	void Start () 
	{
		m_fadeToBlack = false;
		m_fadeAlpha = 0f;
		m_timeToLoad = 0f;

		// register event listener to character
		if(m_character)
		{
			m_character.EventListeners.Add(this);
		}
	}

	// Drawing
	void OnGUI () 
	{
		GUILayout.BeginHorizontal();

		if(m_showLife)
		{
			GUILayout.Box ( m_textureLife, m_icon );
			GUILayout.Label ( m_lifeCount.ToString(), m_text);
		}

		if(m_showAmmo)
		{
			GUILayout.Box ( m_textureAmmo, m_icon );
			GUILayout.Label ( m_ammoCount.ToString(), m_text);
		}

		if(m_showScore)
		{
			GUILayout.Box ( m_textureScore, m_icon );
			GUILayout.Label ( m_scoreCount.ToString(), m_text);
		}

		GUILayout.EndHorizontal();

		// Handle fade to black
		if(m_fadeToBlack)
		{
			Color color = Color.white;
			color.a = m_fadeAlpha;						
			GUI.color = color;
			GUI.DrawTexture(new Rect(0f, 0f, Screen.width, Screen.height),  m_textureFade);
		}
	}

	public void Update()
	{
		if(m_fadeToBlack)
		{
            m_fadeAlpha = Mathf.MoveTowards(m_fadeAlpha, 1f, m_fadeDuration > Mathf.Epsilon ? Time.deltaTime / m_fadeDuration : float.MaxValue);

			if(m_fadeAlpha >= 1f)
			{
                SceneManager.LoadScene(m_levelToLoad);
			}
		}
	}
	
	public void LoadLevel (string levelToLoad, float timeToLoad = 0f)
	{
		m_levelToLoad = levelToLoad;
		m_timeToLoad = timeToLoad;

		StartCoroutine("LoadLevelRoutine");
	}

	IEnumerator LoadLevelRoutine () 
	{
		yield return new WaitForSeconds (m_timeToLoad);
		
		// launch fade to black
		m_fadeToBlack = true;
		m_fadeAlpha = 0f;
	}

	// Refresh GUI
	public override void OnAttackBulletFired(APAttack attack, APBullet bullet)
	{
		m_ammoCount = attack.m_ammo;
	}
}
