using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GameManager : MonoBehaviour {
    [SerializeField] Button musicButton, optionsButton, ExitButton, PlayButton;
    [SerializeField] RectTransform musicPanel, optionsPanel;

    RectTransform currentPanel;

    void Awake() {
        musicButton.onClick.AddListener(() => Open(musicPanel));
        optionsButton.onClick.AddListener(() => Open(optionsPanel));

        PlayButton.onClick.AddListener(CloseCurrent);
        ExitButton.onClick.AddListener(QuitGame);

        musicPanel.gameObject.SetActive(false);
        optionsPanel.gameObject.SetActive(false);
    }

    void Open(RectTransform panel) {
        if (currentPanel)
            currentPanel.gameObject.SetActive(false);

        currentPanel = panel;

        panel.gameObject.SetActive(true);
        panel.localScale = Vector3.zero;

        panel.DOScale(1.1f, 0.25f).SetEase(Ease.OutBack)
             .OnComplete(() => panel.DOScale(1f, 0.15f));
    }

    public void CloseCurrent() {
        if (!currentPanel) return;

        currentPanel.DOScale(0, 0.2f)
            .SetEase(Ease.InBack)
            .OnComplete(() => {
                currentPanel.gameObject.SetActive(false);
                currentPanel = null; // ✅ reset
            });
    }

    void QuitGame() {
        Debug.Log("Quit Game");

        Application.Quit();

    }
}