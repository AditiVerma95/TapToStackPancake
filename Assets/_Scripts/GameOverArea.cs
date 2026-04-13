using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverArea : MonoBehaviour
{
    public GameObject gameOverRoot;
    public static bool isGameOver = false;
    public CanvasGroup grayPanel;
    public RectTransform gameOverText;

    public RectTransform buttonsContainer;
    public Button replayButton;
    public Button quitButton;


    private void Start()
    {
        gameOverRoot.SetActive(false);
        grayPanel.alpha = 0f;


        buttonsContainer.localScale = Vector3.zero;


        replayButton.onClick.AddListener(OnReplay);
        quitButton.onClick.AddListener(OnQuit);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isGameOver) return;

        if (other.CompareTag("Pancake"))
        {
            GameOver();
        }

        Debug.Log(other.name);
    }

    void GameOver()
    {
        if (isGameOver) return;

        isGameOver = true; // 👈 SET FLAG

        Debug.Log("GAME OVER");

        gameOverRoot.SetActive(true);

        grayPanel.DOFade(1f, 0.3f);

        // Text bounce
        gameOverText.localScale = Vector3.zero;
        gameOverText.DOScale(1.2f, 0.3f)
            .SetEase(Ease.OutBack)
            .OnComplete(() =>
            {
                gameOverText.DOScale(1f, 0.15f);
            });

        DOVirtual.DelayedCall(2f, () =>
        {
            buttonsContainer.DOScale(1.2f, 0.3f)
                .SetEase(Ease.OutBack)
                .OnComplete(() =>
                {
                    buttonsContainer.DOScale(1f, 0.15f);
                });
        });
        
    }


    void OnReplay()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(1);
    }


    void OnQuit()
    {
        Debug.Log("QUIT GAME");
        Application.Quit();
    }
}