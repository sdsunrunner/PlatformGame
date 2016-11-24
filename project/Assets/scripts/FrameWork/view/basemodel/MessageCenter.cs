using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
  
public class MessageCenter : IMessageCenter
{
    public Object msg = null;
    public String msgName = "";
    public List<IObserver> observerList = null;

    public void register(IObserver observer)
    {
        if (null == observerList)
        {
            observerList = new List<IObserver>();
        }
        observerList.Add(observer);
    }

    public void unRegister(IObserver observer)
    {
        int itemIndex = observerList.LastIndexOf(observer);
        if (observerList != null)
            observerList.RemoveAt(itemIndex);
    }

    public void unRegisterAll()
    {
        if (null != observerList)
            observerList.Clear();
    }

    public void notify()
    {
        IObserver observer = null;

        if (null != observerList && observerList.Count > 0)
        {
            for (int i = 0; i < observerList.Count; i++)
            {
                observer = observerList[i];
                if (null != observer)
                    observer.infoUpdate(msg, msgName);
            }
        }
        else
        {
                
        }
    }
}

