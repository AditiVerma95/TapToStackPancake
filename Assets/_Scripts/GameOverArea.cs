using UnityEngine;

public class GameOverArea : MonoBehaviour
{
    public GameObject gameOverUI; // assign in inspector

    private bool isGameOver = false;

    private void OnTriggerEnter(Collider other)
    {
        if (isGameOver) return;

        if (other.CompareTag("Pancake"))
        {
            GameOver();
        }Debug.Log(other.name);

    }

    void GameOver()
    {
        isGameOver = true;

        Debug.Log("GAME OVER");

        // Show UI
        gameOverUI.SetActive(true);

        // Stop game
        Time.timeScale = 0f;
    }
}