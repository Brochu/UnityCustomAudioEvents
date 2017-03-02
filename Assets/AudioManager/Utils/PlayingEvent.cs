using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using BrocAudio.Utils;

namespace BrocAudio
{
    public class PlayingEvent
    {
        public AudioEventBase Event;
        public float StartTime;
        public List<PooledAudioSource> Sources;

        public PlayingEvent(AudioEventBase eventRef, float start)
        {
            Event = eventRef;
            StartTime = start;
            Sources = new List<PooledAudioSource>();
        }
    }
}