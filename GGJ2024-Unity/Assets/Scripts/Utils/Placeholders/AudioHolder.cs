using System;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Utils.Placeholders
{
    [CreateAssetMenu(menuName ="Placeholders/AudioHolder",fileName ="AudioHolder")]
    public class AudioHolder : ScriptableObject, IAudioHolder
    {
        /// <summary>
        /// all of the audio clips that may or may not exist.
        /// </summary>
        [SerializeField] public List<AudioClip> audioClips = new List<AudioClip>();

        /// <summary>
        /// Attempts to obtain a random audio clip that may or may not exist
        /// </summary>
        /// <param name="audioClip">will hold the random audio clip if any exist</param>
        /// <returns>true if it could return an audio clip, false otherwise.</returns>
        public bool TryGetRandomAudioClip(out AudioClip audioClip)
        {
            audioClip = null;
            if (audioClips.Count == 0)
            {
                return false;
            }
            if (audioClips.Count == 1)
            {
                audioClip = audioClips[0];
                return true;
            }
            else
            {
                audioClip = audioClips[UnityEngine.Random.Range(0, audioClips.Count)];
                return true;
            }
        }

        /// <summary>
        /// Attempts to obtain the first audio clip that may or may not exist
        /// </summary>
        /// <param name="audioClip">will hold the 0th audio clip if it exists</param>
        /// <returns>true if it could return an audio clip, false otherwise.</returns>
        public bool TryGetAudioClip(out AudioClip audioClip)
        {
            audioClip = null;
            if (audioClips.Count == 0)
            {
                return false;
            }
            else
            {
                audioClip = audioClips[0];
                return true;
            }
        }

        /// <summary>
        /// Attempts to obtain the audio clip at the desired index (or the highest index one if that index doesnt exist)
        /// </summary>
        /// <param name="audioClip">will hold the 0th audio clip if it exists</param>
        /// <returns>true if it could return an audio clip, false otherwise.</returns>
        public bool TryGetSpecificAudioClip(out AudioClip audioClip, int desiredIndex)
        {
            audioClip = null;
            if (audioClips.Count == 0)
            {
                return false;
            }
            else
            {
                audioClip = audioClips[Mathf.Min(desiredIndex, audioClips.Count - 1)];
                return true;
            }
        }

        // This function is called when the ScriptableObject script is started.
        private void Awake()
        {

            RemoveNulls();

        }

        /// <summary>
        /// this function ensures that the audio clip list is completely null-free.
        /// </summary>
        private void RemoveNulls()
        {
            
            List<AudioClip> _tempList = new List<AudioClip>();

            foreach (AudioClip clip in audioClips)
            {
                if (clip != null)
                {
                    _tempList.Add(clip);
                }
            }

            audioClips.Clear();
            audioClips.AddRange(_tempList);
        }

        // This function is called when the scriptable object will be destroyed.
        private void OnDestroy()
        {
        }

        // This function is called when the scriptable object goes out of scope.
        private void OnDisable()
        {
        }

        // This function is called when the object is loaded.
        private void OnEnable()
        {
            RemoveNulls();
        }

#if UNITY_EDITOR
        // Editor-only function that Unity calls when the script is loaded or a value changes in the Inspector.
        private void OnValidate()
        {
        }

        // Reset to default values.
        private void Reset()
        {
        }
#endif
    }

}
