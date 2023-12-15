using System.Collections;
using UnityEngine;
using Cinemachine;
using UnityEngine.SceneManagement;
public class PlayerAnimationEvents : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem wallImpactParticles;

    [SerializeField]
    private AudioClip[] clips;

    private CinemachineVirtualCamera cinemachineCam;
    private CinemachineBasicMultiChannelPerlin perlinShake;
    private AudioSource source;

    private void Start()
    {
        GameObject vCam = GameObject.Find("Virtual Camera");
        if (vCam != null )
        {
            // fine to do it this way since if there's not a virtual camera in the scene, we won't be executing the other functions anyway
            cinemachineCam = vCam.GetComponent<CinemachineVirtualCamera>();
            perlinShake = cinemachineCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        }
        source = GetComponent<AudioSource>();
    }

    private IEnumerator ShakeCamera(float shakeIntensity = 2.5f, float frequency = 15f, float shakeDuration = 0.1f)
    {
        perlinShake.m_AmplitudeGain = shakeIntensity;
        perlinShake.m_FrequencyGain = frequency * Time.timeScale;

        yield return new WaitForSeconds(shakeDuration);

        perlinShake.m_AmplitudeGain = 0f;
        perlinShake.m_FrequencyGain = 0f;
    }

    public void OnTimeSlow()
    {
        source.clip = clips[0];
        source.Play();
    }

    public void OnWallImpact()
    {
        wallImpactParticles.Play();

        // shake camera
        StartCoroutine(ShakeCamera());

        // play thud sound
        source.clip = clips[1];
        source.Play();

        // pause game while we wait for game to end
        TimeController.Instance.Paused = true;
        ResourceController.Instance.Paused = true;
    }

    public void OnFootStep()
    {
        source.clip = clips[2];
        source.Play();
    }

    public void OnGameEnd() {
        ResourceManager.Instance.Time += 60;
        GameManager.Instance.gridPos = new Vector2Int(47, 56);
        SceneManager.LoadScene("Wu");
    }
}
