/* Copyright (c) 2014 Advanced Platformer 2D */

using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(APCharacterController))]
[AddComponentMenu("Advanced Platformer 2D/Samples/APSamplePlayer")]

// Sample for handling Player behavior when being hit + simple life system
public class APSamplePlayer : MonoBehaviour 
{
	////////////////////////////////////////////////////////
	// PUBLIC/HIGH LEVEL
	public float m_life = 10f;   							// total life of player
	public int m_score = 0;   								// initial score of player
	public int m_scorePerLife = 0;   						// give a new life each time this score point is collected
	public float m_jumpDamage = 1f;							// ammount of damage done when jumping on Blob
	public float m_godModeTimeWhenHit = 3f;					// time of god mode (i.e invincible mode when beeing hit)
	public Vector2 m_hitImpulse = new Vector2(8f, 12f);		// impulse when beeing hit by Blob
	public float m_waitTimeAfterDie = 0f;					// time to wait before reseting level after dying (to allows die animation to play)
	public string m_animDie;								// state of animation die

	////////////////////////////////////////////////////////
	// PRIVATE/LOW LEVEL
	APCharacterController m_player;
	Animator m_anim;
	bool m_godMode;
	float m_godModeTime;
	APSampleGUI m_gui;
	bool m_refreshGui;

	// Gets access to our GUI
	public APSampleGUI GetGUI() { return m_gui; }

	// check if currently in god mode
	public bool IsGodModeEnabled()
	{
		return m_godMode; 
	}

	// Use this for initialization
	void Start () 
	{
		// some initializations variables
		m_player = GetComponent<APCharacterController>();
		m_anim = GetComponent<Animator>();

		m_godMode = false;
		m_godModeTime = 0f;
		m_refreshGui = true;

		// save ref to our sample GUI here
		m_gui = GameObject.FindObjectOfType<APSampleGUI>();

		// initialize GUI
		RefreshGUI();
	}

	// Update is called once per frame
	void Update () 
	{
		RefreshGUI();
		
		// handle god mode here
		if(m_godMode && Time.time > m_godModeTime + m_godModeTimeWhenHit)
		{
			if(m_anim)
			{
				m_anim.SetBool("GodMode", false);
			}
			m_godMode = false;
		}
	}

	// refresh GUI
	void RefreshGUI()
	{
		if(m_gui && m_refreshGui)
		{
			m_refreshGui = false;

			// for now display only first ranged attack ammo
			if(m_player.m_attacks.m_attacks.Length > 0)
			{
				m_gui.SetAmmoCount(m_player.m_attacks.m_attacks[0].m_ammo);
			}

			// todo : life should be int
			m_gui.SetLife(Mathf.RoundToInt(m_life));
			m_gui.SetScore(m_score);
		}
	}

	// Add life points to player
	public void AddLife(float life)
	{
		m_life += life;
		m_refreshGui = true;
	}

	// Add score points to player
	public void AddScore(int score)
	{
		// add life point if needed
		if(score > 0 && m_scorePerLife > 0)
		{
			int added = (m_score % m_scorePerLife) + score;
			if(added > 0)
				AddLife((float)(added / m_scorePerLife));
		}

		m_score += score;
		m_refreshGui = true;
	}
		
	// Add ammo to player's specified ranged attack
	public void AddAmmo(int ammo, int rangedAttackIndex)
	{
		m_refreshGui = true;
		if(m_player.m_attacks.m_attacks.Length >= rangedAttackIndex)
		{
			m_player.m_attacks.m_attacks[rangedAttackIndex].m_ammo += ammo;
		}
	}

	// return true if character is dead
	public bool IsDead() 
	{
		return m_life <= 0f;
	}
	
	// called when hit by NPC object
	public void OnHit(float fDamage, Vector3 hitPos)
	{
		// do nothing if already dead or no damage
		if (IsDead() || (fDamage <= 0f))
			return;

		// handle death & callbacks
		if (fDamage >= m_life)
		{
			Kill();
		}
		else
		{
			m_life -= fDamage;
			m_refreshGui = true;

			// hit!
			// enable god mode if not already done
			if(!m_godMode)
			{
				m_godMode = true;
				m_godModeTime = Time.time;
				if(m_anim)
				{
					m_anim.SetBool("GodMode", true);
				}
				
				// add small impulse in opposite direction
				Vector2 v2Dir = transform.position - hitPos;
				m_player.SetVelocity(new Vector2((v2Dir.x > 0f ? 1f : -1f) * m_hitImpulse.x, m_hitImpulse.y));
			}
		}
	}

	// force player to die
	public void Kill()
	{
		// die !
		m_life = 0f;
		m_player.IsControlled = true; // prevent any more action from player
		m_refreshGui = true;
		
		// play animation if exists
		if(!string.IsNullOrEmpty(m_animDie))
		{
			m_player.PlayAnim(m_animDie);
		}

		// Request restart of level
		StartCoroutine("RestartLevel");
	}

	IEnumerator RestartLevel () 
	{
		yield return new WaitForSeconds (m_waitTimeAfterDie);

		// launch fade to black
		if(m_gui)
		{           
            m_gui.LoadLevel(SceneManager.GetActiveScene().name);
		}

		// remove player
		gameObject.SetActive(false);
	}
}
