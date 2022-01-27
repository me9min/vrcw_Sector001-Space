
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace AmelCustomScripts
{
    public class PlayerDBMain : UdonSharpBehaviour
    {
        [Header("플레이어DB할당")]
        public GameObject[] playerDBObject;
        public PlayerDB[] playerDB;

        public void PlayerAttach(VRCPlayerApi player)
        {
            //Networking.SetOwner(player, playerDBObject[오브젝트 번호 할당구역 할당방법강구중]); 
        }

        public void PlayerSync(byte playerDBId)
        {
            playerDBObject[playerDBId].transform.position = Networking.GetOwner(playerDBObject[playerDBId]).GetPosition();
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
    }
}
