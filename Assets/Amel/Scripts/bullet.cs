
using System;
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.Continuous)]
public class bullet : UdonSharpBehaviour
{
    [Header("총알 설정")]
    [Tooltip("데미지")]
    public float damage = 1.0f;

    [Header("UI 메세지")]
    [Tooltip("웰컴 메세지")]
    public Text welcomeMsg;

    [HideInInspector]
    public float tempHp = 1;
    [HideInInspector]
    public string tempMsg;

    public override void OnPlayerParticleCollision(VRCPlayerApi player)
    {
        tempHp = player.CombatGetCurrentHitpoints();
        if (tempHp > 0)
        {
            tempHp -= damage;
            if (tempHp > 0)
            {
                player.CombatSetCurrentHitpoints(tempHp);
                tempMsg = "총맞은사람: " + player.displayName + " 남은체력: " + tempHp;
            }
            else
            {
                player.CombatSetCurrentHitpoints(-1);
                tempMsg = player.displayName + " 님이 벌집이 되었습니다!";
            }
            SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "MsgChange");
        }
    }

    public void MsgChange()
    {
        welcomeMsg.text = tempMsg;
    }
}
