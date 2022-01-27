
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace AmelCustomScripts
{
    public class PlayerListByHost : UdonSharpBehaviour
    {
        private VRCPlayerApi[] playerListByHost;

        public void SetPlayerListByHost(VRCPlayerApi[] playerList)
        {
            playerListByHost = playerList;
        }

        public VRCPlayerApi[] GetPlayerListByHost()
        {
            return playerListByHost;
        }
    }
}
