using UnityEngine;
using System.Collections;

public enum MoveDirectionEnum
{
    up,
    down,
    left,
    right
};

public class InuParam
{
    public static string cmd = "cmd";
    public static string time = "time";
    public static string step = "step";
    public static string name = "name";
    public static string animname = "animname";
    public static string dummy = "dummy";
    public static string during = "during";
    public static string destroy = "destroy";
    public static string amount1 = "amount1";
    public static string amount2 = "amount2";
    public static string amount3 = "amount3";
    public static string looptype = "looptype";
    public static string easetype = "easetype";
    public static string random = "random";
    public static string projectile = "projectile";
    public static string axisstrict = "axisstrict";
    public static string effectskip = "effectskip";
}

public enum InuCmd
{
    DoNothing = 0,				// do nothing cmd: just do nothing in this step
    GoTo,						// go to cmd: move to a specific step, can jump over several steps, or jump back
    Exit,						// exit cmd: do nothing any more in this state
    UsePrefab,					// use prefab cmd: change a prefab as visual models
    UseMaterial,				// use material cmd: change the material for the models
    UseShader,					// use shader cmd: change the shader of the material of the models
    PlayAnimation,				// play animation cmd: play specific animation for which models
    StopAnimation,				// stop current animation cmd: stop immediately
    CrossFadeAnimation,			// crossfade animation cmd: crossfade to another animation
    PlayEffect,           		// play a particle effect cmd: play an effect on a dummy position
    StopEffect,					// stop a particle effect cmd: stop a specific effect with name
    PlaySfx,					// play sound cmd: play a sfx
    iTweenRotate,				// iTween Rotate utility
    iTweenMove,					// iTween Move utility
    iTweenStop,					// Stop iTween cmd: stop all iTween aniamtion
    ShakePosition,				// Shake position cmd: iTween Shake utility
    OrientToTarget,				// Orient to target cmd: face to an object within a second, the object(target) is assigned by its own AI
    LaunchAmmo,					// launch ammo: turret fire bullets or missiles
    CallFunction,				// Callback function cmd: sendMessage to current object
    Destroy,					// Destroy cmd: destroy this gameObject, be careful!
    UpdateAI,					// Update PathAI and Update AttackAI if available
    WaitRandomTime,				// Wait Random Time cmd: wait a second then do next steps
    ShakeCamera,				// Shake camera cmd
    Skill_DoEffect,				// Produce Skill Buff effect, like increasing damage, increasing movement etc, 
    Skill_PlayBuffEffect,		// Display special visual effect for Skill Buff.
    GoToState,					// Goto Another state cmd, jump to other state
    WaitCustomTime,				// Wait customized time: the time is given by outside
    Skill_LaunchAmmo,			// Launch Skill Ammo: for special skill effect
    MoveToTarget,				// MoveTowardsTarget: ignore any obstacles, move the X,Z position besides the target
    DoHeal,						// Do heal behavior: for healer units      
}

public class InuDefine
{
    public enum EInuType
    {
        eNone = 0,
        eTrigger,
        eUnit,
        eSubBoss,
        eFinalBoss,
        eNpc,
    }

    public enum EInuState
    {
        eIdle = 0, // default state, no AI, no moving
        eStandby,
        ePatrol,
        eAlert,
        eAttack,
        eDestroyed,

        eCastSkill1, // for cast skill 1
        eCastSkill2, // for cast skill 2
        eCastSkill3, // for cast skill 3
        eCastSkill4, // for cast skill 4 (backup for future)
        eCastSkill5, // for cast skill 5 (backup for future)

        eCount,
    }

    public enum EInuAmmoType
    {
        eBullet = 0, 	// attack instantly
        eProjectile,   	// attack by visible projectiles in a trajectory
        eLaser,			// attack by visible laser in a line
        eBulletLine,	// attack by visible line (must be LineRenderer)
        eRay,			// attack by a unlimited ray       
    }

    public enum EAxisStrict
    {
        none = 0,
        x,
        y,
        z
    }

    public enum EInuAIType
    {
        eNone = 0,
        eDefault,//1       
    }
}