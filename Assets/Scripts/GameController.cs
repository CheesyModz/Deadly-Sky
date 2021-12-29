using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public bool isPaused;

    public bool isGameOver;

    public GameObject pauseMenu;

    public GameObject gameOverMenu;

    public Enemy enemyPrefab;

    private Vector2 enemySpawnIntervalRange = new Vector2(1.5f, 3f);

    private Vector2 enemySpawnMinPosition = new Vector2(-17f, 8f);

    private Vector2 enemySpawnMaxPosition = new Vector2(17f, 8f);


    // Start is called before the first frame update
    void Start()
    {
        pauseMenu.SetActive(false);
        gameOverMenu.SetActive(false);
        Invoke("SpawnEnemy", Random.Range(enemySpawnIntervalRange.x, enemySpawnIntervalRange.y));
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPaused && !isGameOver) Pause();
            else if (isPaused && !isGameOver) Resume();
        }
    }

    void SpawnEnemy()
    {
        Instantiate(enemyPrefab, new Vector2(Random.Range(enemySpawnMinPosition.x, enemySpawnMaxPosition.x),
                                            Random.Range(enemySpawnMinPosition.y, enemySpawnMaxPosition.y)), Quaternion.identity);

        Invoke("SpawnEnemy", Random.Range(enemySpawnIntervalRange.x, enemySpawnIntervalRange.y));
    }

    void Pause()
    {
        Time.timeScale = 0f;
        isPaused = true;

        pauseMenu.SetActive(true);
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        isPaused = false;

        pauseMenu.SetActive(false);
    }

    public void GameOver()
    {
        isGameOver = true;
        gameOverMenu.SetActive(true);
    }

    public void Restart()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void Exit()
    {
        Application.Quit();
    }
}
