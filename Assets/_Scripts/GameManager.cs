
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    [SerializeField] Button playButton;
    
    private void Awake()
    {
        playButton.onClick.AddListener(LoadGameScene);
        
    }

    private void LoadGameScene()
    {
        SceneManager.LoadScene(1);
    }

    
}
