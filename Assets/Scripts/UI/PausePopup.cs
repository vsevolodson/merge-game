using UnityEngine;

public class PausePopup : BasePopup
{
    [SerializeField] protected PopupManager popupManager;
    
    public void Continue()
    {
        popupManager.Hide();
    }

    public void Pause()
    {
        popupManager.Show(this);
    }
}