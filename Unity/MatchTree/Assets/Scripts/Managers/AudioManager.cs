using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace Manager
{
    public class AudioManager : MonoBehaviour
    {
        public enum audioClipEnum { selected, deselected, collected};
        audioClipEnum availableAudioClips;

        List<AudioSource> audioSources = new List<AudioSource>();

        [SerializeField]
        AudioClip selected;

        [SerializeField]
        AudioClip deselected;

        [SerializeField]
        AudioClip collected;

        [SerializeField]
        AudioMixerGroup effectsGroup;

        // Use this for initialization
        void Start()
        {
            for (int i = 0; i < 5; i++) {
                AudioSource _newSource = CreateAudioSource();
                audioSources.Add(_newSource);
            }
        }

        // <summary>
        // Play a audio clip available in the audioclip enum
        // </summary>
        public void PlayAudioClip(audioClipEnum _audioclip)
        {
            AudioSource _source = GetAvailableSource();

            switch (_audioclip) {
                case audioClipEnum.selected:
                    _source.clip = selected;
                    break;
                case audioClipEnum.deselected:
                    _source.clip = deselected;
                    break;
                case audioClipEnum.collected:
                    _source.clip = collected;
                    break;
                default:
                    return;
            }
            _source.Play();
        }

        // <summary>
        // Get a audio source that is not in use, and if all are in use, instantiate a new one
        // </summary>
        private AudioSource GetAvailableSource()
        {
            for (int i = 0; i < audioSources.Count; i++)
            {
                if (audioSources[i].isPlaying == false)
                {
                    return audioSources[i];
                }
            }
            AudioSource _newSource = CreateAudioSource();
            audioSources.Add(_newSource);

            return _newSource;
        }

        // <summary>
        // Create a new audiosource to the audiosource pool
        // </summary>
        private AudioSource CreateAudioSource()
        {
            GameObject _sourceObject = new GameObject();
            _sourceObject.name = "AudioSourceObject";
            _sourceObject.transform.parent = transform;
            AudioSource _newSource = _sourceObject.AddComponent<AudioSource>();
            _newSource.outputAudioMixerGroup = effectsGroup;

            return _newSource;
        }
    }
}
