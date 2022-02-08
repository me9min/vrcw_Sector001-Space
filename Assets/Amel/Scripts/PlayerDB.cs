
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

    private int point = 0;

    //플레이어 컴뱃시스템관련 함수
    public void Damage1Global()
    {
        SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "Damage1");
    }
    public void Damage10Global()
    {
        SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "Damage10");
    }
    public void Damage20Global()
    {
        SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "Damage20");
    }
    public void KillGlobal()
    {
        SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "Kill");
    }

    public void Damage1()
    {
        if (playerDBMain.isPlayerSetted)
        {
            VRCPlayerApi player;
            player = playerDBMain.GetPlayerBySeq(playerDBSeq);
            player.CombatSetCurrentHitpoints(player.CombatGetCurrentHitpoints()-1);
        }
    }
    public void Damage10()
    {
        if (playerDBMain.isPlayerSetted)
        {
            VRCPlayerApi player;
            float tempHp;
            player = playerDBMain.GetPlayerBySeq(playerDBSeq);
            tempHp = player.CombatGetCurrentHitpoints() - 10;
            if (tempHp > 0)
            {
                player.CombatSetCurrentHitpoints(tempHp);
            }
            else
            {
                player.CombatSetCurrentHitpoints(0);
            }
        }
    }
    public void Damage20()
    {
        if (playerDBMain.isPlayerSetted)
        {
            VRCPlayerApi player;
            float tempHp;
            player = playerDBMain.GetPlayerBySeq(playerDBSeq);
            tempHp = player.CombatGetCurrentHitpoints() - 20;
            if (tempHp > 0)
            {
                player.CombatSetCurrentHitpoints(tempHp);
            }
            else
            {
                player.CombatSetCurrentHitpoints(0);
            }
        }
    }
    public void Kill()
    {
        if (playerDBMain.isPlayerSetted)
        {
            playerDBMain.GetPlayerBySeq(playerDBSeq).CombatSetCurrentHitpoints(0);
        }
    }

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
    public int GetPoint()
    {
        return point;
    }

    public void SetPointReq(int inputPoint)
    {
        
    }
    public void AddPointReq(int inputPoint)
    {
        
    }
    public void SubPointReq(int inputPoint)
    {
        
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