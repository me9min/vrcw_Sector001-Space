
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

    private Transform target;
    //private int updateRate = 0;

    private void Start()
    {
        target = playerDBMain.playerDB[followPlayerSeq].transform;
    }

    public override void Interact()
    {
        if (playerDBMain.isPlayerSetted)
        {
            Networking.SetOwner(playerDBMain.localPlayer, this.gameObject);
            followPlayerSeq = playerDBMain.localPlayerSeq;
            isFollow = true;
            RequestSerialization();

            SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "TargetChange");
        }
    }

    private void Update()
    {
        if (playerDBMain.isPlayerSetted)
        {
            if (isFollow)
            {
                Vector3 velo = Vector3.zero;
                this.transform.position = Vector3.SmoothDamp(this.transform.position, target.position + adjustedValue, ref velo, 0.1f);
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

    /*
    private void FixedUpdate()
    {
        if (updateRate >= 10)
        {
            Vector3 velo = Vector3.zero;
            this.transform.position = Vector3.SmoothDamp(this.transform.position, target.position + adjustedValue, ref velo, 0.1f);
        }
        else
        {
            updateRate++;
        }
    }*/

    public void TargetChange()
    {
        target = playerDBMain.playerDB[followPlayerSeq].transform;
    }
}
