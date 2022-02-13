
using UdonSharp;
using UnityEngine;
using VRC.SDK3.Components;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
public class PlayerDBPool : UdonSharpBehaviour
{
    public VRCObjectPool playerDBPool;
    public int playerDBMax = 255;
    /*
    private void Start()
    {
        playerDBPool = ((VRCObjectPool)GetComponent(typeof(VRCObjectPool)));
    }

    //어떤 플레이어가 접속했을때 들어온 플레이어객체를 반환
    public override void OnPlayerJoined(VRCPlayerApi player)
    {
        if (Networking.IsMaster)
        {
            if (player.isLocal) //내가 접속했을때
            {

            }
            else //내가아닌 누군가 접속했을때
            {

            }

            Networking.SetOwner(player, playerDBPool.TryToSpawn());
        }
    }

    //어떤 플레이어가 나갔을때 들어온 플레이어객체를 반환
    public override void OnPlayerLeft(VRCPlayerApi player)
    {
        if (Networking.IsMaster)
        {
            for (int i = 0; i < playerDBMax; i++)
            {
                if (player.IsOwner(playerDBPool.Pool[i]))
                {
                    playerDBPool.Return(playerDBPool.Pool[i]);
                    break;
                }
            }
        }
    }*/
}
