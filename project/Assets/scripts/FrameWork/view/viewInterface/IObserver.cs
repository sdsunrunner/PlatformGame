using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
/*
 * 观察者接口
 */
public interface IObserver
{
    void infoUpdate(object data, string msgName);
}

