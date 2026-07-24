using UnityEngine;
using UnityEngine.UI;

public class VictoryPanelController : MonoBehaviour
{
    [SerializeField] private BasePopup popup;
    [SerializeField] private Button nextButton;
    [SerializeField] private LevelManager levelManager;
    [SerializeField] protected PopupManager popupManager;

    private void OnEnable()
    {
        levelManager.LevelCompleted += OpenPopup;
    }

    private void OnDisable()
    {
        levelManager.LevelCompleted -= OpenPopup;
    }

    private void OpenPopup()
    {
        popupManager.Show(popup);

        nextButton.gameObject.SetActive(levelManager.HasNextLevel);
    }

    public void Hide()
    {
        popupManager.Hide();
    }
}