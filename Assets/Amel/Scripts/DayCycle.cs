
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
public class DayCycle : UdonSharpBehaviour
{
    [Header("설정")]
    [Tooltip("Directional Light 필요")]
    public Transform sun;
    [Tooltip("Skydome(모델메쉬) 필요")]
    public Transform skydome;
    [Tooltip("시간 속도 : 실제1초 =1분(0.25), =1시간(15)")]
    public float timeSpeed = 0.25f;
    [HideInInspector]
    public float hour = 0; // 360 15=1시간/실제1분 0.25=1분/실제1초

    // 프레임마다 실행
    private void Update()
    {
        Debug.Log(hour);

        // 0.25/360각도 = day시스템1분 = 실제1초
        // 시간값만큼 sun로테이션 바꿈, eulerAngles = 기존3차원각의 문제점을 보완하는 오일러각(4차원)
        sun.transform.eulerAngles = new Vector3(hour, 30, 15);
        skydome.transform.eulerAngles = new Vector3(hour, 30, 15);

        //RenderSettings.skybox.SetFloat("_Rotation", hour);

        // deltaTime : 1fps = 1, 50fps = 0.02, 100fps = 0.01
        hour += Time.deltaTime*timeSpeed; // hour=1분/실제1초 == hour=0.25

        // 24시 이상일시 24시간 나눠서 나머지를 적용  예) hour가24.4면 0.4로, 49면 1로
        if (hour >= 360f)
        {
            hour = hour%360f;
        }
    }
}
