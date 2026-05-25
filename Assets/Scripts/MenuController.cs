using UnityEngine;

public class MenuController : MonoBehaviour
{
    [Tooltip("Drag the main Menu object here in the Inspector")]
    public GameObject menuWindow;

    public void CloseMenu()
    {
        if (menuWindow != null)
        {
            menuWindow.SetActive(false);
            Debug.Log("Menu closed via MenuController script!");
        }
        else
        {
            Debug.LogWarning("Menu Window is not assigned in the MenuController!");
        }
    }

    public void OpenMenu()
    {
        if (menuWindow != null)
        {
            menuWindow.SetActive(true);
        }
    }
}