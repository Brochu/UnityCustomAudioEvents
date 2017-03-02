using UnityEngine;
using System.Collections;
using BrocAudio.Utils;

namespace BrocAudio
{
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

        private static AudioManager _instance;

        private AudioManagerComponent _component;
        private AudioDatabase _db;

        public void Init(AudioManagerComponent component)
        {
            _component = component;
            _db = _component.Database;
        }

        public void Log(string message)
        {
            if (_db.LogToConsole)
            {
                Debug.Log(string.Format("[AudioManager] - {0}", message));
            }
        }

        #region Playing Events
        public PlayingEvent PlayEvent(string name)
        {
            return PlayEvent(name, 0.0f);
        }

        public PlayingEvent PlayEvent(string name, float delay)
        {
            if (Mathf.Approximately(delay, 0.0f))
            {
                AudioEventBase e = _db.FindEventDataByName(name);
                if (e != null)
                {
                    PlayingEvent p = e.Play();
                    Log(string.Format("Started playing event {0}", name));
                    return p;
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
            return null;
        }
        #endregion

        #region Stopping Events
        public void StopOldest(string name)
        {
            _db.FindEventDataByName(name).StopOldest();
        }

        public void StopNewest(string name)
        {
            _db.FindEventDataByName(name).StopNewest();
        }

        public void Stop(PlayingEvent e)
        {
            e.Event.Stop(e);
        }
        #endregion

        #region Coroutine functions
        public Coroutine StartCoroutine(IEnumerator routine)
        {
            return _component.StartCoroutine(routine);
        }

        public void StopCoroutine(Coroutine c)
        {
            _component.StopCoroutine(c);
        }
        #endregion

        #region Object pool access
        public PooledAudioSource PoolSource(AudioChannel channel)
        {
            return _component.PoolSource(channel);
        }

        public void ReturnSource(PooledAudioSource pooled)
        {
            _component.ReturnSource(pooled);
        }
        #endregion

        private IEnumerator DelayedPlayEvent(string name, float delay)
        {
            Log(string.Format("Delaying event {0} by {1} secs", name, delay));
            yield return new WaitForSeconds(delay);
            PlayEvent(name);
        }
    }
}