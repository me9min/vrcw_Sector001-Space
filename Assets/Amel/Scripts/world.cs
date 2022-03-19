
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

//월드 메인 클래스
[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)] //메모: RequestSerialization(); 은 싱크모드가 수동(Manual)일때 모든 UdonSync변수 동기화요청
public class world : UdonSharpBehaviour
{
    [Header("플레이어DB시스템")]
    [Tooltip("플레이어DB메인")]
    public PlayerDBMain playerDBMain;

    [Header("UI설정")]
    [Tooltip("웰컴 메세지")]
    public Text welcomeMsg;

    private int localTimeSpentFallingAndLanding = 0; //추락후 착지시 걸린시간 0~255

    //프레임 관계없이 모두가 같은주기(0.02초)로 반복
    private void FixedUpdate()
    {
        //플레이어 설정이 세팅됬는지 여부확인, localPlayer에 null이면 IsPlayerGrounded가 안됨
        if (playerDBMain.isPlayerSetted)
        {
            //로컬플레이어(나)가 땅에 닿아있는지
            if (playerDBMain.playerList[playerDBMain.localPlayerSeq].IsPlayerGrounded())
            {
                //WASD중 한개라도 눌럿을때
                if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
                {
                    //왼쪽쉬프트 눌럿을때
                    if (Input.GetKey(KeyCode.LeftShift))
                    {
                        //위치 동기화 후 달리는소리 play 모두에게 전송
                        playerDBMain.PlayerPositionSync(playerDBMain.localPlayerSeq);
                        playerDBMain.PlayerRunSoundPlay(playerDBMain.localPlayerSeq);
                    }
                    else
                    {
                        //위치 동기화 후 걷는소리 play 모두에게 전송
                        playerDBMain.PlayerPositionSync(playerDBMain.localPlayerSeq);
                        playerDBMain.PlayerWalkSoundPlay(playerDBMain.localPlayerSeq);
                    }
                }
                //추락후 착지시 걸린시간 체크
                if (localTimeSpentFallingAndLanding >= 50)
                {
                    /*if (localTimeSpentFallingAndLanding >= 100)
                    {
                        //강한착지 소리 play 모두에게 전송
                    }
                    else
                    {
                        //착지 소리 play 모두에게 전송
                    }*/

                    //위치 동기화 후 강한착지 소리 play 모두에게 전송
                    playerDBMain.PlayerPositionSync(playerDBMain.localPlayerSeq);
                    playerDBMain.PlayerHardLandingSoundPlay(playerDBMain.localPlayerSeq);

                    //추락후 착지시 걸린시간 초기화
                    localTimeSpentFallingAndLanding = 0;
                }
            }
            else
            {
                if (localTimeSpentFallingAndLanding < 3000)
                {
                    localTimeSpentFallingAndLanding++;
                }
            }
        }
    }

    private void Update()
    {
        //Q버튼 눌럿을때
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (playerDBMain.isPlayerSetted)
            {
                //내가 마스터가 아닐경우 마스터에게 나를 텔레포트
                if (playerDBMain.localPlayerSeq == 0)
                {
                    //모두에게 (마스터에게 나를 텔레포트) 함수 실행
                    SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "TeleportMeToMaster");
                }
                else
                {
                    //마스터에게 나를 텔레포트
                    TeleportMeToMaster();
                }
            }
        }

        //포인트 설정 키
        if (Input.GetKeyDown(KeyCode.KeypadPlus))
        {
            if (playerDBMain.isPlayerSetted)
            {
                playerDBMain.PlayerAddPoint(playerDBMain.localPlayerSeq, 1);
                playerDBMain.PlayerSyncPoint(playerDBMain.localPlayerSeq);
                SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "UpdatePointChanged");
            }
        }
        if (Input.GetKeyDown(KeyCode.KeypadMinus))
        {
            if (playerDBMain.isPlayerSetted)
            {
                playerDBMain.PlayerSubPoint(playerDBMain.localPlayerSeq, 1);
                playerDBMain.PlayerSyncPoint(playerDBMain.localPlayerSeq);
                SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "UpdatePointChanged");
            }
        }
        if (Input.GetKeyDown(KeyCode.Keypad0))
        {
            if (playerDBMain.isPlayerSetted)
            {
                playerDBMain.PlayerSetPoint(playerDBMain.localPlayerSeq, 0);
                playerDBMain.PlayerSyncPoint(playerDBMain.localPlayerSeq);
                SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "UpdatePointChanged");
            }
        }
    }

    //마스터전용 치트 관련 함수
    public void TeleportMeToMaster()
    {
        playerDBMain.playerList[playerDBMain.localPlayerSeq].TeleportTo(playerDBMain.playerList[0].GetPosition(), playerDBMain.playerList[0].GetRotation());
    }

    //포인트 UI업데이트
    public void UpdatePointChanged()
    {
        welcomeMsg.text = playerDBMain.playerList[playerDBMain.tempPlayerSeq].displayName + "님의 포인트: " + playerDBMain.PlayerGetPoint(playerDBMain.tempPlayerSeq).ToString();
    }
}