using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MergeSoundController : MonoBehaviour
{
    [SerializeField] private AudioClip mergeClip;

    private AudioSource audioSource;
    private bool soundEnabled = true;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        MergeHandler.MergePerformed += PlayMergeSound;
    }

    private void OnDisable()
    {
        MergeHandler.MergePerformed -= PlayMergeSound;
    }

    private void PlayMergeSound(Vector3 position)
    {
        if (!soundEnabled)
        {
            return;
        }

        audioSource.PlayOneShot(mergeClip);
    }

    public void SetSoundEnabled(bool enabled)
    {
        soundEnabled = enabled;
    }
}