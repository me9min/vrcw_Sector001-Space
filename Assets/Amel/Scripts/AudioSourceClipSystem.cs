
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace AmelCustomScripts
{
    public class AudioSourceClipSystem : UdonSharpBehaviour
    {
        [Header("오디오 설정")]
        [Tooltip("오디오 재생기 위치")]
        public Transform playAudioPosition;
        [Tooltip("오디오 소스 (재생기)")]
        public AudioSource playAudio;
        [Tooltip("오디오 클립")]
        public AudioClip[] audioClip;
        
        public void RandomPlay()
        {
            playAudio.clip = audioClip[Random.Range(0, audioClip.Length-1)];
            playAudio.Play();
        }

        public void Stop()
        {
            playAudio.Stop();
        }

        public bool IsPlaying()
        {
            return playAudio.isPlaying;
        }
    }
}