using System;
public class BaseNotification : INotification
{
    private Object _data = null;      

    public object data
    {
        get
        {
            return this._data;
        }
        set 
        {
            this._data = value;
        }
    }
}

