using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InuAI_UnitAttack : InuAI_Attack
{
    public InuDefine.EInuAIType mAIType;

    protected InuFloat mAttackRange;
    protected InuFloat mAttackRangeSqr;

    protected InuFloat mSightRange = new InuFloat(5);
    protected InuFloat mSightRangeSqr;
    public InuAI_UnitAttack(InuStructure _t, InuDefine.EInuAIType _aiType, InuFloat _range, InuFloat _sight)
    {
        mInuObj = _t;
        mTarget = null;
        mAIType = _aiType;
        mAttackRange = _range;
        mAttackRangeSqr = mAttackRange * mAttackRange;
        mSightRange = _sight;
        mSightRangeSqr = mSightRange * mSightRange;
    }

    // return the attack target
    public override InuStructure GetAttackTarget()
    {
        return mAttackTarget;
    }

    // return the focus target
    public override InuStructure GetFocusTarget()
    {
        if (null != mAttackTarget)
            return mAttackTarget;
        return mTarget;
    }
    public override void SeekTarget()
    {
        LocateTarget();
    }

    #region Seeking AI

	
	// get the focus-on target and attack-on target
    protected virtual void LocateTarget()
    {
        ClearTarget();
        
        AI_LocateOffenseUnitNearest_AllScale();
               
        if (mAttackTarget != null)
        {
            mState = InuAI_Attack.AttackState.eSeeTargetAndCanAttack;
            mTarget = mAttackTarget;
        }
        // can we see anything
        else if (mTarget != null)
        {
            mState = InuAI_Attack.AttackState.eSeeTargetAndCannotAttack;
        }
        // we cannot see anything and attack anything, find the nearest one out of sight
        else
        {
            mState = InuAI_Attack.AttackState.eSeeNothingAndNoTarget;
        }
    }
    #endregion

    void AI_LocateOffenseUnitNearest_AllScale()
    {

    }


    public override bool CheckAttackingTargetIsInAttackRange()
    {
        return false;
    }

    public override bool CheckTargetIsStillAvailable()
    {
        return false;
    }

    public override bool CheckTargetIsInAttackRange()
    {
        return false;
    }

    public override bool CheckAttackingTargetIsStillAvailable()
    {
        return false;
    }

    public override void UpdateAI()
    {
 
    }
}
