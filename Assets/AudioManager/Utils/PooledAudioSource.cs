using UnityEngine;
using System.Collections;

namespace BrocAudio.Utils
{
    public class PooledAudioSource
    {
        public AudioChannel Channel;
        public AudioSource Source;
        public float TimeStamp;
        public bool InUse;
    }
}