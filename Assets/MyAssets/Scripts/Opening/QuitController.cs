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
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();

    }
}
