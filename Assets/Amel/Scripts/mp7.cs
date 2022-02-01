
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.Continuous)]
public class mp7 : UdonSharpBehaviour
{
    public ParticleSystem bulletParticle;
    public AudioSource fireSound;

    public void ShotStart()
    {
        bulletParticle.Play();
        fireSound.Play();
    }

    public void ShotStop()
    {
        bulletParticle.Stop();
        fireSound.Stop();
    }

    public override void OnPickupUseDown()
    {
        SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "ShotStart");
    }

    public override void OnPickupUseUp()
    {
        SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "ShotStop");
    }
}
