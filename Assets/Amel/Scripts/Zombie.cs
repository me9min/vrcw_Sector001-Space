
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.Continuous)]
public class Zombie : UdonSharpBehaviour
{
    public PlayerDBMain playerDBMain;
    [UdonSynced] public int followPlayerSeq = 0;
    public bool isFollow = false;
    public Animator anime;

    private Vector3 target;
    private Vector3 targetDistance;
    private int tempPlayerId = 0;
    private bool followPlayerUpdateSwitch = false;
    private int followPlayerUpdateSwitchTimer = 0;
    private int playerPositionUpdateTimer = 0;

    public override void OnPlayerLeft(VRCPlayerApi player)
    {
        isFollow = false;
        if (playerDBMain.isPlayerSetted)
        {
            if (player.playerId == playerDBMain.playerList[followPlayerSeq].playerId)
            {
                followPlayerSeq = 0;
            }
            else
            {
                tempPlayerId = playerDBMain.playerList[followPlayerSeq].playerId;
                followPlayerUpdateSwitch = true;
            }
        }
    }

    public override void OnPlayerTriggerEnter(VRCPlayerApi player)
    {
        if (playerDBMain.isPlayerSetted)
        {
            if (!isFollow)
            {
                SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "SetAnimationWalk");
                Debug.Log("[" + player.playerId + "] " + player.displayName + " 에게 좀비가따라감");
                Networking.SetOwner(player, this.gameObject);

                followPlayerSeq = playerDBMain.localPlayerSeq;
                SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "IsFollowTrue");
                RequestSerialization();
            }
        }
    }

    public override void OnPlayerTriggerExit(VRCPlayerApi player)
    {
        if (playerDBMain.isPlayerSetted)
        {
            if (player == Networking.GetOwner(this.gameObject))
            {
                SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "SetAnimationIdle");
                Debug.Log("[" + player.playerId + "] " + player.displayName + " (이)가 좀비로부터 해방");

                SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "IsFollowFalse");
                RequestSerialization();
            }
        }
    }

    private void Update()
    {
        if (isFollow)
        {
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.LookRotation(targetDistance), Time.deltaTime*4);
            this.transform.position = Vector3.MoveTowards(this.transform.position, target, 0.01f);
        }
    }

    //프레임 관계없이 모두가 같은주기(0.02초)로 반복
    private void FixedUpdate()
    {
        if (isFollow)
        {
            if (playerPositionUpdateTimer >= 50)
            {
                target = playerDBMain.playerList[followPlayerSeq].GetPosition();
                targetDistance = target - this.transform.position;
                targetDistance.y = 0;
                playerPositionUpdateTimer = 0;
            }
            else
            {
                playerPositionUpdateTimer++;
            }
        }

        if (followPlayerUpdateSwitch)
        {
            if (followPlayerUpdateSwitchTimer >= 10)
            {
                followPlayerSeq = playerDBMain.GetPlayerSeqById(tempPlayerId);
                isFollow = true;
                followPlayerUpdateSwitch = false;
                followPlayerUpdateSwitchTimer = 0;
            }
            else
            {
                followPlayerUpdateSwitchTimer++;
            }
        }
    }

    public void IsFollowTrue()
    {
        isFollow = true;
    }
    public void IsFollowFalse()
    {
        isFollow = false;
    }

    public void SetAnimationIdle()
    {
        anime.SetTrigger("idle");
    }
    public void SetAnimationWalk()
    {
        anime.SetTrigger("walk");
    }
}