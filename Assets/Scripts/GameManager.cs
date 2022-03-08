using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    int score;
    public Text scoreText;
    public GameObject retryPanel;
    public RectTransform timerRect;
    [HideInInspector] public float timer;
    [HideInInspector] public bool gameIsRunning;

    private void Start() {
        timer = 10;
        gameIsRunning = true;
        score = 0;
    }

    private void Update() {
        if (gameIsRunning) {
            timer -= Time.deltaTime;
            timerRect.localScale = new Vector3(timer / 10f, 1f, 1f);
            timerRect.localPosition = new Vector3(-509f * (10f - timer - 0.2f) / 10f, 0f, 0f);
            if (timer <= 0) {
                GameOver();
            }
        }
    }

    public void AddScore(int value) {
        score += value;
        UpdateScore(score);
    }

    void UpdateScore(int value) {
        scoreText.text = "SCORE : " + value;
    }

    public void GameOver() {
        gameIsRunning = false;
        retryPanel.SetActive(true);
    }

    public void RestartGame() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
