using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level : MonoBehaviour
{

    [SerializeField] float delayInSeconds = 1f;

    private void Update()
    {
        ManualRestart();
    }

    private void ManualRestart()
    {
        int retry = SceneManager.GetActiveScene().buildIndex;
        if (Input.GetKey(KeyCode.R))
        {
            SceneManager.LoadScene(retry);
        }
    }

    public void Retry()
    {
        SceneManager.LoadScene(1);
        FindObjectOfType<GameSession>().ResetGame();
    }

    public int NumberOfScenes()
    {
        int numberOfScenes = SceneManager.sceneCount +1;
        return numberOfScenes; //Returns number of a last scene
    }
    
    public void LoadGameScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
        FindObjectOfType<GameSession>().ResetGame();
    }

    public void LoadGameOver()
    {
            StartCoroutine(Delay());
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(delayInSeconds);
        SceneManager.LoadScene(NumberOfScenes());
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void LoadStartMenu()
    {
        SceneManager.LoadScene(0);
    }
}
