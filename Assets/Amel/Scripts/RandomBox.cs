
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
public class RandomBox : UdonSharpBehaviour
{
    public MeshRenderer target; //마테리얼 바꿀 타겟
    public Material[] materials; //마테리얼 배열

    private int latestId = 0; //최근결과값 저장

    public override void Interact()
    {
        int randomId; //이번결과값 저장

        //랜덤값 (0 부터 materials의길이-1 중간값) 생성
        randomId = Random.Range(0, materials.Length);

        //최근결과값과 이번 랜덤결과값 비교 해서 같으면 한무반복
        while (latestId == randomId)
        {
            randomId = Random.Range(0, materials.Length);
        }

        //이번결과값을 최근결과값 변수에 저장
        latestId = randomId;

        //디버깅 (결과)
        Debug.Log(randomId+1 + "/" + materials.Length);
        //마테리얼 바꾸기
        target.material = materials[randomId];
    }
}
