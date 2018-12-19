using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public GameObject[] hazards;
    public Vector3 spawnValues;
    public int hazardCount;
    public float spawnWait;
    public float startWait;
    public float waveWait;

    public Text scoreText;
    //public Text restartText;
    public Text gameOverText;
    public GameObject restartButton;
    //public GameObject quitButton;

    private bool gameOver;
    private bool restart;
    private int score;
    //private bool escaped;

    void Start()
    {
        gameOver = false;
        restart = false;
        //restartText.text = "";
        restartButton.SetActive(false);
        gameOverText.text = "";
        score = 0;
        UpdateScore();
        StartCoroutine(SpawnWaves());
    }
    /*    private void Update()
        {
            if(restart)
            {
                if(Input.GetKeyDown(KeyCode.R))
                {
                    SceneManager.LoadScene("Main");
                }
            }
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                if(escaped)
                {
                    quitButton.SetActive(false);
                    Time.timeScale = 1;
                    escaped = false;
                }
                else
                {
                    quitButton.SetActive(true);
                    Time.timeScale = 0;
                    escaped = true;
                }
            }
        }
    */
    IEnumerator SpawnWaves()
    {
        yield return new WaitForSeconds(startWait);
        while (true)
        {
            for (int i = 0; i < hazardCount; i++)
            {
                GameObject hazard = hazards[Random.Range(0, hazards.Length)];
                Vector3 spawnPosition = new Vector3(Random.Range(-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
                Quaternion spawnRotation = Quaternion.identity;
                Instantiate(hazard, spawnPosition, spawnRotation);
                yield return new WaitForSeconds(spawnWait);
            }
            yield return new WaitForSeconds(waveWait);
            if (gameOver)
            {
                restartButton.SetActive(true);
                //restartText.text = "Press 'R' for Restart";
                restart = true;
                break;
            }
        }
    }
    public void AddScore(int newScoreValue)
    {
        score += newScoreValue;
        UpdateScore();
    }
    void UpdateScore()
    {
        scoreText.text = "Score: " + score;
    }
    public void GameOver()
    {
        gameOverText.text = "Game Over!";
        gameOver = true;
    }
    //public void QuitGame()
    //{
    //    Application.Quit();   
    //}
    public void RestartGame()
    {
        SceneManager.LoadScene("Main");
    }
}
