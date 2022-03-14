
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
public class PlayerFollowDrone : UdonSharpBehaviour
{
    public PlayerDBMain playerDBMain;
    [UdonSynced] public int followPlayerSeq = 0;
    [UdonSynced] public bool isFollow = true;
    public Vector3 adjustedValue = new Vector3(0, 0, 0);

    private int tempPlayerId = 0;
    private bool followPlayerUpdateSwitch = false;
    private int followPlayerUpdateSwitchTimer = 0;

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

    public override void Interact()
    {
        if (playerDBMain.isPlayerSetted)
        {
            Networking.SetOwner(playerDBMain.playerList[playerDBMain.localPlayerSeq], this.gameObject);

            followPlayerSeq = playerDBMain.localPlayerSeq;
            isFollow = true;
            RequestSerialization();
        }
    }

    private void Update()
    {
        if (playerDBMain.isPlayerSetted)
        {
            if (isFollow)
            {
                Vector3 velo = Vector3.zero;
                this.transform.position = Vector3.SmoothDamp(this.transform.position, playerDBMain.playerList[followPlayerSeq].GetPosition() + adjustedValue, ref velo, 0.1f);
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                if (followPlayerSeq == playerDBMain.localPlayerSeq)
                {
                    isFollow = !isFollow;
                    RequestSerialization();
                }
            }
        }
    }

    private void FixedUpdate()
    {
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
}
