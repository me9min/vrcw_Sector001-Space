
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
public class bullet : UdonSharpBehaviour
{
    [Header("총알 설정")]
    [Tooltip("데미지")]
    public float damage = 1.0f;
    [Tooltip("정렬된 플레이어 리스트 가지고있는 스크립트")]
    public PlayerDBMain playerDBMain;

    [Header("UI설정")]
    [Tooltip("웰컴 메세지")]
    public Text welcomeMsg;

    private float tempHp = 1;
    [UdonSynced] private int tempPlayerSeq = 0;

    public override void OnPlayerParticleCollision(VRCPlayerApi player)
    {
        if (playerDBMain.isPlayerSetted)
        {
            Networking.SetOwner(player, this.gameObject);
            tempPlayerSeq = playerDBMain.GetPlayerSeqById(player.playerId);
            RequestSerialization();

            tempHp = player.CombatGetCurrentHitpoints();
            if (tempHp > 0)
            {
                tempHp -= damage;
                if (tempHp > 0)
                {
                    player.CombatSetCurrentHitpoints(tempHp);
                }
                else
                {
                    player.CombatSetCurrentHitpoints(0);
                }
                SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "UpdateWelcomeMsg");
            }
        }
    }

    public void UpdateWelcomeMsg()
    {
        string tempMsg;
        float tempHp = playerDBMain.playerList[tempPlayerSeq].CombatGetCurrentHitpoints();
        if (tempHp > 0)
        {
            tempMsg = "총맞은사람: " + playerDBMain.playerList[tempPlayerSeq].displayName + " 남은체력: " + tempHp.ToString();
        }
        else
        {
            tempMsg = playerDBMain.playerList[tempPlayerSeq].displayName + " 님이 벌집이 되었습니다!";
        }
        welcomeMsg.text = tempMsg;
    }
}
