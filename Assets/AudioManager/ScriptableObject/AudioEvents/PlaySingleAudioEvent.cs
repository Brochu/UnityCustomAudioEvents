using UnityEngine;

using System.Collections;
using System.Linq;

[CreateAssetMenu(fileName = "PlaySingleAudioEvent", menuName = "ScriptableObject/PlaySingleAudioEvent")]
public class PlaySingleAudioEvent : AudioEventBase
{
    [SerializeField]
    protected AudioClip _clip;
    [SerializeField]
    protected float _duration;

    private Coroutine _playingRoutine;

    public override PlayingEvent Play()
    {
        Debug.Log(string.Format("Playing the audio event -> {0}", _eventName));

        PlayingEvent e = new PlayingEvent { Event = this, StartTime = Time.time };
        _playing.Add(e);
        _playingRoutine = AudioManager.Instance.StartCoroutine(WaitForEndOfEvent());

        return e;
    }

    public override void StopOldest()
    {
        Debug.Log(string.Format("Stopping the oldest of audio event -> {0}", _eventName));
        PlayingEvent toStop = null;
        float min = float.MaxValue;

        for (int i = 0; i < _playing.Count; ++i)
        {
            if (_playing[i].StartTime < min)
            {
                toStop = _playing[i];
                min = _playing[i].StartTime;
            }
        }

        Stop(toStop);
    }

    public override void StopNewest()
    {
        Debug.Log(string.Format("Stopping the newest of audio event -> {0}", _eventName));
        PlayingEvent toStop = null;
        float max = float.MinValue;

        for (int i = 0; i < _playing.Count; ++i)
        {
            if (_playing[i].StartTime > max)
            {
                toStop = _playing[i];
                max = _playing[i].StartTime;
            }
        }

        Stop(toStop);
    }

    public override void Stop(PlayingEvent e)
    {
        if (e == null)
        {
            Debug.Log(string.Format("Could not find the event to stop for -> {0}", _eventName));
            return;
        }

        Debug.Log(string.Format("Ending the audio event -> {0}, returning source(s) to pools", _eventName));
        if (_playingRoutine != null)
        {
            AudioManager.Instance.StopCoroutine(_playingRoutine);
            _playingRoutine = null;
        }
        // Remove audio sources
        // return sources to pools

        _playing.Remove(e);
    }

    private IEnumerator WaitForEndOfEvent()
    {
        yield return new WaitForSeconds(_duration);
        Debug.Log(string.Format("Duration of event -> {0} done", _eventName));
        _playingRoutine = null;
    }
}