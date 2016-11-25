using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public abstract class RenderableData<T>
{
    public delegate void RenderableDataValueChangedHandler(T renderableData);
    public event RenderableDataValueChangedHandler OnValueChanged;

    protected void TriggerOnValueChanged(T renderableData)
    {
        if (OnValueChanged != null)
            OnValueChanged(renderableData);
    }
}
