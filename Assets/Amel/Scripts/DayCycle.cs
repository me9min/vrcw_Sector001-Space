
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
public class DayCycle : UdonSharpBehaviour
{
    [Header("설정")]
    [Tooltip("Directional Light 필요")]
    public Transform sunray;
    [Tooltip("Skydome(모델메쉬) 필요")]
    public Transform skydome;
    [Tooltip("1초당 시간 : 실제1초 =1분(0.25), =1시간(15)")]
    public float timeSpeed = 0.25f;
    [Tooltip("0 <= 시작시간 < 360")]
    [UdonSynced] public float hour = 0; //360 15=1시간/실제1분 0.25=1분/실제1초
    [Tooltip("시간정지")]
    [UdonSynced] public bool isTimeFlow = true;

    [Header("시각표시")]
    [Tooltip("시계(Ui.Text)")]
    public Text ClockMsg;
    private string tHour;
    private string tMin;
    private string tSec;

    public override void OnPlayerJoined(VRCPlayerApi player)
    {
        if (player.isLocal)
        {
            RequestSerialization();
        }
    }

    //프레임마다 반복
    private void Update()
    {
        if (isTimeFlow)
        {
            //0.25/360각도 = day시스템1분 = 실제1초
            //시간값만큼 sun로테이션 바꿈, eulerAngles = 기존3차원각의 문제점을 보완하는 오일러각(4차원)
            sunray.transform.eulerAngles = new Vector3(hour, 30, 15);
            skydome.transform.eulerAngles = new Vector3(hour, 30, 15);

            //deltaTime : 1fps = 1, 50fps = 0.02, 100fps = 0.01
            hour += Time.deltaTime * timeSpeed; // hour=1분/실제1초 == hour=0.25

            //24시 이상일시 24시간 나눠서 나머지를 적용  예) hour가24.4면 0.4로, 49면 1로
            if (hour >= 360f)
            {
                hour = hour % 360f;
            }

            //시계표현값 계산
            tSec = Mathf.Floor((hour % 0.25f) * 240f).ToString("00");
            tMin = Mathf.Floor((hour % 15f) * 4f).ToString("00");
            tHour = ((Mathf.Floor(hour / 15f) + 6) % 24).ToString("00");

            //시계UI 업데이트
            ClockMsg.text = tHour + ":" + tMin + ":" + tSec;
        }
    }

    //시간정지 toggle
    public void IsTimeFlowToggle()
    {
        isTimeFlow = !isTimeFlow;
    }
}