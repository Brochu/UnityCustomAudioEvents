using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using BrocAudio.Utils;

namespace BrocAudio
{
    /// <summary>
    /// Monobehaviour to handle scene logic for the audio manager
    /// </summary>
    public class AudioManagerComponent : MonoBehaviour
    {
        public AudioDatabase Database
        {
            get { return _database; }
        }

        [SerializeField]
        private AudioDatabase _database;

        private GameObject[] _poolGOs;
        private Dictionary<AudioChannel, List<PooledAudioSource>> _poolSources;
        private const string RESOURCES_PATH = "AudioManager";
        private const string SOURCE_NAME_FORMAT = "[{0}] Source (POOLED)";
        private const string SOURCE_NAME_FORMAT_INUSE = "[{0}] Source (INUSE)";

        protected void Awake()
        {
            DontDestroyOnLoad(this.gameObject);

            // Creating base buckets for audio sources pooling
            _poolGOs = new GameObject[Enum.GetValues(typeof(AudioChannel)).Length];
            _poolSources = new Dictionary<AudioChannel, List<PooledAudioSource>>();

            foreach (AudioChannel channel in Enum.GetValues(typeof(AudioChannel)))
            {
                _poolSources.Add(channel, new List<PooledAudioSource>());

                GameObject go = new GameObject(channel.ToString());
                go.transform.SetParent(this.transform, false);

                for (int i = 0; i < _database.GetMaxCountForChannel(channel); ++i)
                {
                    AudioSource source;
                    if (_database.BaseSource != null)
                    {
                        source = Instantiate(_database.BaseSource);
                        source.name = string.Format(SOURCE_NAME_FORMAT, channel.ToString());
                    }
                    else
                    {
                        source = new GameObject(string.Format(SOURCE_NAME_FORMAT, channel.ToString()), typeof(AudioSource)).GetComponent<AudioSource>();
                    }
                    source.transform.SetParent(go.transform, false);
                    PooledAudioSource pooled = new PooledAudioSource
                    {
                        Channel = channel,
                        Source = source,
                        TimeStamp = float.MinValue,
                        InUse = false
                    };
                    _poolSources[channel].Add(pooled);
                }

                _poolGOs[(int)channel] = go;
            }
        }

        public static void CloneFromResources()
        {
            AudioManager.Instance.Init(Instantiate(Resources.Load<AudioManagerComponent>(RESOURCES_PATH)));
        }

        public GameObject GetChannelPoolRootObject(AudioChannel channel)
        {
            return _poolGOs[(int)channel];
        }

        public PooledAudioSource PoolSource(AudioChannel channel)
        {
            IOrderedEnumerable<PooledAudioSource> usable = _poolSources[channel].Where(p => !p.InUse).OrderBy(p => p.TimeStamp);
            if (usable.Count() <= 0)
            {
                usable = _poolSources[channel].OrderBy(p => p.TimeStamp);
            }

            PooledAudioSource next = usable.First();
            next.Source.name = string.Format(SOURCE_NAME_FORMAT_INUSE, next.Channel.ToString());
            next.InUse = true;
            next.TimeStamp = Time.time;
            return next;
        }

        public void ReturnSource(PooledAudioSource pooled)
        {
            pooled.TimeStamp = float.MinValue;
            pooled.InUse = false;

            pooled.Source.transform.SetParent(_poolGOs[(int)pooled.Channel].transform, false);
            pooled.Source.transform.SetAsLastSibling();
            pooled.Source.name = string.Format(SOURCE_NAME_FORMAT, pooled.Channel.ToString());
        }
    }
}