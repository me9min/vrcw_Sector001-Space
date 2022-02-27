
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
public class Switch : UdonSharpBehaviour
{
    [Header("스위치 설정")]
    [Tooltip("스위치 Off 일때 보일 오브젝트")] [SerializeField] private GameObject object0;
    [Tooltip("스위치 On 일때 보일 오브젝트")] [SerializeField] private GameObject object1;
    [Tooltip("체크시 스위치 모두동기화")] [SerializeField] private bool isGlobal = false;
    [Tooltip("스위치 초기값")] [SerializeField] private bool isActive = false;

    private void Start()
    {
        if (object0 == null || object1 == null)
        {
            gameObject.SetActive(false);
            Debug.LogWarning(gameObject.name + " 스위치의 On/Off 오브젝트가 할당되지않음!");
        }
        else
        {
            if (isActive)
            {
                object0.SetActive(false);
                object1.SetActive(true);
            }
            else
            {
                object0.SetActive(true);
                object1.SetActive(false);
            }
        }
    }

    public override void Interact()
    {
        if (isGlobal)
        {
            if (isActive)
            {
                //스위치 Off 글로벌
                SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "SwitchOff");
            }
            else
            {
                //스위치 On 글로벌
                SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "SwitchOn");
            }
        }
        else
        {
            if (isActive)
            {
                //스위치 Off 로컬
                SwitchOff();
            }
            else
            {
                //스위치 On 로컬
                SwitchOn();
            }
        }
    }

    public void SwitchOn()
    {
        object0.SetActive(false);
        object1.SetActive(true);
        isActive = true;
    }
    public void SwitchOff()
    {
        object0.SetActive(true);
        object1.SetActive(false);
        isActive = false;
    }
}