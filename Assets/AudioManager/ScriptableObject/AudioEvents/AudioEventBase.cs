using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

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

    protected List<PlayingEvent> _playing = new List<PlayingEvent>();

    public abstract PlayingEvent Play();
    public abstract void StopOldest();
    public abstract void StopNewest();
    public abstract void Stop(PlayingEvent e);
}