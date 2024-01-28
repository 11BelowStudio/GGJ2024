using UnityEngine;

namespace Scripts.Utils.Placeholders
{
    public interface IAudioHolder
    {
        /// <summary>
        /// Attempts to obtain a random audio clip that may or may not exist
        /// </summary>
        /// <param name="audioClip">will hold the random audio clip if any exist</param>
        /// <returns>true if it could return an audio clip, false otherwise.</returns>
        public bool TryGetRandomAudioClip(out AudioClip audioClip);

        /// <summary>
        /// Attempts to obtain the first audio clip that may or may not exist
        /// </summary>
        /// <param name="audioClip">will hold the 0th audio clip if it exists</param>
        /// <returns>true if it could return an audio clip, false otherwise.</returns>
        public bool TryGetAudioClip(out AudioClip audioClip);

        /// <summary>
        /// Attempts to obtain the audio clip at the desired index (or the highest index one if that index doesnt exist)
        /// </summary>
        /// <param name="audioClip">will hold the 0th audio clip if it exists</param>
        /// <returns>true if it could return an audio clip, false otherwise.</returns>
        public bool TryGetSpecificAudioClip(out AudioClip audioClip, int desiredIndex);


    }
}
