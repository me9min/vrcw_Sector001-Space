
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

public class welcomemat : UdonSharpBehaviour
{
    [Header("트랩 설정")]
    [Tooltip("트랩 애니메이션")]
    public Animator trapAni;
    [Tooltip("트랩 사운드")]
    public AudioSource trapSound;

    [Header("UI메세지")]
    [Tooltip("웰컴 메세지")]
    public Text welcomeMsg;

    private string tempMsg;

    //어떤 플레이어가 조인 했을때 들어온 플레이어객체를 반환
    public override void OnPlayerJoined(VRCPlayerApi player)
    {
        if (player == Networking.LocalPlayer) //조인한 플레이어 와 지금나자신을 비교
        {
            welcomeMsg.text = player.displayName + " 님 환영!\n체력: " + player.CombatGetCurrentHitpoints();
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

    //콜라이더가 is trigger 체크되어 있고 플레이어가 콜라이더에 들어왔을때
    public override void OnPlayerTriggerEnter(VRCPlayerApi player)
    {
        trapAni.SetTrigger("trap");
        trapSound.Play();
        player.CombatSetCurrentHitpoints(0);

        tempMsg = player.displayName + " 님이 덫밟고 사망!";
        SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "MsgChange");
    }

    public void MsgChange()
    {
        welcomeMsg.text = tempMsg;
    }

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
