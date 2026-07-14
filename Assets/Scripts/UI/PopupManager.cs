using System.Collections.Generic;
using UnityEngine;

public class PopupManager : MonoBehaviour
{
    private readonly Stack<BasePopup> popupStack = new();

    public void Show(BasePopup popup)
    {
        if (popupStack.Count > 0)
        {
            if (popupStack.Peek() == popup)
            {
                return;
            }
            
            popupStack.Peek().Hide();
        }

        popup.Show();

        popupStack.Push(popup);
    }

    public void Hide()
    {
        if (popupStack.Count == 0)
        {
            return;
        }

        BasePopup popup = popupStack.Pop();

        popup.Hide();

        if (popupStack.Count > 0)
        {
            popupStack.Peek().Show();
        }
    }
}