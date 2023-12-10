using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationsInteractable : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem wallImpactParticles;

    private void Start() {
    }
    public void OnBedImpact() {
        wallImpactParticles.Play();

    }
    

}
