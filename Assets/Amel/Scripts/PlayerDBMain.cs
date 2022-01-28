
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
        private byte tempId = 0;
        private string tempMsg = "";

        public void SetPlayerList()
        {
            VRCPlayerApi.GetPlayers(playerList);
            playerCount = (byte)VRCPlayerApi.GetPlayerCount();
        }
        public VRCPlayerApi[] GetPlayerList()
        {
            return playerList;
        }
        public byte GetPlayerCount()
        {
            return playerCount;
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
            tempMsg = "[0] " + playerList[0];
            for (byte i = 1; i < playerCount; i++)
            {
                tempId = (byte)playerList[i].playerId;
                tempMsg += "\n[" + tempId.ToString() + "] " + playerList[tempId].displayName;
            }
            return tempMsg;
        }
    }
}
