using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMgr : MonoBehaviour
{
    public GameObject PauseMenu;
    private bool isPaused;

    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }

    void Start()
    {
        isPaused = false;
    }

    void Update()
    {
        if (Input.GetKeyDown("escape") && isPaused == false)
        {
            PauseMenu.SetActive(true);
            Time.timeScale = 0.0f;
            isPaused = true;
        }
        else if (Input.GetKeyDown("escape") && isPaused == true)
        {
            PauseMenu.SetActive(false);
            Time.timeScale = 1.0f;
            isPaused = false;
        }
        if (isPaused == false)
            PauseMenu.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ResumeGame()
    {
        Time.timeScale = 1.0f;
        isPaused = false;
    }

    public void ChangeToScene(string SceneToChange)
    {
        isPaused = false;
        SceneManager.LoadScene(SceneToChange);
    }
}
