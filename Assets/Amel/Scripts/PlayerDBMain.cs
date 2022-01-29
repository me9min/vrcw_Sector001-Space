
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace AmelCustomScripts
{
    //이 클래스의 기능은 명령함수(플레이어ID)를 받아 명령을 라우팅 해줍니다
    public class PlayerDBMain : UdonSharpBehaviour
    {
        [Header("플레이어DB할당")]
        public PlayerDB[] playerDB;

        private VRCPlayerApi[] playerList = new VRCPlayerApi[256]; //플레이어 리스트 로컬저장 byte크기만큼256개할당 0~255
        private byte playerCount = 0; //플레이어수 로컬저장
        private string tempMsg = "";

        public void SetPlayerList()
        {
            VRCPlayerApi.GetPlayers(playerList);
            playerCount = (byte)VRCPlayerApi.GetPlayerCount();
        }

        public int PlayerGetPoint(byte playerDBId)
        {
            return playerDB[playerDBId].GetPoint();
        }
        public void PlayerSetPoint(byte playerDBId, int inputPoint)
        {
            playerDB[playerDBId].SetPoint(inputPoint);
        }
        public void PlayerAddPoint(byte playerDBId, int inputPoint)
        {
            playerDB[playerDBId].AddPoint(inputPoint);
        }
        public void PlayerSubPoint(byte playerDBId, int inputPoint)
        {
            playerDB[playerDBId].SubPoint(inputPoint);
        }

        public void PlayerPositionSync(byte playerDBId)
        {
            playerDB[playerDBId].PositionSyncGlobal();
        }
        public void PlayerRotationSync(byte playerDBId)
        {
            playerDB[playerDBId].RotationSyncGlobal();
        }

        public void PlayerWalkSoundPlay(byte playerDBId)
        {
            playerDB[playerDBId].WalkSoundPlayGlobal();
        }
        public void PlayerRunSoundPlay(byte playerDBId)
        {
            playerDB[playerDBId].RunSoundPlayGlobal();
        }
        public void PlayerLandingSoundPlay(byte playerDBId)
        {
            playerDB[playerDBId].LandingSoundPlayGlobal();
        }
        public void PlayerHardLandingSoundPlay(byte playerDBId)
        {
            playerDB[playerDBId].HardLandingSoundPlayGlobal();
        }

        //플레이어리스트 UI 텍스트 만들기
        public string MakePlayerListMsg()
        {
            //임시메세지 저장소 초기화
            tempMsg = "";
            //리스트메세지 만들기(플레이어수 만큼 반복)
            for (byte i = 0; i < playerCount; i++)
            {
                tempMsg += "[" + playerList[i].playerId.ToString() + "] " + playerList[i].displayName + "\n";
            }
            return tempMsg;
        }
    }
}