
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.Continuous)]
public class welcomemat : UdonSharpBehaviour
{
    [Header("트랩 설정")]
    [Tooltip("트랩 애니메이션")]
    public Animator trapAni;
    [Tooltip("트랩 사운드")]
    public AudioSource trapSound;
    [Tooltip("정렬된 플레이어 리스트 가지고있는 스크립트")]
    public PlayerDBMain playerDBMain;

    [Header("UI메세지")]
    [Tooltip("웰컴 메세지")]
    public Text welcomeMsg;

    private string tempMsg = "";
    private VRCPlayerApi tempPlayer = null;
    private bool msgUpdateSwitch = false;
    private int msgUpdateSwitchTimer = 0;

    //어떤 플레이어가 조인 했을때 들어온 플레이어객체를 반환
    public override void OnPlayerJoined(VRCPlayerApi player)
    {
        tempPlayer = player;
        msgUpdateSwitch = true;
    }

    //콜라이더가 is trigger 체크되어 있고 플레이어가 콜라이더에 들어왔을때
    public override void OnPlayerTriggerEnter(VRCPlayerApi player)
    {
        if (playerDBMain.isPlayerSetted)
        {
            trapAni.SetTrigger("trap");
            trapSound.Play();
            player.CombatSetCurrentHitpoints(0);
            //playerDBMain.LocalPlayerKill(playerDBMain.GetPlayerSeqById(player.playerId));
            tempMsg = player.displayName + " 님이 덫밟고 사망!";
            welcomeMsg.text = tempMsg;
        }
    }

    private void FixedUpdate()
    {
        //true일 때 lazy하게 반복
        if (msgUpdateSwitch)
        {
            //10번 반복후 실행
            if (msgUpdateSwitchTimer > 9)
            {
                if (playerDBMain.isPlayerSetted)
                {
                    welcomeMsg.text = tempPlayer.displayName + " 님 환영!\n체력: " + tempPlayer.CombatGetCurrentHitpoints().ToString();
                    msgUpdateSwitch = false;
                }

                //타이머 초기화
                msgUpdateSwitchTimer = 0;
            }
            else
            {
                msgUpdateSwitchTimer++;
            }
        }
    }

    //private void OnCollisionEnter(Collision col)
    //{
    //    Debug.Log(col.gameObject.name);
    //}

    //플레이어의 애니메이션이작동해서 플레이어가 부딫히거나 rigidbody에 물리적으로 충돌했을때
    //public override void OnPlayerCollisionEnter(VRCPlayerApi player)
    //{
    //    Debug.Log(player.displayName);
    //}

    //플레이어의 파티클이 부딫혓을때
    //public override void OnPlayerParticleCollision(VRCPlayerApi player)
    //{
    //    Debug.Log(player.displayName);
    //}

    //public void isInputfieldEnd()
    //{
    //    welcomeMsg.text = inputMsg.text;
    //}

    //public override void OnPlayerParticleCollision(VRCPlayerApi player)
    //{
    // 기존3차원각의 문제점을 보완하는 오일러각(4차원) 으로 설정
    // this.transform.eulerAngles = new Vector3(0, 0, 0);
    //}
}
