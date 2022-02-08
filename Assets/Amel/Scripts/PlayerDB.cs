
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

//플레이어 마다 할당되어 플레이어 추적 및 추가기능 하는 플레이어Tag 같은 오브젝트
[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
public class PlayerDB : UdonSharpBehaviour
{
    [Header("플레이어DB 설정")]
    [Tooltip("플레이어DB메인")]
    public PlayerDBMain playerDBMain;
    [Tooltip("플레이어DB 순서 (반드시 설정, 중복X)")]
    public int playerDBSeq = 0;

    [Header("플레이어 발소리 설정")]
    [Tooltip("걷는 발걸음 소리")]
    public AudioSourceClipSystem walkSoundClip;
    [Tooltip("달리는 발걸음 소리")]
    public AudioSourceClipSystem runSoundClip;
    [Tooltip("착지 소리")]
    public AudioSourceClipSystem landingSoundClip;
    [Tooltip("강한 착지 소리")]
    public AudioSourceClipSystem hardLandingSoundClip;

    [Header("플레이어 초기 소지금")]
    [UdonSynced] public int point = 0;

    public void PositionSyncGlobal()
    {
        SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "PositionSync");
    }
    public void RotationSyncGlobal()
    {
        SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "RotationSync");
    }

    public void PositionSync()
    {
        if (playerDBMain.isPlayerSetted)
        {
            this.transform.position = playerDBMain.GetPlayerBySeq(playerDBSeq).GetPosition();
        }
    }
    public void RotationSync()
    {
        if (playerDBMain.isPlayerSetted)
        {
            this.transform.rotation = playerDBMain.GetPlayerBySeq(playerDBSeq).GetRotation();
        }
    }

    public void WalkSoundPlayGlobal()
    {
        //걷는소리 and 뛰는소리 둘다 플레이 중이 아닐시
        if (!walkSoundClip.IsPlaying() && !runSoundClip.IsPlaying())
        {
            //걷는소리 play 모두에게 전송
            SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "WalkSoundPlay");
        }
    }
    public void RunSoundPlayGlobal()
    {
        //걷는소리 and 뛰는소리 둘다 플레이 중이 아닐시
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
        if (playerDBMain.isPlayerSetted)
        {
            walkSoundClip.RandomPlay();
        }
    }
    public void RunSoundPlay()
    {
        if (playerDBMain.isPlayerSetted)
        {
            runSoundClip.RandomPlay();
        }
    }
    public void LandingSoundPlay()
    {
        if (playerDBMain.isPlayerSetted)
        {
            landingSoundClip.RandomPlay();
        }
    }
    public void HardLandingSoundPlay()
    {
        if (playerDBMain.isPlayerSetted)
        {
            hardLandingSoundClip.RandomPlay();
        }
    }

    //플레이어 포인트관련 함수
    [System.Obsolete]
    public override void OnOwnershipTransferred()
    {
        point = 0;
    }
    public void SyncPoint()
    {
        RequestSerialization();
        SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "SyncPointGlobal");
    }
    public void SyncPointGlobal()
    {
        playerDBMain.tempPlayerSeq = playerDBSeq;
    }
    public int GetPoint()
    {
        return point;
    }
    public void SetPoint(int inputPoint)
    {
        point = inputPoint;
    }
    public void AddPoint(int inputPoint)
    {
        point += inputPoint;
    }
    public void SubPoint(int inputPoint)
    {
        point -= inputPoint;
    }
}