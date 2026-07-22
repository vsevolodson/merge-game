using UnityEngine;

public class MergeParticlesController : MonoBehaviour
{
    [SerializeField] private ParticleSystem mergeParticlesPrefab;  
    [SerializeField] private RectTransform canvas;

    private void OnEnable()
    {
        MergeHandler.MergePerformed += PlayParticles;
    }

    private void OnDisable()
    {
        MergeHandler.MergePerformed -= PlayParticles;
    }

    private void PlayParticles(Vector3 position)
    {
        ParticleSystem particles = Instantiate(
            mergeParticlesPrefab,
            canvas);

        particles.transform.position = position;
    }
}