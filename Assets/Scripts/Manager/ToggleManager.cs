using UnityEngine;
using UnityEngine.UI;

public class ToggleManager : MonoBehaviour
{
    public CanvasGroup entityMenuCanvas;
    public Canvas selectARoomTuto;
    public Canvas selectEntityTuto;

    public void ToggleOnMenu(Toggle toggle)
    {
        if (toggle.isOn)
        {
            entityMenuCanvas.alpha = 1;
            entityMenuCanvas.interactable = true;
            selectARoomTuto.gameObject.SetActive(false);
            selectEntityTuto.gameObject.SetActive(true);
        }
        else
        {
			entityMenuCanvas.alpha = 0.2f;
			entityMenuCanvas.interactable = false;
			selectARoomTuto.gameObject.SetActive(true);
			selectEntityTuto.gameObject.SetActive(false);
		}
    }
}
