// BaseScriptableObject.cs
using UnityEngine;
 
public abstract class BaseScriptableObject<T> : ScriptableObject
{
    [SerializeField, HideInInspector]
    protected T data;

    public T Data
    {
        get { return data; }
        set { data = value; }
    }
      
}