using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationsInteractable : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem ImpactParticles;

    [SerializeField]
    private AudioClip typingClip;

    [SerializeField]
    private AudioClip thudClip;

    private AudioSource source;

    void Start()
    {
        ImpactParticles = GetComponentInChildren<ParticleSystem>();
        source = GetComponent<AudioSource>();
    }

    public void OnImpact() {
        AudioSource source = GetComponent<AudioSource>();
        ImpactParticles.Play();

        source.Stop();
        source.clip = thudClip;
        source.loop = false;
        source.Play();
    }

    public void OnJump()
    {
        source.Stop();
        source.clip = typingClip;
        source.loop = true;
        source.Play();
    }

    public void newImpact() {
        ParticleSystem createdImpacts = Instantiate(ImpactParticles);
        createdImpacts.transform.position = new Vector3(ImpactParticles.transform.position.x, .2f, ImpactParticles.transform.position.z);
        createdImpacts.transform.localScale = new Vector3(.6f, 1, .6f);
        createdImpacts.transform.eulerAngles = new Vector3(90, 0, 0);
        createdImpacts.Play();
        StartCoroutine(destoryImpact(createdImpacts, .2f));
    }
    IEnumerator destoryImpact(ParticleSystem particles, float delay) {
        yield return new WaitForSeconds(delay);
        Object.Destroy(particles.gameObject);
        yield return null;
    }

    public void OnImpactStop() {
        ImpactParticles.Stop();
    }
}
