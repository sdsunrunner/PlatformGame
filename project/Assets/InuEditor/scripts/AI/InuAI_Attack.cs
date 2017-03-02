using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class InuAI_Attack : InuAI
{   
    public enum AttackState
    {
        eSeeNothingAndNoTarget = 0, // no focus target, no attack target
        eSeeTargetAndCannotAttack,  // has focus target, no attack target
        eSeeTargetAndCanAttack,     // has focus target, has attack target        
    }

    // the target focused on
    protected InuStructure mTarget;
    // the target gonna to attack on
    protected InuStructure mAttackTarget;
    protected AttackState mState = AttackState.eSeeNothingAndNoTarget;
    public AttackState GetAttackState()
    {
        return mState;
    }
    public virtual void ClearTarget()
    {
        mAttackTarget = null;
        mTarget = null;
        mState = AttackState.eSeeNothingAndNoTarget;
    }

    // return the attack target
    public abstract InuStructure GetAttackTarget();

    // return the focus target
    public abstract InuStructure GetFocusTarget();

    // seeking...
    public abstract void SeekTarget();

    // checking the target is still available
    public abstract bool CheckTargetIsStillAvailable();

    // checking the target is in attack range
    public abstract bool CheckTargetIsInAttackRange();

    // checking the attacking target is still available
    public abstract bool CheckAttackingTargetIsStillAvailable();

    // checking the attacking target is still in attack range
    public abstract bool CheckAttackingTargetIsInAttackRange();

    // Update
    public abstract void UpdateAI();

}

