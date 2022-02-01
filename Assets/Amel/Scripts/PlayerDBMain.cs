
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

//이 클래스의 기능은 명령함수(플레이어ID)를 받아 명령을 PlayerDB마다 라우팅 해줍니다
[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
public class PlayerDBMain : UdonSharpBehaviour
{
    [Header("플레이어DB할당")]
    public PlayerDB[] playerDB;

    //플레이어 포인트 관련 함수 라우팅
    public int PlayerGetPoint(int playerDBSeq)
    {
        return playerDB[playerDBSeq].GetPoint();
    }
    public void PlayerSetPoint(int playerDBSeq, int inputPoint)
    {
        playerDB[playerDBSeq].SetPoint(inputPoint);
    }
    public void PlayerAddPoint(int playerDBSeq, int inputPoint)
    {
        playerDB[playerDBSeq].AddPoint(inputPoint);
    }
    public void PlayerSubPoint(int playerDBSeq, int inputPoint)
    {
        playerDB[playerDBSeq].SubPoint(inputPoint);
    }

    //플레이어목록대로 오너 업데이트
    /*public void UpdatePlayerDBOwn(VRCPlayerApi[] players, int playerCount)
    {
        for (int i = 0; i < playerCount; i++)
        {
            playerDB[i].Set오너(players[i]);
        }
    }*/
    //플레이어 위치 정보 동기화 함수 라우팅
    public void PlayerPositionSync(int playerDBSeq)
    {
        playerDB[playerDBSeq].PositionSyncGlobal();
    }
    //플레이어 회전 정보 동기화 함수 라우팅
    public void PlayerRotationSync(int playerDBSeq)
    {
        playerDB[playerDBSeq].RotationSyncGlobal();
    }

    //플레이어 발소리 재생 관련 함수 라우팅
    public void PlayerWalkSoundPlay(int playerDBSeq)
    {
        playerDB[playerDBSeq].WalkSoundPlayGlobal();
    }
    public void PlayerRunSoundPlay(int playerDBSeq)
    {
        playerDB[playerDBSeq].RunSoundPlayGlobal();
    }
    public void PlayerLandingSoundPlay(int playerDBSeq)
    {
        playerDB[playerDBSeq].LandingSoundPlayGlobal();
    }
    public void PlayerHardLandingSoundPlay(int playerDBSeq)
    {
        playerDB[playerDBSeq].HardLandingSoundPlayGlobal();
    }
}