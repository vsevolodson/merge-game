using UnityEngine;
using UnityEngine.UI;
using System;

[RequireComponent(typeof(AudioSource))]
public class MergeSoundController : MonoBehaviour
{
    [SerializeField] private AudioClip mergeClip;
    [SerializeField] private Button soundButton;

    private AudioSource audioSource;
    private bool soundEnabled = true;

    public bool SoundEnabled => soundEnabled;
    public event Action SoundEnabledSwitched;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        SetSoundButtonColor();
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

    public void SwitchSoundEnabled()
    {
        soundEnabled = !soundEnabled;

        SetSoundButtonColor();

        SoundEnabledSwitched?.Invoke();
    }

    private void SetSoundButtonColor()
    {
        ColorBlock colors = soundButton.colors;

        Color targetColor = soundEnabled ? Color.white : Color.gray;

        colors.normalColor = targetColor;
        colors.highlightedColor = targetColor;
        colors.pressedColor = targetColor;
        colors.selectedColor = targetColor;

        soundButton.colors = colors;
    }

    public void Load(SaveData saveData)
    {
        Debug.Log(saveData.soundEnabled);
        if (saveData == null)
        {
            StartNewGame();
            return;
        }
        
        if (saveData.soundEnabled != soundEnabled)
        {
            SwitchSoundEnabled();
        }
    }

    public void StartNewGame()
    {
        soundEnabled = true;
        SetSoundButtonColor();
    }
}