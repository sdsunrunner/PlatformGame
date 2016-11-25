/* Copyright (c) 2014 Advanced Platformer 2D */

using UnityEngine;

[System.Serializable]
public class APAttack
{
    ////////////////////////////////////////////////////////
    // PUBLIC/HIGH LEVEL
	public APInputButton m_button;				                                    // button to use for this attack
	public int m_ammo = 10;						                                    // number of ammo
	public bool m_infiniteAmmo = false;			                                    // ignore ammo count?
	public bool m_autoFire = false;				                                    // enable/disable autofire
	public APBullet m_bullet;					                                    // reference to bullet prefab to launch
    public APHitZone[] m_hitZones;				                                    // list of hit zones for hit detection

	public AttackContext m_contextStand; 		// when attacking while stopped
	public AttackContext m_contextRun; 			// when attacking while running
	public AttackContext m_contextCrouched; 	// when attacking in crouched position
	public AttackContext m_contextInAir; 		// when attacking while in air

	public enum ContextId
	{
		eStand = 0,
		eRun,
		eCrouched,
		eInAir
	}

	[System.Serializable]
	public class AttackContext
	{
        public bool m_enabled = false;                  // activation status
		public string m_anim = string.Empty;            // animation state name
		public Vector2 m_bulletStartPosition;           // spawn position of bullet
		public float m_bulletDirection;		            // launch direction
        public bool m_stopOnGround = true;              // should we stop moving while on ground during an attack
    }

	public void FireBullet(APCharacterController launcher, ContextId contextId)
	{
		if ((m_ammo > 0 || m_infiniteAmmo) && (m_bullet != null))
		{
			if ( !m_infiniteAmmo )
			{
				m_ammo--;
			}

			AttackContext curContext = m_contextStand;
			switch(contextId)
			{
				case ContextId.eRun: curContext = m_contextRun; break;
				case ContextId.eInAir: curContext = m_contextInAir; break;
				case ContextId.eCrouched: curContext = m_contextCrouched; break;
			}

			// make sure move horizontal direction is valid
			float fAngle = Mathf.Deg2Rad * curContext.m_bulletDirection;
			Vector2 v2MoveDir = new Vector2(Mathf.Cos(fAngle), -Mathf.Sin(fAngle));
			if(launcher.GetMotor().m_faceRight && v2MoveDir.x < 0f || !launcher.GetMotor().m_faceRight && v2MoveDir.x > 0f)
			{
				v2MoveDir.x = -v2MoveDir.x;
			}

			// spawn and launch bullet (add player velocity before spawn)
			Vector2 pointPos = launcher.transform.TransformPoint(curContext.m_bulletStartPosition);
			pointPos = pointPos + (Time.deltaTime * launcher.GetMotor().m_velocity);
			APBullet newBullet = (APBullet)UnityEngine.Object.Instantiate(m_bullet, pointPos, Quaternion.identity);

			// init bullet
			v2MoveDir = launcher.transform.TransformDirection(v2MoveDir);
			newBullet.Setup(launcher, pointPos, v2MoveDir);

			// launch listeners
			launcher.EventListeners.ForEach(e => e.OnAttackBulletFired(this, newBullet));
		}
	}
}
