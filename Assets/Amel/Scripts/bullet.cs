
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
    [Tooltip("정렬된 플레이어 리스트 가지고있는 스크립트")]
    public PlayerDBMain playerDBMain;

    [Header("UI 메세지")]
    [Tooltip("웰컴 메세지")]
    public Text welcomeMsg;

    private float tempHp = 1;
    private string tempMsg = "";

    public override void OnPlayerParticleCollision(VRCPlayerApi player)
    {
        if (playerDBMain.isPlayerSetted)
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
                    player.CombatSetCurrentHitpoints(0);
                    tempMsg = player.displayName + " 님이 벌집이 되었습니다!";
                }
                welcomeMsg.text = tempMsg;
            }
        }
    }
}
