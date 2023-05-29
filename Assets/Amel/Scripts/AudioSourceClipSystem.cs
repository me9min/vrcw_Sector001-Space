
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
public class AudioSourceClipSystem : UdonSharpBehaviour
{
    [Header("오디오 설정")]
    [Tooltip("오디오 재생기")]
    public AudioSource playAudio;
    [Tooltip("오디오 소스 클립")]
    public AudioClip[] audioClips;
    
    public void RandomPlay()
    {
        playAudio.clip = audioClips[Random.Range(0, audioClips.Length)];
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