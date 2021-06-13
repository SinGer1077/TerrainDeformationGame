using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject controlMenu;
    public GameObject pauseMenu;
    public void QuitToMenu()
    {        
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
    public void QuitToDesktop()
    {
        Debug.Log("Quit!");
        Application.Quit();
    }
    public void ControlShow()
    {
        if (controlMenu.activeSelf == false)
        {
            pauseMenu.SetActive(false);
            controlMenu.SetActive(true);
        }
    }
    public void ControlBack()
    {
        if (controlMenu.activeSelf == true)
        {
            controlMenu.SetActive(false);
            pauseMenu.SetActive(true);
        }
    }
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
