using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    [SerializeField] Button playButton, musicButton, optionsButton;
    [SerializeField] RectTransform musicPanel, optionsPanel;
    [SerializeField] CanvasGroup musicCanvas, optionsCanvas;

    RectTransform currentPanel;
    CanvasGroup currentCanvas;

    void Awake()
    {
        playButton.onClick.AddListener(() => SceneManager.LoadScene(1));

        musicButton.onClick.AddListener(() => Open(musicPanel, musicCanvas));
        optionsButton.onClick.AddListener(() => Open(optionsPanel, optionsCanvas));

        Init(musicPanel, musicCanvas);
        Init(optionsPanel, optionsCanvas);
    }

    void Init(RectTransform p, CanvasGroup c)
    {
        p.localScale = Vector3.zero;
        c.alpha = 0;
        c.interactable = c.blocksRaycasts = false;
    }

    void Open(RectTransform p, CanvasGroup c)
    {
        if (currentPanel) Close(currentPanel, currentCanvas);

        currentPanel = p;
        currentCanvas = c;

        c.interactable = c.blocksRaycasts = true;
        p.localScale = Vector3.zero;

        c.DOFade(1, 0.3f);
        p.DOScale(1.1f, 0.25f).SetEase(Ease.OutBack)
         .OnComplete(() => p.DOScale(1f, 0.15f));
    }

    public void CloseCurrent()
    {
        if (currentPanel) Close(currentPanel, currentCanvas);
    }

    void Close(RectTransform p, CanvasGroup c)
    {
        c.interactable = c.blocksRaycasts = false;
        c.DOFade(0, 0.2f);
        p.DOScale(0, 0.2f).SetEase(Ease.InBack);
    }
}