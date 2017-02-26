using UnityEngine;
using System.Collections;
using System;

[CreateAssetMenu(fileName = "PlaySingleAudioEvent", menuName = "ScriptableObject/PlaySingleAudioEvent")]
public class PlaySingleAudioEvent : AudioEventBase
{
    [SerializeField]
    protected AudioClip _clip;
    [SerializeField]
    protected float _duration;

    public override void Play()
    {
        Debug.Log(string.Format("Playing the audio event -> {0}", _eventName));

        PlayingEvent e = new PlayingEvent { Event = this, StartTime = Time.time };
        AudioManager.Instance.PlayingDB[_channel].Add(e);
        AudioManager.Instance.StartCoroutine(WaitForEndOfEvent(e));
    }

    public override void Stop()
    {
        Debug.Log(string.Format("Stopping the audio event -> {0}", _eventName));
    }

    private IEnumerator WaitForEndOfEvent(PlayingEvent e)
    {
        yield return new WaitForSeconds(_duration);
        AudioManager.Instance.PlayingDB[_channel].Remove(e);
    }
}