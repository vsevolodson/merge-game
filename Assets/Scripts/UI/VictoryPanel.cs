using UnityEngine;
using UnityEngine.UI;

public class VictoryPanel : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private Button nextButton;
    [SerializeField] private LevelManager levelManager;

    private void OnEnable()
    {
        levelManager.LevelCompleted += Show;
    }

    private void OnDisable()
    {
        levelManager.LevelCompleted -= Show;
    }

    private void Show()
    {
        panel.SetActive(true);

        nextButton.gameObject.SetActive(levelManager.HasNextLevel);
    }

    public void Hide()
    {
        panel.SetActive(false);
    }
}