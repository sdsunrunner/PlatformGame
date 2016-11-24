
using System;
using UnityEngine;
public class StartAppMacroCommand : MacroCommand
{
    public StartAppMacroCommand()
    {
        Debug.LogError("----StartAppMacroCommand");
        //注册命令
        addSubCommand(typeof(StartApplicationCommand));
    }
}

