
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using AmelCustomScripts;
using UnityEngine.UI;
using System.Collections.Generic;

public class world : UdonSharpBehaviour
{
    [Header("플레이어 스탯 설정")]
    [Tooltip("걷는 속도")]
    public float walkSpeed = 3.0f;
    [Tooltip("달리기 속도")]
    public float runSpeed = 5.0f;
    [Tooltip("점프 힘")]
    public float jumpPower = 5.0f;
    [Tooltip("중력")]
    public float gravityPower = 1.5f;

    [Header("플레이어 컴뱃시스템 설정")]
    [Tooltip("사망시 리스폰지점")]
    public Transform respawnLocation;
    [Tooltip("사망시 리스폰시간")]
    public float respawnTime = 5.0f;
    [Tooltip("최대체력 및 초기체력")]
    public float HP = 2.0f;

    [Header("플레이어DB시스템")]
    [Tooltip("플레이어DB메인")]
    public PlayerDBMain playerDBMain;

    [Header("UI 업데이트")]
    [Tooltip("플레이어 리스트 보여줄 UI텍스트")]
    public Text playerListMsg;
    [Tooltip("플레이어 정렬된 리스트 보여줄 UI텍스트")]
    public Text playerListSortMsg;
    [Tooltip("에러메세지(닉네임)")]
    public string errorMsg = "<color=#de1616>플레이어 찾을 수 없음</color>";

    private int playerCount = 0; //플레이어 수 저장
    //private Dictionary<int, string> playerList = new Dictionary<int, string>();
    private VRCPlayerApi[] playerListTemp = new VRCPlayerApi[20]; //플레이어 리스트 저장 넉넉하게20개할당
    private string playerListMsgTemp = ""; //플레이어리스트 메세지 임시저장

    private VRCPlayerApi localPlayer = null; //로컬플레이어(나)
    private byte localTimeSpentFallingAndLanding = 0; //추락후 착지시 걸린시간 0~255
    private bool isPlayerSetted = false; //로컬플레이어 세팅후에 true로바뀜

    //컴뱃시스템 세팅 모아둔 함수
    public void CombatSystemSetup(VRCPlayerApi player)
    {
        //들어온 플레이어 컴뱃 시스템 세팅
        player.CombatSetup(); //컴뱃 시스템 셋팅시작? 월드에 플레이어를따라다니는 컴뱃시스템 오브젝트를 삽입?
        player.CombatSetCurrentHitpoints(HP); //현재체력, 0이나 -1이될시 사망
        player.CombatSetDamageGraphic(null); //데미지입엇을때 혹은 죽었을때 효과
        player.CombatSetMaxHitpoints(HP); //최대체력
        player.CombatSetRespawn(true, respawnTime, respawnLocation); //리스폰 할지 여부 true or false , 리스폰시간float값(초) , 리스폰할 위치 transform형식 외부에서받아오는걸로많이씀
        player.CombatSetup(); //이걸로 마무리 이유는모르겠음
    }

