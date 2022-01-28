
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace AmelCustomScripts
{
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

        public void PlayerPositionSync(byte playerDBId)
        {
            playerDB[playerDBId].PositionSyncGlobal();
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
            //VRCPlayerApi.GetPlayers(playerList);
            //playerCount = (byte)VRCPlayerApi.GetPlayerCount();
            for (byte i = 0; i < playerCount; i++)
            {
                tempMsg += "[" + playerList[i].playerId.ToString() + "] " + playerList[i].displayName + "\n";
            }
            return tempMsg;
        }
    }
}