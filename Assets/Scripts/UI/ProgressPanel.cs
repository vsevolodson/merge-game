using TMPro;
using UnityEngine;

public class ProgressPanel : MonoBehaviour
{
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private TMP_Text goalText;
    [SerializeField] private TMP_Text progressText;

    private void OnEnable()
    {
        levelManager.LevelLoaded += SetGoal;
        levelManager.ProgressChanged += UpdateProgress;
    }

    private void OnDisable()
    {
        levelManager.LevelLoaded -= SetGoal;
        levelManager.ProgressChanged -= UpdateProgress;
    }

    public void SetGoal(ItemData item, int targetCount)
    {
        if (item == null)
        {
            return;
        }
        goalText.text = $"Get {targetCount} items lvl {item.Level}";
    }

    public void UpdateProgress(int current, int target)
    {
        progressText.text = $"{current} / {target}";
    }
}