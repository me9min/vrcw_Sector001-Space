
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

//월드 메인 클래스
[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)] //메모: RequestSerialization(); 은 싱크모드가 수동(Manual)일때 모든 UdonSync변수 동기화요청
public class world : UdonSharpBehaviour
{
    [Header("플레이어DB시스템")]
    [Tooltip("플레이어DB메인")]
    public PlayerDBMain playerDBMain;

    private int localTimeSpentFallingAndLanding = 0; //추락후 착지시 걸린시간 0~255

    //프레임 관계없이 모두가 같은주기로 반복
    private void FixedUpdate()
    {
        //플레이어 설정이 세팅됬는지 여부확인, localPlayer에 null이면 IsPlayerGrounded가 안됨
        if (playerDBMain.isPlayerSetted)
        {
            //로컬플레이어(나)가 땅에 닿아있는지
            if (playerDBMain.localPlayer.IsPlayerGrounded())
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
                    playerDBMain.PlayerPositionSync(playerDBMain.localPlayerSeq);
                    playerDBMain.PlayerHardLandingSoundPlay(playerDBMain.localPlayerSeq);

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