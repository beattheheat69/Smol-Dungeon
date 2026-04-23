using UnityEngine;
using UnityEngine.UI;

public class ToggleManager : MonoBehaviour
{
    public CanvasGroup entityMenuCanvas;

    public void ToggleOnMenu(Toggle toggle)
    {
        if (toggle.isOn)
        {
            entityMenuCanvas.alpha = 1;
            entityMenuCanvas.interactable = true;
        }
        else
        {
			entityMenuCanvas.alpha = 0.2f;
			entityMenuCanvas.interactable = false;
		}
    }
}
