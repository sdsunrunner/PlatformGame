using System;
using System.Collections.Generic;
public class MacroCommand : ICommand
{
    private IList<Type> m_subCommands;

    public MacroCommand()
    {
        m_subCommands = new List<Type>();
        initializeMacroCommand();
    }

    public void addSubCommand(Type commandType)
    {
        m_subCommands.Add(commandType);
    }

    public void excute(INotification notification)
    {
        while (m_subCommands.Count > 0)
        {
            Type commandType = m_subCommands[0];
            object commandInstance = Activator.CreateInstance(commandType);

            if (commandInstance is ICommand)
            {
                ((ICommand)commandInstance).excute(notification);
            }

            m_subCommands.RemoveAt(0);
        }
    }

    protected virtual void initializeMacroCommand()
    {

    }
}