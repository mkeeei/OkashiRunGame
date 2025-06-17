using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class QuitController : MonoBehaviour
{
    [SerializeField] private Button quitButton;
    private void Start()
    {
        SetEvent();
    }

    void SetEvent()
    {
        quitButton.onClick.AddListener(() => EndGame());
    }

    public void EndGame()
    {
        UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();

    }
}
