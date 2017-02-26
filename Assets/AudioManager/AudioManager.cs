using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Main class to handle audio related work
/// This class initiate sounds playing and manages audio source object pools
/// </summary>
public class AudioManager
{
    public static AudioManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new AudioManager();
                AudioManagerComponent.CloneFromResources();
            }
            return _instance;
        }
    }

    public Dictionary<AudioChannel, List<PlayingEvent>> PlayingDB
    {
        get { return _playing; }
    }

    private static AudioManager _instance;

    private AudioManagerComponent _component;
    private AudioDatabase _db;
    private Dictionary<AudioChannel, List<PlayingEvent>> _playing;

    public void Init(AudioManagerComponent component)
    {
        _component = component;
        _db = _component.Database;

        _playing = new Dictionary<AudioChannel, List<PlayingEvent>>();
        for (int i = 0; i < _db.Events.Length; ++i)
        {
            if (!_playing.ContainsKey(_db.Events[i].Channel))
            {
                _playing.Add(_db.Events[i].Channel, new List<PlayingEvent>());
            }
        }
    }

    public void Log(string message)
    {
        if (_db.LogToConsole)
        {
            Debug.Log(string.Format("[AudioManager] - {0}", message));
        }
    }

    public void PlayEvent(string name)
    {
        PlayEvent(name, 0.0f);
    }

    public void PlayEvent(string name, float delay)
    {
        if (Mathf.Approximately(delay, 0.0f))
        {
            AudioEventBase e = _db.FindEventDataByName(name);
            if (e != null)
            {
                e.Play();
                Log(string.Format("Started playing event {0}", name));
            }
            else
            {
                Log(string.Format("Could not find audio event: {0}", name));
            }
        }
        else
        {
            _component.StartCoroutine(DelayedPlayEvent(name, delay));
        }
    }

    public Coroutine StartCoroutine(IEnumerator routine)
    {
        return _component.StartCoroutine(routine);
    }

    private IEnumerator DelayedPlayEvent(string name, float delay)
    {
        Log(string.Format("Delaying event {0} by {1} secs", name, delay));
        yield return new WaitForSeconds(delay);
        PlayEvent(name);
    }
}