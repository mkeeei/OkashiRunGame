using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class Athletic02Manager : MonoBehaviour
{
    public enum GameState { Ready, Play, GameOver }
    public GameState State { get; private set; }

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI readyText;      // "Ready?" 表示用
    [SerializeField] private TextMeshProUGUI gameOverText;   // "Game Over" 表示用

    [Header("Player")]
    [SerializeField] private MonoBehaviour[] playerControllers;
    // プレイヤー移動スクリプト（PlayerFlappy など）をインスペクターでセット

    private void Start()
    {
        SetState(GameState.Ready);
    }

    private void Update()
    {
        switch (State)
        {
            case GameState.Ready:
                if (Input.GetKeyDown(KeyCode.Space))
                    SetState(GameState.Play);
                break;

            case GameState.Play:
                // プレイ中は特にキー受付なし
                break;

            case GameState.GameOver:
                if (Input.GetKeyDown(KeyCode.Space))
                    ReloadScene();
                break;
        }
    }

    public void SetState(GameState newState)
    {
        State = newState;
        switch (newState)
        {
            case GameState.Ready:
                Time.timeScale = 0f;             // ゲーム停止
                readyText.gameObject.SetActive(true);
                gameOverText.gameObject.SetActive(false);
                SetPlayerEnabled(false);
                break;

            case GameState.Play:
                Time.timeScale = 1f;             // ゲーム再開
                readyText.gameObject.SetActive(false);
                gameOverText.gameObject.SetActive(false);
                SetPlayerEnabled(true);
                break;

            case GameState.GameOver:
                Time.timeScale = 0f;             // ゲーム停止
                readyText.gameObject.SetActive(false);
                gameOverText.gameObject.SetActive(true);
                SetPlayerEnabled(false);
                break;
        }
    }

    private void SetPlayerEnabled(bool enabled)
    {
        foreach (var ctrl in playerControllers)
            ctrl.enabled = enabled;
    }

    public void OnPlayerDead()
    {
        if (State == GameState.Play)
            SetState(GameState.GameOver);
    }

    private void ReloadScene()
    {
        // 現在のシーンを再読み込み
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
