/* Copyright (c) 2014 Advanced Platformer 2D */


using UnityEngine;
using System.Collections;

[AddComponentMenu("Advanced Platformer 2D/Samples/APSampleSpikes")]

// Sample for Spikes object
public class APSampleSpikes : MonoBehaviour
{
	////////////////////////////////////////////////////////
	// PUBLIC/HIGH LEVEL
	public float m_touchDamage = 1f;						// damage done when touching player

	////////////////////////////////////////////////////////
	// PRIVATE/LOW LEVEL
	float m_minTimeBetweenTwoReceivedHits = 0.1f;
	float m_lastHitTime;

	void Start () 
	{
		m_lastHitTime = 0f;
	}

	// called when character is entering this collectable
	public void OnTriggerEnter2D(Collider2D otherCollider)
	{
		APSamplePlayer character = otherCollider.GetComponent<APSamplePlayer>();
		if(character && !character.IsGodModeEnabled())
		{
			// prevent checking hits too often
			if(Time.time < m_lastHitTime + m_minTimeBetweenTwoReceivedHits)
				return;
			
			// save current hit time
			m_lastHitTime = Time.time;
			
			// add hit to character
			character.OnHit(m_touchDamage, transform.position);
		}
	}
}
