
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
    public int PlayerGetPoint(int playerDBId)
    {
        return playerDB[playerDBId].GetPoint();
    }
    public void PlayerSetPoint(int playerDBId, int inputPoint)
    {
        playerDB[playerDBId].SetPoint(inputPoint);
    }
    public void PlayerAddPoint(int playerDBId, int inputPoint)
    {
        playerDB[playerDBId].AddPoint(inputPoint);
    }
    public void PlayerSubPoint(int playerDBId, int inputPoint)
    {
        playerDB[playerDBId].SubPoint(inputPoint);
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
    public void PlayerPositionSync(int playerDBId)
    {
        playerDB[playerDBId].PositionSyncGlobal();
    }
    //플레이어 회전 정보 동기화 함수 라우팅
    public void PlayerRotationSync(int playerDBId)
    {
        playerDB[playerDBId].RotationSyncGlobal();
    }

    //플레이어 발소리 재생 관련 함수 라우팅
    public void PlayerWalkSoundPlay(int playerDBId)
    {
        playerDB[playerDBId].WalkSoundPlayGlobal();
    }
    public void PlayerRunSoundPlay(int playerDBId)
    {
        playerDB[playerDBId].RunSoundPlayGlobal();
    }
    public void PlayerLandingSoundPlay(int playerDBId)
    {
        playerDB[playerDBId].LandingSoundPlayGlobal();
    }
    public void PlayerHardLandingSoundPlay(int playerDBId)
    {
        playerDB[playerDBId].HardLandingSoundPlayGlobal();
    }
}