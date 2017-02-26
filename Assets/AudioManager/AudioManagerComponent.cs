using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

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

    private Dictionary<AudioChannel, GameObject> _poolGOs;
    private const string RESOURCES_PATH = "AudioManager";
    private const string SOURCE_NAME_FORMAT = "[{0}] Source (POOLED)";

    protected void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        // Creating base buckets for audio sources pooling
        _poolGOs = new Dictionary<AudioChannel, GameObject>();
        foreach (AudioChannel channel in Enum.GetValues(typeof(AudioChannel)))
        {
            GameObject go = new GameObject(channel.ToString());
            go.transform.SetParent(this.transform, false);
            for (int i = 0; i < _database.GetMaxCountForChannel(channel); ++i)
            {
                GameObject source;
                if (_database.BaseSource != null)
                {
                    source = Instantiate(_database.BaseSource).gameObject;
                    source.name = string.Format(SOURCE_NAME_FORMAT, channel.ToString());
                }
                else
                {
                    source = new GameObject(string.Format(SOURCE_NAME_FORMAT, channel.ToString()), typeof(AudioSource));
                }
                source.transform.SetParent(go.transform, false);
            }

            _poolGOs.Add(channel, go);
        }
    }

    public static void CloneFromResources()
    {
        AudioManager.Instance.Init(Instantiate(Resources.Load<AudioManagerComponent>(RESOURCES_PATH)));
    }

    public GameObject GetChannelPoolRootObject(AudioChannel c)
    {
        return _poolGOs[c];
    }
}