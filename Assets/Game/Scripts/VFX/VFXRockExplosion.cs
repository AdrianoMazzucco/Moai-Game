using UnityEngine;

public class VFXRockExplosion : MonoBehaviour
{
    public void OnParticleSystemStopped()
    {
        GameManager.Instance.DestructionFXPool.Release(this.gameObject);
    }
}
