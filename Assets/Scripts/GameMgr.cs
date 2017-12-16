using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMgr : MonoBehaviour
{
    public static GameMgr instance = null;
    public GameObject PauseMenu;
    private bool isPaused;

    // private int lives;
    private string currScene;
    public Canvas winCanvas;
    public Canvas deathCanvas;
    public float Timer;
    public int DeathCount;

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
        // lives = 3;
        currScene = SceneManager.GetActiveScene().name;
        Timer = 0.0f;
        DeathCount = 0;
    }

    void Update()
    {
        if (Input.GetKeyDown("escape") && isPaused == false && SceneManager.GetActiveScene().name != "DoozyMenu")
        {
            PauseMenu.SetActive(true);
            Time.timeScale = 0.0f;
            isPaused = true;
        }
        else if (Input.GetKeyDown("escape") && isPaused == true && SceneManager.GetActiveScene().name != "DoozyMenu")
        {
            PauseMenu.SetActive(false);
            Time.timeScale = 1.0f;
            isPaused = false;
        }
        if (isPaused == false)
            PauseMenu.SetActive(false);
        if (SceneManager.GetActiveScene().name == "Level1" || SceneManager.GetActiveScene().name == "Level2" || SceneManager.GetActiveScene().name == "Level3")
            Timer += Time.deltaTime;
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

    public void Die()
    {
        // lives -= 1;
        // if (lives > 0)
        //     deathCanvas.gameObject.SetActive(true);
        // else
        //     SceneManager.LoadScene("DoozyMenu");
        deathCanvas.gameObject.SetActive(true);
        Time.timeScale = 0f;
        DeathCount += 1;
        Text DeathCountText = deathCanvas.transform.Find("DeathCount").GetComponent<Text>();
        DeathCountText.text = "Deaths: " + DeathCount.ToString();
        Text TimerText = deathCanvas.transform.Find("TotalTime").GetComponent<Text>();
        TimerText.text = "Play Time: " + Timer.ToString("#.00") + "s";

    }

    public void Win()
    {
        if (SceneManager.GetActiveScene().name == "Level1")
            SceneManager.LoadScene("Level2");
        else if (SceneManager.GetActiveScene().name == "Level2")
            SceneManager.LoadScene("Level3");
        else
        {
            Time.timeScale = 0f;
            winCanvas.gameObject.SetActive(true);
        }
        Text DeathCountText = winCanvas.transform.Find("DeathCount").GetComponent<Text>();
        DeathCountText.text = "Deaths: " + DeathCount.ToString();
        Text TimerText = winCanvas.transform.Find("TotalTime").GetComponent<Text>();
        TimerText.text = "Play Time: " + Timer.ToString("#.00") + "s";

    }
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        deathCanvas.gameObject.SetActive(false);
        winCanvas.gameObject.SetActive(false);
        Time.timeScale = 1.0f;
        if (SceneManager.GetActiveScene().name == "DoozyMenu")
        {
            DeathCount = 0;
            Timer = 0.0f;
        }
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
