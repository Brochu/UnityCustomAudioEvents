using UnityEngine;

using System;
using System.Collections;

public abstract class AudioEventBase : ScriptableObject
{
    public AudioChannel Channel
    {
        get { return _channel; }
    }

    public string EventName
    {
        get { return _eventName; }
    }

    [SerializeField]
    protected AudioChannel _channel;
    [SerializeField]
    protected string _eventName;

    public abstract void Play();
    public abstract void Stop();
}