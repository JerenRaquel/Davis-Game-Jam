using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {
    #region Class Instance
    public static GameController instance = null;
    private void CreateInstance() {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }
    #endregion
    private void Awake() => CreateInstance();

    [HideInInspector] public EnemySpawner activeSpawner = null;
    public AudioSource audioSource;
    public GameObject gameoverScreen;
    public GameObject winScreen;

    private void Start() {
        audioSource.Play();
    }

    public void GameOver() {
        Pause();
        gameoverScreen.SetActive(true);
    }

    public void Win() {
        Pause();
        winScreen.SetActive(true);
    }

    public void NewGame() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void Pause() {
        if (activeSpawner != null) {
            activeSpawner.SwapAIState(false);
        }
    }
}
