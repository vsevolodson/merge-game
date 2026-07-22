using UnityEngine;

public class DestroyAfterParticles : MonoBehaviour
{
    [SerializeField] private ParticleSystem particles;

    private void Awake()
    {
        if (particles == null)
        {
            particles = GetComponent<ParticleSystem>();
        }
    }

    private void OnEnable()
    {
        Destroy(gameObject, particles.main.duration + particles.main.startLifetime.constant);
    }
}