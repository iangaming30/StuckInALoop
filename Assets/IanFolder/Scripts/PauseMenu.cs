using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    private bool isPaused = false;
    private Button currentlyPressedButton;

    void Update()
    {
        if (Input.GetButtonDown("Pause"))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        if (isPaused)
        {
            Resume();
        }
        else
        {
            Pause();
        }
    }

    public void Pause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0;
        isPaused = true;
    }

    public void Home()
    {
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1;
        isPaused = false;
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
        isPaused = false;
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
        isPaused = false;
    }

    public void OnButtonPress(Button button)
    {
        if (currentlyPressedButton != null)
        {
            Debug.Log("Another button is already pressed. Ignoring click.");
            return;
        }

        currentlyPressedButton = button;
        Debug.Log("Button action started.");
        Invoke("ResetButtonPress", 0.5f);
    }

    private void ResetButtonPress()
    {
        currentlyPressedButton = null;
        Debug.Log("Button action completed. Ready for next click.");
    }
}
