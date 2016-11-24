using System;

public class SuperCommand : ICommand
{
    public virtual void excute(INotification note)
	{
			
	}

    public void notify(String typeStr, Object data = null)
	{
        BaseNotification notification = new BaseNotification();
        notification.data = data;

        AppNotification note = new AppNotification(typeStr, notification);
			
		note.dispatch();
	}
}

