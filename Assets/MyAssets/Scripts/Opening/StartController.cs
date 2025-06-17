using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartController : MonoBehaviour
{
    [SerializeField] private Button startButton;


    private void Start()
    {
        SetEvent();
    }

    void SetEvent()
    {
        startButton.onClick.AddListener(() => StartGame());
    }

    public void StartGame()
    {
        Debug.Log("ƒ{ƒ^ƒ“‰Ÿ‚³‚ê‚½‚æ");
        SceneManager.LoadScene("AthleticScene01");
    }

}
