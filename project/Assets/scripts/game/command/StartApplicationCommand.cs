using System;
using UnityEngine;
public class StartApplicationCommand : SuperCommand
{
    private AppFacade _facade = AppFacade.instance;		
//==============================================================================
// Public Functions
//==============================================================================
    public override void excute(INotification note)
    {
        base.excute(note);
        this.addCommandForSys();
        this.addCommandForShowView();
    }

    void addCommandForSys()
    {
        Debug.LogError("--------------addCommandForSys----------");
    }

    void addCommandForShowView()
    {
        Debug.LogError("--------------addCommandForShowView----------");
    }
}

