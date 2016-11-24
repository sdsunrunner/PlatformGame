using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class AppFacade
{
    private Dictionary<String, Type> _facade = null;
    private static AppFacade _instance = null;

    private AppFacade()
    {
        _facade = new Dictionary<string, Type>();
    }

    //Singleton模式
    public static AppFacade instance
    {
        get
        {
            if (null == _instance)
                _instance = new AppFacade();
            return _instance;
        }
    }

    public void addCommand(String commandKey, Type commandType)
    {
        if (_facade.ContainsKey(commandKey))
            throw new Exception(commandKey + "has been registed");
        _facade[commandKey] = commandType;
    }

    public ICommand instanceCommandByType(String commandKey)
	{
        Type commandType = _facade[commandKey];
        ICommand commandInstance = (ICommand)Activator.CreateInstance(commandType,null);
        return commandInstance;
    }

    public bool hasCommand(string commandType)
	{
        if (_facade.ContainsKey(commandType))
			return true;
		return false;
	}

}

