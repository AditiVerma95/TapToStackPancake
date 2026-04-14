using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Play : MonoBehaviour
{
    [SerializeField] Button playButton;

    void Awake() {
        playButton.onClick.AddListener(() => SceneManager.LoadScene(1));
    }
}
