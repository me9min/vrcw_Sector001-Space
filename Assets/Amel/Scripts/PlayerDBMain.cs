
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

//이 클래스의 기능은 명령함수(플레이어ID)를 받아 명령을 PlayerDB마다 라우팅 해줍니다
[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
public class PlayerDBMain : UdonSharpBehaviour
{
    [Header("플레이어DB할당")]
    public PlayerDB[] playerDB;

    [Header("UI설정")]
    [Tooltip("플레이어 리스트 보여줄 UI텍스트")]
    public Text playerListMsg;
    [Tooltip("에러메세지(닉네임)")]
    public string errorMsg = "<color=#de1616>플레이어 찾을 수 없음</color>";

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

    [HideInInspector] public VRCPlayerApi[] playerList = new VRCPlayerApi[20]; //플레이어 리스트 로컬저장
    [HideInInspector] public int playerCount = 0; //플레이어수 로컬저장
    [HideInInspector] public int localPlayerSeq = 0; //로컬플레이어(나)의 순서
    [HideInInspector] public bool isPlayerSetted = false; //로컬플레이어 세팅후에 true로바뀜
    [HideInInspector] public int tempPlayerSeq = 0; //플레이어 순서 임시저장
    private bool playerListUpdateSwitch = false;
    private int playerListUpdateSwitchTimer = 0;

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
            else if (i == playerCount - 1)
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

    //플레이어 순서로 플레이어 얻기
    public VRCPlayerApi GetPlayerBySeq(int playerSeq)
    {
        return playerList[playerSeq];
    }
    //플레이어 아이디로 플레이어 얻기
    public VRCPlayerApi GetPlayerById(int playerId)
    {
        VRCPlayerApi player = null;
        for (int i = 0; i < playerCount; i++)
        {
            if (playerList[i].playerId == playerId)
            {
                player = playerList[i];
                break;
            }
        }
        return player;
    }
    //플레이어 아이디로 플레이어순서 얻기
    public int GetPlayerSeqById(int playerId)
    {
        int playerSeq = 0;
        for (int i = 0; i < playerCount; i++)
        {
            if (playerList[i].playerId == playerId)
            {
                playerSeq = i;
                break;
            }
        }
        return playerSeq;
    }

    //플레이어 물리 세팅 모아둔 함수
    public void PhysicsSetup(int playerSeq)
    {
        playerList[playerSeq].SetWalkSpeed(walkSpeed); //앞 속도
        playerList[playerSeq].SetStrafeSpeed(walkSpeed); //좌우(옆) 속도
        playerList[playerSeq].SetRunSpeed(runSpeed); //달리기 속도
        playerList[playerSeq].SetJumpImpulse(jumpPower); //점프 힘
        playerList[playerSeq].SetGravityStrength(gravityPower); //중력
    }

    //플레이어 컴뱃시스템 세팅 모아둔 함수
    public void CombatSystemSetup(int playerSeq)
    {
        playerList[playerSeq].CombatSetup(); //컴뱃 시스템 셋팅시작? 월드에 플레이어를따라다니는 컴뱃시스템 오브젝트를 삽입?
        playerList[playerSeq].CombatSetCurrentHitpoints(HP); //현재체력, 0이나 -1이될시 사망
        playerList[playerSeq].CombatSetDamageGraphic(null); //데미지입엇을때 혹은 죽었을때 효과
        playerList[playerSeq].CombatSetMaxHitpoints(HP); //최대체력
        playerList[playerSeq].CombatSetRespawn(true, respawnTime, respawnLocation); //리스폰 할지 여부 true or false , 리스폰시간float값(초) , 리스폰할 위치 transform형식 외부에서받아오는걸로많이씀
        playerList[playerSeq].CombatSetup(); //이걸로 마무리 이유는모르겠음
    }
    
    //어떤 플레이어가 접속했을때 들어온 플레이어객체를 반환
    public override void OnPlayerJoined(VRCPlayerApi player)
    {
        //로컬변수에 인원리스트,인원수 세팅
        UpdateSortPlayerList();
        //플레이어리스트 UI 업데이트
        playerListMsg.text = MakePlayerListMsg();

        //플레이어 설정이 되어있을때
        if (isPlayerSetted)
        {
            //들어온사람 컴뱃시스템 세팅
            CombatSystemSetup(GetPlayerSeqById(player.playerId));
            //들어온사람 컴뱃시스템 세팅
            Networking.SetOwner(player, playerDB[GetPlayerSeqById(player.playerId)].gameObject);
        }
        else //플레이어 설정이 되어있지않을때(최초접속시)
        {
            if (player.isLocal) //조인한 플레이어 가 로컬(나)인지
            {
                //플레이어 물리 세팅
                PhysicsSetup(localPlayerSeq);
                //모든플레이어 컴뱃시스템 세팅
                for (int i = 0; i < playerCount; i++)
                {
                    CombatSystemSetup(i);
                }
                //모든플레이어 오브젝트 오너 업데이트
                for (int i = 0; i < playerCount; i++)
                {
                    Networking.SetOwner(playerList[i], playerDB[i].gameObject);
                }

                //이코드는 최초로 한번실행하면되므로 false에서 true로 변경
                isPlayerSetted = true;
            }
        }
    }

    //어떤 플레이어가 나갔을때 들어온 플레이어객체를 반환
    public override void OnPlayerLeft(VRCPlayerApi player)
    {
        //lazy 플레이어 리스트 갱신 트리거 ON
        playerListUpdateSwitch = true;
        playerListUpdateSwitchTimer = 0;
    }

    //프레임 관계없이 모두가 같은주기(0.02초)로 반복
    private void FixedUpdate()
    {
        //true일 때 lazy하게 플레이어 리스트 갱신
        if (playerListUpdateSwitch)
        {
            //10번 반복후 실행
            if (playerListUpdateSwitchTimer >= 10)
            {
                //로컬변수에 인원리스트,인원수 세팅
                UpdateSortPlayerList();
                //모든플레이어 오브젝트 오너 업데이트
                for (int i = 0; i < playerCount; i++)
                {
                    Networking.SetOwner(playerList[i], playerDB[i].gameObject);
                }
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
    }



    //플레이어리스트 UI 텍스트 만들기
    public string MakePlayerListMsg()
    {
        //임시 메세지 선언, 빈칸삽입
        string tempMsg = "";

        //리스트메세지 만들기(플레이어수 만큼 반복)
        for (byte i = 0; i < playerCount; i++)
        {
            tempMsg += i.ToString() + ".[" + playerList[i].playerId.ToString() + "] <color=#ffff32>" + playerList[i].displayName + "</color>";

            if (playerList[i].isMaster)
            {
                tempMsg += " <color=#de1616>(Master)</color>";
            }

            if (i == localPlayerSeq)
            {
                tempMsg += " (나)\n";
            }
            else
            {
                tempMsg += "\n";
            }
        }
        return tempMsg;
    }

    //플레이어 컴뱃시스템 관련 함수 (로컬)
    public float CombatSystemGetHP(int playerDBSeq)
    {
        return playerList[playerDBSeq].CombatGetCurrentHitpoints();
    }
    public void LocalPlayerKill(int playerDBSeq)
    {
        playerList[playerDBSeq].CombatSetCurrentHitpoints(0);
    }

    //플레이어 위치 정보 동기화 함수 라우팅
    public void PlayerPositionSync(int playerDBSeq)
    {
        playerDB[playerDBSeq].PositionSyncGlobal();
    }
    //플레이어 회전 정보 동기화 함수 라우팅
    public void PlayerRotationSync(int playerDBSeq)
    {
        playerDB[playerDBSeq].RotationSyncGlobal();
    }

    //플레이어 발소리 재생 관련 함수 라우팅
    public void PlayerWalkSoundPlay(int playerDBSeq)
    {
        playerDB[playerDBSeq].WalkSoundPlayGlobal();
    }
    public void PlayerRunSoundPlay(int playerDBSeq)
    {
        playerDB[playerDBSeq].RunSoundPlayGlobal();
    }
    public void PlayerJumpSoundPlay(int playerDBSeq)
    {
        playerDB[playerDBSeq].JumpSoundPlayGlobal();
    }
    public void PlayerLandingSoundPlay(int playerDBSeq)
    {
        playerDB[playerDBSeq].LandingSoundPlayGlobal();
    }

    //플레이어 포인트 관련 함수 라우팅
    public void PlayerSyncPoint(int playerDBSeq)
    {
        playerDB[playerDBSeq].SyncPoint();
    }
    public int PlayerGetPoint(int playerDBSeq)
    {
        return playerDB[playerDBSeq].GetPoint();
    }
    public void PlayerSetPoint(int playerDBSeq, int inputPoint)
    {
        playerDB[playerDBSeq].SetPoint(inputPoint);
    }
    public void PlayerAddPoint(int playerDBSeq, int inputPoint)
    {
        playerDB[playerDBSeq].AddPoint(inputPoint);
    }
    public void PlayerSubPoint(int playerDBSeq, int inputPoint)
    {
        playerDB[playerDBSeq].SubPoint(inputPoint);
    }
}