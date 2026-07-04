using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private LevelConfig levelConfig;

    private int currentCount;

    public void OnItemCreated(ItemData item)
    {
        if (item != levelConfig.TargetItem)
        {
            return;
        }

        currentCount++;

        Debug.Log($"{currentCount}/{levelConfig.TargetCount}");

        if (currentCount >= levelConfig.TargetCount)
        {
            CompleteLevel();
        }
    }

    private void CompleteLevel()
    {
        Debug.Log("congrtltns");
    }
}