using UnityEngine;

public class PlayerAnimationEffects : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem wallImpactParticles;

    public void OnWallImpact()
    {
        wallImpactParticles.Play();
    }
}
