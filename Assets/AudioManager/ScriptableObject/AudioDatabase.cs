using UnityEngine;
using System.Collections;
using System.Linq;

[CreateAssetMenu(fileName = "AudioDatabase", menuName = "ScriptableObject/AudioDatabase")]
public class AudioDatabase : ScriptableObject
{
    [System.Serializable]
    public struct ChannelPool
    {
        public AudioChannel Channel;
        public int MaxCount; 
    }

    public AudioEventBase[] Events
    {
        get { return _events; }
    }

    public AudioSource BaseSource
    {
        get { return _baseSource; }
    }

    public bool LogToConsole
    {
        get { return _logToConsole; }
    }

    [SerializeField]
    private AudioEventBase[] _events;
    [SerializeField]
    private ChannelPool[] _poolsInfo;
    [SerializeField]
    private AudioSource _baseSource;
    [SerializeField]
    private bool _logToConsole = true;

    public int GetMaxCountForChannel(AudioChannel c)
    {
        ChannelPool pool = _poolsInfo.Where(p => p.Channel == c).FirstOrDefault();
        return pool.MaxCount;
    }

    public AudioEventBase FindEventDataByName(string eventName)
    {
        for (int i = 0; i < _events.Length; ++i)
        {
            if (_events[i].EventName.Equals(eventName))
            {
                return _events[i];
            }
        }

        return null;
    }
}