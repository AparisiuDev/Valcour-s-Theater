using ElmanGameDevTools.PlayerSystem;
using UnityEngine;
using UnityEngine.Audio;

public class PauseMenu : MonoBehaviour
{

    public GameObject container;
    public GameObject pausemenuoptions;
    public AudioMixer audioMixer;


    private bool inOptions = false;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!container.activeSelf && !inOptions)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                // Abrir pause
                container.SetActive(true);
                Time.timeScale = 0;
            }
            else if (inOptions)
            {
                // Volver de options al pause
                Back();
            }
            else
            {
                // Cerrar pause
                Resume();
            }
        }
    }

    public void Resume()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        container.SetActive(false);
        Time.timeScale = 1;

    }

    public void Options()
    {

        container.SetActive(false);
        pausemenuoptions.SetActive(true);
        inOptions = true;

    }

    public void SetVolume(float volume)
    {

        audioMixer.SetFloat("volume", volume);

    }

    public void SetFullScreen(bool isFullscreen)
    {

        Screen.fullScreen = isFullscreen;

    }

    public void Back()
    {

        container.SetActive(true);
        pausemenuoptions.SetActive(false);
        inOptions = false;

    }

    public void MainMenu()
    {
        container.SetActive(false);
        Time.timeScale = 1;
        UnityEngine.SceneManagement.SceneManager.LoadScene("menu");

    }

}
