using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    public CanvasGroup bg;

    private void Awake()
    {
        ResumeGame();
    }
    public void PauseGame()
    {
        Time.timeScale = 0;
        bg.blocksRaycasts = true;
        bg.alpha = 1f;

    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        bg.blocksRaycasts = false;
        bg.alpha = 0f;
    }
    public void MenuInput(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            if (Time.timeScale == 0)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }

    }
}
