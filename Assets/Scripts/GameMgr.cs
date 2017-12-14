using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMgr : MonoBehaviour
{
    public static GameMgr instance = null;
    public GameObject PauseMenu;
    private bool isPaused;

    void Awake()
    {
        //Check if instance already exists
        if (instance == null)
            //if not, set instance to this
            instance = this;

        //If instance already exists and it's not this:
        else if (instance != this)
            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);

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
