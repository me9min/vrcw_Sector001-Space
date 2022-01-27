
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace AmelCustomScripts
{
    public class PlayerDB : UdonSharpBehaviour
    {
        [Header("플레이어 발소리 설정")]
        [Tooltip("걷는 발걸음 소리")]
        public AudioSourceClipSystem walkSoundClip;
        [Tooltip("달리는 발걸음 소리")]
        public AudioSourceClipSystem runSoundClip;
        [Tooltip("착지 소리")]
        public AudioSourceClipSystem landingSoundClip;
        [Tooltip("강한 착지 소리")]
        public AudioSourceClipSystem hardLandingSoundClip;

        public void WalkSoundPlayGlobal()
        {
            if (!walkSoundClip.IsPlaying() && !runSoundClip.IsPlaying())
            {
                //걷는소리 play 모두에게 전송
                SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "WalkSoundPlay");
            }
        }
        public void RunSoundPlayGlobal()
        {
            if (!walkSoundClip.IsPlaying() && !runSoundClip.IsPlaying())
            {
                //달리는소리 play 모두에게 전송
                SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "RunSoundPlay");
            }
        }
        public void LandingSoundPlayGlobal()
        {
            //착지소리 play 모두에게 전송
            SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "LandingSoundPlay");
        }
        public void HardLandingSoundPlayGlobal()
        {
            //강한착지소리 play 모두에게 전송
            SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "HardLandingSoundPlay");
        }

        public void WalkSoundPlay()
        {
            walkSoundClip.RandomPlay();
        }
        public void RunSoundPlay()
        {
            runSoundClip.RandomPlay();
        }
        public void LandingSoundPlay()
        {
            landingSoundClip.RandomPlay();
        }
        public void HardLandingSoundPlay()
        {
            hardLandingSoundClip.RandomPlay();
        }
    }
}