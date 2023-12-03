using System.Collections;
using UnityEngine;
using Cinemachine;

public class PlayerAnimationEffects : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem wallImpactParticles;

    private CinemachineVirtualCamera cinemachineCam;
    private CinemachineBasicMultiChannelPerlin perlinShake;

    private void Start()
    {
        cinemachineCam = GameObject.Find("Virtual Camera").GetComponent<CinemachineVirtualCamera>(); 
        perlinShake = cinemachineCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    private IEnumerator ShakeCamera(float shakeIntensity = 2.5f, float frequency = 15f, float shakeDuration = 0.1f)
    {
        perlinShake.m_AmplitudeGain = shakeIntensity;
        perlinShake.m_FrequencyGain = frequency * Time.timeScale;

        yield return new WaitForSeconds(shakeDuration);

        perlinShake.m_AmplitudeGain = 0f;
        perlinShake.m_FrequencyGain = 0f;
    }

    public void OnWallImpact()
    {
        wallImpactParticles.Play();

        // shake camera
        StartCoroutine(ShakeCamera());
    }
}
