using UnityEngine;
using System.Collections;

using BrocAudio.Utils;

namespace BrocAudio
{
    [CreateAssetMenu(fileName = "PlayRandomAudioEvent", menuName = "ScriptableObject/PlayRandomAudioEvent")]
    public class PlayRandomAudioEvent : AudioEventBase
    {
        [SerializeField]
        private AudioClip[] _clips;
        [SerializeField]
        private bool _specifyWeights;
        [SerializeField][Tooltip("The sum of the given weights must equal 1.0")]
        private float[] _givenWeights;

        //TEMP: to replace with the duration of the selected clip
        [SerializeField]
        private float _duration;

        private Coroutine _playingRoutine;

        public override PlayingEvent Play()
        {
            // Setting weights if needed
            float[] weights = _givenWeights;
            if (!_specifyWeights || _clips.Length != _givenWeights.Length)
            {
                weights = new float[_clips.Length];
                for (int i = 0; i < _clips.Length; ++i)
                {
                    weights[i] = 1.0f / (float)_clips.Length;
                }
            }

            Debug.Log(string.Format("Playing the audio event -> {0}", _eventName));

            PlayingEvent e = new PlayingEvent(this, Time.time);
            PooledAudioSource pooled = AudioManager.Instance.PoolSource(_channel);
            pooled.Source.clip = SelectRandomClip(weights);
            e.Sources.Add(pooled);
            _playing.Add(e);
            _playingRoutine = AudioManager.Instance.StartCoroutine(WaitForEndOfEvent(e));

            return e;
        }

        public override void StopNewest()
        {
        }

        public override void StopOldest()
        {
        }

        public override void Stop(PlayingEvent e)
        {
        }

        private AudioClip SelectRandomClip(float[] weights)
        {
            float rng = Random.value;
            float current = 0.0f;
            for (int i = 0, maxi = weights.Length; i < maxi; ++i)
            {
                current += weights[i];
                if (rng < current)
                {
                    return _clips[i];
                }
            }

            Debug.LogWarning(string.Format("[{0}]({1}) Could not select a random clip ... something went wrong",
                "PlayRandomAudioEvent",
                _eventName));
            return null;
        }

        private IEnumerator WaitForEndOfEvent(PlayingEvent e)
        {
            yield return new WaitForSeconds(_duration);
            Debug.Log(string.Format("Duration of event -> {0} done", _eventName));
            Stop(e);
        }
    }
}