using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Toggle full_screen;
    public Dropdown dropdown;
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void QuitGame()
    {
        Debug.Log("Quit!");
        Application.Quit();
    }
    public void SetScreenSize()
    {
        string screen_size = dropdown.options[dropdown.value].text;
        string[] resolutions = screen_size.Split(' ', 'х', 'x');
        if (full_screen.isOn == true)
        {
            Screen.SetResolution(int.Parse(resolutions[0]), int.Parse(resolutions[3]), FullScreenMode.FullScreenWindow);
            Debug.Log("we did it!");
        }
        else           
        {
            Screen.SetResolution(int.Parse(resolutions[0]), int.Parse(resolutions[3]), FullScreenMode.Windowed);
            Debug.Log("we did it!");
        }

    }
}