    //어떤 플레이어가 조인 했을때 들어온 플레이어객체를 반환
    public override void OnPlayerJoined(VRCPlayerApi player)
    {
        if (isPlayerSetted)
        {
            /*//UI 플레이어 리스트 갱신
            playerCount = VRCPlayerApi.GetPlayerCount();
            VRCPlayerApi.GetPlayers(playerList);

            //리스트 갱신
            playerListMsgTemp = "[0] " + playerList[0].displayName;
            for (byte i = 1; i < playerCount; i++)
            {
                playerListMsgTemp += "\n[" + i.ToString() + "] " + playerList[i].displayName;
            }
            playerListMsg.text = playerListMsgTemp;

            // playerList playerId순으로 배열을 정렬시킨다

            //리스트 정렬 갱신
            playerListMsgTemp = "[0] " + playerList[0].displayName;
            for (byte i = 1; i < playerCount; i++)
            {
                playerListMsgTemp += "\n[" + i.ToString() + "] " + playerList[i].displayName;
            }
            playerListSortMsg.text = playerListMsgTemp;*/
        }
        else
        {
            if (player == Networking.LocalPlayer) //조인한 플레이어 와 지금나자신을 비교
            {
                localPlayer = player;
                player.SetWalkSpeed(walkSpeed);
                player.SetStrafeSpeed(walkSpeed);
                player.SetRunSpeed(runSpeed);
                player.SetJumpImpulse(jumpPower);
                player.SetGravityStrength(gravityPower);
                CombatSystemSetup(player);
            }

            /*//임시로 playerListTemp변수에 모든플레이어정보저장
            VRCPlayerApi.GetPlayers(playerListTemp);
            //플레이어 인원수 저장
            playerCount = VRCPlayerApi.GetPlayerCount();
            //playerList변수에 playerListTemp를 처리해 정제된 값 저장
            for (byte i = 0; i < playerCount; i++)
            {
                playerList.Add(playerListTemp[i].playerId, playerListTemp[i].displayName);
            }
            //플레이어리스트 UI 갱신
            playerListMsgTemp = "[0] " + playerList[0];
            for (byte i = 1; i < playerCount; i++)
            {
                playerListMsgTemp += "\n[" + i.ToString() + "] " + playerList.TryGetValue(i, out errorMsg);
            }
            playerListSortMsg.text = playerListMsgTemp

            //이코드는 최초로 한번실행하면되므로 false에서 true로 변경
            isPlayerSetted = true;*/
        }
    }

    public override void OnPlayerLeft(VRCPlayerApi player)
    {
        /*//UI 플레이어 리스트 갱신
        playerCount = VRCPlayerApi.GetPlayerCount();
        VRCPlayerApi.GetPlayers(playerList);

        //리스트 갱신
        playerListMsgTemp = "[0] " + playerList[0].displayName;
        for (byte i = 1; i < playerCount; i++)
        {
            playerListMsgTemp += "\n[" + i.ToString() + "] " + playerList[i].displayName;
        }
        playerListMsg.text = playerListMsgTemp;

        // playerList playerId순으로 배열을 정렬시킨다

        //리스트 정렬 갱신
        playerListMsgTemp = "[0] " + playerList[0].displayName;
        for (byte i = 1; i < playerCount; i++)
        {
            playerListMsgTemp += "\n[" + i.ToString() + "] " + playerList[i].displayName;
        }
        playerListSortMsg.text = playerListMsgTemp;*/
    }

    //프레임 관계없이 모두가 같은주기로 반복
    private void FixedUpdate()
    {
        //플레이어 설정이 세팅됬는지 여부확인, localPlayer에 null이면 IsPlayerGrounded가 안됨
        if (isPlayerSetted)
        {
            //로컬플레이어(나)가 땅에 닿아있는지
            if (localPlayer.IsPlayerGrounded())
            {
                //WASD눌럿을때
                if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
                {
                    //왼쪽쉬프트눌럿을때
                    if (Input.GetKey(KeyCode.LeftShift))
                    {
                        //달리는소리 play 모두에게 전송
                        //playerDBMain.PlayerWalkSoundPlay();
                    }
                    else
                    {
                        //걷는소리 play 모두에게 전송
                    }
                }
                //추락후 착지시 걸린시간 체크
                if (localTimeSpentFallingAndLanding >= 40)
                {
                    //if (localTimeSpentFallingAndLanding >= 80)
                    //{
                    //    //강한착지 소리 play 모두에게 전송
                    //    SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "HardLandingSoundPlay");
                    //}
                    //else
                    //{
                    //    //착지 소리 play 모두에게 전송
                    //    SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "LandingSoundPlay");
                    //}

                    //SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "HardLandingSoundPlay");

                    //초기화
                    localTimeSpentFallingAndLanding = 0;
                }
            }
            else
            {
                localTimeSpentFallingAndLanding++;
            }
        }
    }
}