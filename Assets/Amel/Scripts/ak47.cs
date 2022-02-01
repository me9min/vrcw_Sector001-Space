
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.Continuous)]
public class ak47 : UdonSharpBehaviour
{
    public ParticleSystem bulletParticle;
    public Animator magAnimation;
    public VRC_Pickup onwer = null;

    public void ShotStart()
    {
        bulletParticle.Play();
    }

    public void ShotStop()
    {
        bulletParticle.Stop();
    }

    public void Reload()
    {
        magAnimation.SetTrigger("isr");
    }

    public override void OnPickupUseDown()
    {
        SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "ShotStart");
    }

    public override void OnPickupUseUp()
    {
        SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "ShotStop");
    }

    private void Update()
    {
        if (onwer != null)
        {
            if (onwer.currentPlayer == Networking.LocalPlayer)
            {
                if (Input.GetKeyDown(KeyCode.F))
                {
                    SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "Reload");
                }
            }
        }
    }
}