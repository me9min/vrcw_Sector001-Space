
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using UnityEngine.UI;

//월드 메인 클래스
[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)] //메모: RequestSerialization(); 은 싱크모드가 수동(Manual)일때 모든 UdonSync변수 동기화요청
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
    [Tooltip("에러메세지(닉네임)")]
    public string errorMsg = "<color=#de1616>플레이어 찾을 수 없음</color>";

    private VRCPlayerApi[] playerList = new VRCPlayerApi[20]; //플레이어 리스트 로컬저장
    [HideInInspector] public int playerCount = 0; //플레이어수 로컬저장
    [HideInInspector] public VRCPlayerApi localPlayer = null; //로컬플레이어(나)
    [HideInInspector] public int localPlayerSeq = 0; //로컬플레이어(나)의 순서
    [HideInInspector] public bool isPlayerSetted = false; //로컬플레이어 세팅후에 true로바뀜
    [HideInInspector] public int localTimeSpentFallingAndLanding = 0; //추락후 착지시 걸린시간 0~255

    private bool playerListUpdateSwitch = false;
    private int playerListUpdateSwitchTimer = 0;

    public int GetPlayerIdBySeq(int playerSeq)
    {
        return playerList[playerSeq].playerId;
    }

    //플레이어 물리 세팅 모아둔 함수
    public void PlayerPhysicsSetup(VRCPlayerApi player)
    {
        player.SetWalkSpeed(walkSpeed);
        player.SetStrafeSpeed(walkSpeed);
        player.SetRunSpeed(runSpeed);
        player.SetJumpImpulse(jumpPower);
        player.SetGravityStrength(gravityPower);
    }

    //컴뱃시스템 세팅 모아둔 함수
    public void PlayerCombatSystemSetup(VRCPlayerApi player)
    {
        //들어온 플레이어 컴뱃 시스템 세팅
        player.CombatSetup(); //컴뱃 시스템 셋팅시작? 월드에 플레이어를따라다니는 컴뱃시스템 오브젝트를 삽입?
        player.CombatSetCurrentHitpoints(HP); //현재체력, 0이나 -1이될시 사망
        player.CombatSetDamageGraphic(null); //데미지입엇을때 혹은 죽었을때 효과
        player.CombatSetMaxHitpoints(HP); //최대체력
        player.CombatSetRespawn(true, respawnTime, respawnLocation); //리스폰 할지 여부 true or false , 리스폰시간float값(초) , 리스폰할 위치 transform형식 외부에서받아오는걸로많이씀
        player.CombatSetup(); //이걸로 마무리 이유는모르겠음
    }
    
    //플레이어 리스트,인원수 갱신,정렬 후 변수에저장
    public void UpdateSortPlayerList()
    {
        VRCPlayerApi tempPlayer; //정렬할때 임시저장소로 사용

        playerCount = VRCPlayerApi.GetPlayerCount(); //VRChat에서 인원수를 가져와 playerCount에 삽입
        VRCPlayerApi.GetPlayers(playerList); //VRChat에서 플레이어리스트를 가져와 playerList에 삽입
        
        //플레이어 목록 정렬 알고리즘
        for (int i = 1; i < playerCount; i++)
        {
            //i번째 플레이어ID가 나의ID보다 높을때, 마지막은 break로 for문탈출
            if (playerList[i].playerId > playerList[0].playerId)
            {
                tempPlayer = playerList[0];
                for (int j = 1; j < i; j++)
                {
                    playerList[j - 1] = playerList[j];
                }
                playerList[i - 1] = tempPlayer;
                localPlayerSeq = i - 1;
                break;
            }
            else if (i == playerCount-1)
            {
                tempPlayer = playerList[0];
                for (int j = 1; j <= i; j++)
                {
                    playerList[j - 1] = playerList[j];
                }
                playerList[i] = tempPlayer;
                localPlayerSeq = i;
                break;
            }
        }
    }

    //플레이어리스트 UI 텍스트 만들기
    public string MakePlayerListMsg()
    {
        string tempMsg = ""; //임시 메세지 선언, 빈칸삽입
        //리스트메세지 만들기(플레이어수 만큼 반복)
        for (byte i = 0; i < playerCount; i++)
        {
            tempMsg += i.ToString() + ".[" + playerList[i].playerId.ToString() + "] " + playerList[i].displayName + "\n";
        }
        return tempMsg;
    }

    //어떤 플레이어가 조인 했을때 들어온 플레이어객체를 반환
    public override void OnPlayerJoined(VRCPlayerApi player)
    {
        //플레이어 설정이 되어있지 않았을때
        if (!isPlayerSetted)
        {
            if (player == Networking.LocalPlayer) //조인한 플레이어 와 지금나자신을 비교
            {
                //변수에 나자신의 정보 삽입 VRCPlayerApi형식(전체정보)
                localPlayer = player;

                //플레이어 물리,컴뱃시스템 세팅
                PlayerPhysicsSetup(player);
                PlayerCombatSystemSetup(player);

                //이코드는 최초로 한번실행하면되므로 false에서 true로 변경
                isPlayerSetted = true;
            }
        }

        //플레이어 리스트 갱신 트리거 ON
        playerListUpdateSwitch = true;
    }

    //어떤 플레이어가 나갔을때 들어온 플레이어객체를 반환
    public override void OnPlayerLeft(VRCPlayerApi player)
    {
        //플레이어 리스트 갱신 트리거 ON
        playerListUpdateSwitch = true;
    }

    //프레임 관계없이 모두가 같은주기로 반복
    private void FixedUpdate()
    {
        //true일 때 lazy하게 플레이어 리스트 갱신
        if (playerListUpdateSwitch)
        {
            //10번 반복후 실행
            if (playerListUpdateSwitchTimer > 9)
            {
                //로컬변수에 인원리스트,인원수 세팅
                UpdateSortPlayerList();
                //오브젝트 오너 업데이트
                //playerDBMain.UpdatePlayerDBOwn(playerList, playerCount);
                //플레이어목록UI 갱신
                playerListMsg.text = MakePlayerListMsg();

                //스위치 초기화
                playerListUpdateSwitchTimer = 0;
                playerListUpdateSwitch = false;
            }
            else
            {
                playerListUpdateSwitchTimer++;
            }
        }

        //플레이어 설정이 세팅됬는지 여부확인, localPlayer에 null이면 IsPlayerGrounded가 안됨
        if (isPlayerSetted)
        {
            //로컬플레이어(나)가 땅에 닿아있는지
            if (localPlayer.IsPlayerGrounded())
            {
                //WASD중 한개라도 눌럿을때
                if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
                {
                    //왼쪽쉬프트 눌럿을때
                    if (Input.GetKey(KeyCode.LeftShift))
                    {
                        //위치 동기화 후 달리는소리 play 모두에게 전송
                        playerDBMain.PlayerPositionSync(localPlayerSeq);
                        playerDBMain.PlayerWalkSoundPlay(localPlayerSeq);
                    }
                    else
                    {
                        //위치 동기화 후 걷는소리 play 모두에게 전송
                        playerDBMain.PlayerPositionSync(localPlayerSeq);
                        playerDBMain.PlayerWalkSoundPlay(localPlayerSeq);
                    }
                }
                //추락후 착지시 걸린시간 체크
                if (localTimeSpentFallingAndLanding >= 40)
                {
                    /*if (localTimeSpentFallingAndLanding >= 80)
                    {
                        //강한착지 소리 play 모두에게 전송
                    }
                    else
                    {
                        //착지 소리 play 모두에게 전송
                    }*/

                    //위치 동기화 후 강한착지 소리 play 모두에게 전송
                    playerDBMain.PlayerPositionSync(localPlayerSeq);
                    playerDBMain.PlayerHardLandingSoundPlay(localPlayerSeq);

                    //추락후 착지시 걸린시간 초기화
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