using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class InuAIManager
{
    public bool PauseGame = false;

    static InuAIManager sInstance;
    public static InuAIManager instance
    {
        get
        {
            if (sInstance == null)
            {
                sInstance = new InuAIManager();
            }
            return sInstance;
        }
    }

    public void Update()
    {
        if (PauseGame)
            return;
    }

    // detector AI list
    List<InuAI> mList_DetectorDefend = new List<InuAI>();

    #region Path AI interfaces

    //public InuAI_Path CreatePathAI(InuStructure src, InuDefine.EInuPathAI aiType, InuFloat speed, bool inAir, bool isDefense)
    //{
    //    InuAI_Path ai = new InuAI_Path(src, aiType, speed, inAir, isDefense);
    //    return ai;
    //}

    //public void DestroyPathAI(InuAI_Path _ai)
    //{
    //    if (_ai == null)
    //        return;
    //    _ai.Destroy();
    //    mList_Path.Remove(_ai);
    //}

    #endregion   

    #region Defense AI interfaces
    public InuAI_Attack CreateDefenseAI(InuStructure src,
                                        InuDefine.EInuAIType aiType, 
                                        InuFloat attackRange,
                                        InuFloat sightRange
        )
    {
        InuAI_Attack ai = null;

        //ai = new InuAI_UnitAttack(src, attackScale, aiType, pickTargetAiType, attackRange, sightRange);
        return ai;
    }
    #endregion

    public void Clear()
    {
        mList_DetectorDefend.Clear();
    }
}
